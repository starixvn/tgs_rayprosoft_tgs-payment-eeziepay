using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tgs.Payment.Clients;
using Tgs.Payment.EeziePay;

namespace EeziePayTest
{
    internal class Demo : BackgroundService
    {
        private readonly IPaymentClient _paymentClient;
        private readonly EeziePaySerializer _eeziePaySerializer;
        private readonly ILogger<Demo> _logger;

        public Demo(IPaymentClient paymentClient, EeziePaySerializer eeziePaySerializer, ILogger<Demo> logger)
        {
            _paymentClient = paymentClient ?? throw new ArgumentNullException(nameof(paymentClient));
            _eeziePaySerializer = eeziePaySerializer ?? throw new ArgumentNullException(nameof(eeziePaySerializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var merchantEncryptionKey = "";
            var apiVersion = "3.0";
            var merchantCode = "";
            var merchantTransactionId = Guid.NewGuid();
            var membersId = "";
            var transactionCurrency = "VND";//VND,THB,IDR,MYR
            var depositSettings = new EeziePayDepositSettings(
                merchantEncryptionKey,
                apiVersion,
                merchantCode,
                merchantTransactionId,
                membersId,
                transactionCurrency,
                "https://google.com"
                );
            var depositReturn = await _paymentClient.DepositAsync("tgs.payment.eeziepay.deposit", 100000, _eeziePaySerializer.Serialize(depositSettings));
            _logger.LogInformation($"Deposit Return:", depositReturn);

            var memberAccountName = "";
            var memberAccountNumber = "";
            var memberBankProvince = "";
            var memberBankCity = "";
            var memberBankBranch = "";
            var memberDepositBankCode = "TCB.VN";
            var withdrawSettings = new EeziePayWithdrawSettings(
                merchantEncryptionKey,
                apiVersion,
                merchantCode,
                merchantTransactionId,
                membersId,
                transactionCurrency,
                memberAccountName,
                memberAccountNumber,
                memberBankProvince,
                memberBankCity,
                memberBankBranch,
                memberDepositBankCode
                );
            var withdrawReturn = await _paymentClient.WithdrawAsync("tgs.payment.eeziepay.withdraw", 100000, _eeziePaySerializer.Serialize(withdrawSettings));
            _logger.LogInformation($"Withdraw Return:", withdrawReturn);
        }
    }
}