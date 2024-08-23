using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Tgs.Payment.Clients;
using Tgs.Payment.EeziePay.Client;
using Tgs.Payment.Protos;
using Valhalla.MessageQueue;

namespace Tgs.Payment.EeziePay.MessageQueue
{
    internal class WithdrawProcessor(EeziePaySerializer eeziePaySerializer, IOptions<EeziePayOptions> optionsAccessor, EeziePayClient eeziePayClient, IPaymentClient paymentClient) : IMessageProcessor<WithdrawRequest, WithdrawResponse>
    {
        private readonly EeziePaySerializer _eeziePaySerializer = eeziePaySerializer ?? throw new ArgumentNullException(nameof(eeziePaySerializer));
        private readonly EeziePayOptions _options = optionsAccessor.Value;
        private readonly EeziePayClient _eeziePayClient = eeziePayClient ?? throw new ArgumentNullException(nameof(eeziePayClient));
        private readonly IPaymentClient _paymentClient = paymentClient ?? throw new ArgumentNullException(nameof(paymentClient));
       

        public async ValueTask<WithdrawResponse> HandleAsync(string subject, WithdrawRequest data, IEnumerable<Valhalla.MessageQueue.MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
        {
            var settings = JsonSerializer.Deserialize<EeziePayWithdrawSettings>(
            data.ApplyInfo,
            EeziePayUtility.SerializerOptions)!;
            var order = new WithdrawOrder
            {
                ApiVersion = settings.ApiVersion,
                MemberAccountName = settings.MemberAccountNumber,
                MemberAccountNumber = settings.MemberAccountNumber,
                MemberBankBranch = settings.MemberBankBranch,
                MemberBankCity = settings.MemberBankCity,
                MemberBankProvince = settings.MemberBankProvince,
                MemberDepositBankCode = settings.MemberDepositBankCode,
                MerchantCode = settings.MerchantCode,
                MerchantTransactionId = settings.MerchantTransactionId,
                TransactionAmount = int.Parse(data.Amount),
                TransactionCurrency = settings.TransactionCurrency,
                MembersId = settings.MembersId,
                NotificationUrl = $"{_options.CallbackBaseUrl}/withdraw/callback"
            };

            var withdrawResponse = await _eeziePayClient
                .WithdrawAsync(settings.MerchantEncryptionKey, order)
                .ConfigureAwait(false);

            var applyInfo = new EeziePayWithdrawApplyInfo(
            settings,
            new EeziePayWithdrawResponse(withdrawResponse.FormId));

            await _paymentClient.SaveWithdrawFormThirdPartyOrderNoAsync(
                settings.FormId,
                withdrawResponse.ThirdPartyOrderNo,
                cancellationToken);

            return new WithdrawResponse
            {
                Result = JsonSerializer.Serialize(applyInfo, EeziePayUtility.SerializerOptions)
            };
        }

        public ValueTask<byte[]> HandleExceptionAsync(byte[] data, Exception exception, CancellationToken cancellationToken = default)
        {
            //TODO: WithdrawProcessor HandleException
            throw new NotImplementedException();
        }
    }
}