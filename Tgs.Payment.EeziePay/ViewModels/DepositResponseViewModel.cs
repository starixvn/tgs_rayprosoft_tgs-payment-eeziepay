using System.Text.Json.Serialization;

namespace Tgs.Payment.EeziePay.ViewModels
{
    public class DepositResponseViewModel
    {
        [JsonPropertyName("service_version")]
        public string ApiVersion { get; init; } = default!;

        [JsonPropertyName("sign")]
        public string Signature { get; init; } = default!;

        [JsonPropertyName("billno")]
        public string SystemRefId { get; init; } = default!;

        [JsonPropertyName("partner_orderid")]
        public string MerchantTransacitonReference { get; init; } = default!;

        [JsonPropertyName("currency")]
        public string TransactionCurrency { get; init; } = default!;

        [JsonPropertyName("request_amount")]
        public int RequestAmount { get; init; } = default!;

        [JsonPropertyName("receive_amount")]
        public int ReceiveAmount { get; init; }

        [JsonPropertyName("fee")]
        public int DepositFeeAppliedToThisTransaction { get; init; }

        /// <summary>
        /// 000: Success<br/>
        /// 001: Pending<br/>
        /// 002: Bank Success<br/>
        /// 110: Expired<br/>
        /// 111: Fail<br/>
        /// 112: Login Error<br/>
        /// 113: Amount Error<br/>
        /// 114: Pin Error<br/>
        /// 115: Pin Timeout<br/>
        /// 116: Login Timeount<br/>
        /// 117: Account Timeout<br/>
        /// 118: Security Question error<br/>
        /// 119: User Abort
        /// 200: Refunded
        /// </summary>
        [JsonPropertyName("status")]
        public string StatusOfTheTransaction { get; init; } = default!;
    }
}