using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using Tgs.Payment.Clients;
using Tgs.Payment.EeziePay.Client;
using Tgs.Payment.Protos;
using Valhalla.MessageQueue;

namespace Tgs.Payment.EeziePay.MessageQueue
{
    internal class DepositProcessor(
    EeziePaySerializer eeziePaySerializer, EeziePayClient eeziePayClient, IPaymentClient paymentClient, IOptions<EeziePayOptions> optionsAccessor)
    : IMessageProcessor<DepositRequest, DepositResponse>
    {
        private readonly EeziePaySerializer _eeziePaySerializer = eeziePaySerializer;
        private readonly EeziePayClient _eeziePayClient = eeziePayClient;
        private readonly EeziePayOptions _options = optionsAccessor.Value;
        private readonly IPaymentClient _paymentClient = paymentClient;

        public async ValueTask<byte[]> HandleAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            var request = DepositRequest.Parser.ParseFrom(data);
            var settings = _eeziePaySerializer.Deserialize<EeziePayDepositSettings>(request.ApplyInfo)!;

            var order = new DepositOrder
            {
                ApiVersion = settings.ApiVersion,
                MerchantCode = settings.MerchantCode,
                MembersId = settings.MembersId,
                MerchantTransactionId = settings.MerchantTransactionId,
                TransactionAmount = int.Parse(request.Amount),
                RedirectUrl = "",
                TransactionCurrency = settings.TransactionCurrency,
                BackendUrl = $"{_options.CallbackBaseUrl}/deposit/callback"
            };

            //TODO: Prepare EeziePayDepositResponse Property
            var depositResponse = await _eeziePayClient
                .DepositAsync(settings.MerchantEncryptionKey, order)
                .ConfigureAwait(false);

            if(depositResponse != null)
            {                
                var applyinfo = new EeziePayDepositApplyInfo(settings,depositResponse);
                var msg = new DepositResponse
                {
                    Result = _eeziePaySerializer.Serialize(applyinfo)
                };
                return msg.ToByteArray();
            }
            else
            {
                //TODO: Deposit Fail Error Handle
                var response = new EeziePayDepositFailResponse() 
                {
                    
                };

                var applyinfo = new EeziePayDepositApplyInfo(settings,response);

                var msg = new DepositResponse
                {
                    Result = _eeziePaySerializer.Serialize(applyinfo)
                };
                return msg.ToByteArray();
            }            
        }

        public async ValueTask<DepositResponse> HandleAsync(string subject, DepositRequest data, IEnumerable<Valhalla.MessageQueue.MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
        {
            var settings = JsonSerializer.Deserialize<EeziePayDepositSettings>(
            data.ApplyInfo,
            EeziePayUtility.SerializerOptions)!;
            var order = new DepositOrder
            {
                ApiVersion = settings.ApiVersion,
                MerchantCode = settings.MerchantCode,
                MembersId = settings.MembersId,
                MerchantTransactionId = settings.MerchantTransactionId,
                TransactionAmount = int.Parse(data.Amount),
                RedirectUrl = "",
                TransactionCurrency = settings.TransactionCurrency,
                BackendUrl = $"{_options.CallbackBaseUrl}/deposit/callback"
            };

            //TODO: Prepare EeziePayDepositResponse Property
            var depositResponse = await _eeziePayClient
                .DepositAsync(settings.MerchantEncryptionKey, order)
                .ConfigureAwait(false);

            var applyInfo = new EeziePayDepositApplyInfo(settings, depositResponse);

            await _paymentClient.ScheduleAsync(
            $"tgs.payment.eezie.deposit.schedule.reject",
            settings.FormId,
            DateTime.UtcNow.AddMinutes(31D),
            cancellationToken).ConfigureAwait(false);

            return new DepositResponse
            {
                Result = JsonSerializer.Serialize(applyInfo, EeziePayUtility.SerializerOptions)
            };
        }

            public ValueTask<byte[]> HandleExceptionAsync(byte[] data, Exception exception, CancellationToken cancellationToken = default)
        {
            //TODO: DepositProcessor HandleException
            throw new NotImplementedException();
        }
    }
}
