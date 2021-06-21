using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using Tgs.MessageQueue;
using Tgs.Payment.Protos;

namespace Tgs.Payment.EeziePay.MessageQueue
{
    internal class DepositProcessor : IMessageProcessor, IMessageExceptionProcessor
    {
        private readonly EeziePaySerializer _eeziePaySerializer;
        private readonly EeziePayClient _eeziePayClient;
        private readonly EeziePayOptions _options;

        public DepositProcessor(EeziePaySerializer eeziePaySerializer, EeziePayClient eeziePayClient, IOptions<EeziePayOptions> optionsAccessor)
        {
            _eeziePaySerializer = eeziePaySerializer ?? throw new ArgumentNullException(nameof(eeziePaySerializer));
            _eeziePayClient = eeziePayClient ?? throw new ArgumentNullException(nameof(eeziePayClient));
            _options = optionsAccessor.Value;
        }
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

        public ValueTask<byte[]> HandleExceptionAsync(byte[] data, Exception exception, CancellationToken cancellationToken = default)
        {
            //TODO: DepositProcessor HandleException
            throw new NotImplementedException();
        }
    }
}
