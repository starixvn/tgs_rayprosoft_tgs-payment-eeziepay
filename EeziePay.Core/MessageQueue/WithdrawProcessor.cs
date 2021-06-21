using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using Tgs.MessageQueue;
using Tgs.Payment.Protos;

namespace Tgs.Payment.EeziePay.MessageQueue
{
    internal class WithdrawProcessor : IMessageProcessor, IMessageExceptionProcessor
    {
        private readonly EeziePaySerializer _eeziePaySerializer;
        private readonly EeziePayOptions _options;
        private readonly EeziePayClient _eeziePayClient;

        public WithdrawProcessor(EeziePaySerializer eeziePaySerializer, IOptions<EeziePayOptions> optionsAccessor, EeziePayClient eeziePayClient)
        {
            _eeziePaySerializer = eeziePaySerializer ?? throw new ArgumentNullException(nameof(eeziePaySerializer));
            _options = optionsAccessor.Value;
            _eeziePayClient = eeziePayClient ?? throw new ArgumentNullException(nameof(eeziePayClient));
        }

        public async ValueTask<byte[]> HandleAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            var request = WithdrawRequest.Parser.ParseFrom(data);
            var settings = _eeziePaySerializer.Deserialize<EeziePayWithdrawSettings>(request.ApplyInfo)!;

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
                TransactionAmount = int.Parse(request.Amount),
                TransactionCurrency = settings.TransactionCurrency,
                MembersId = settings.MembersId,
                NotificationUrl = $"{_options.CallbackBaseUrl}/withdraw/callback"
            };

            var withdrawResponse = await _eeziePayClient
                .WithdrawAsync(settings.MerchantEncryptionKey, order)
                .ConfigureAwait(false);

            if (withdrawResponse != null)
            {
                var applyInfo = new EeziePayWithdrawApplyInfo(settings, withdrawResponse);

                var msg = new WithdrawResponse
                {
                    Result = _eeziePaySerializer.Serialize(applyInfo)
                };
                return msg.ToByteArray();
            }
            else
            {
                //TODO: Withdraw Error Handle
                var response = new EeziePayWithdrawFailResponse
                {
                };

                var applyInfo = new EeziePayWithdrawApplyInfo(settings, response);

                var msg = new WithdrawResponse
                {
                    Result = _eeziePaySerializer.Serialize(applyInfo)
                };
                return msg.ToByteArray();
            }
        }

        public ValueTask<byte[]> HandleExceptionAsync(byte[] data, Exception exception, CancellationToken cancellationToken = default)
        {
            //TODO: WithdrawProcessor HandleException
            throw new NotImplementedException();
        }
    }
}