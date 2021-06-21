using System.Text.Json.Serialization;

namespace Tgs.Payment.EeziePay
{
    public class EeziePayDepositResponse
    {
        //TODO:文件沒看到詳細的Deposit回傳訊息,須測試

        /// <summary>
        /// 10025 Data Error - Merchant ID Suspended<br/>
        /// 10031 Missing value or invalid data in parameters: partner_orderid<br/>
        /// - Missing value or invalid data in parameters: partner_code<br/>
        /// - Missing value or invalid data in parameters: member_id<br/>
        /// - Missing value or invalid data in parameters: member_ip<br/>
        /// - Missing value or invalid data in parameters: bank_code<br/>
        /// - Missing value or invalid data in parameters: trans_time<br/>
        /// - Invalid or existing reference number provided for transaction<br/>
        /// - Unable to get appropriate account or partner services not set correctly<br/>
        /// - Version not supported<br/>
        /// - Invalid Signature<br/>
        /// - Bank unavailable at the moment<br/>
        /// - Member is blacklisted
        /// </summary>

        [JsonPropertyName("error_code")]
        public string? ErrorCode { get; set; }//TODO:可能的ErrorCode
    }
}