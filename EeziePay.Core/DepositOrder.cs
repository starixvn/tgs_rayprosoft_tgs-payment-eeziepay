using System;
using System.Text.Json.Serialization;

namespace Tgs.Payment.EeziePay
{
    public class DepositOrder
    {
        /// <summary>
        /// Version number of the API
        /// current used for this request.
        /// Must set as “3.0”.
        /// </summary>
        [JsonPropertyName("service_version")]
        public string ApiVersion { get; init; } = default!;

        /// <summary>
        /// Signature of the request.
        /// </summary>
        [JsonPropertyName("sign")]
        public string? Signature { get; set; }

        /// <summary>
        /// The partner code assigned in the
        /// system.The code is normally 8
        /// alphanumeric characters.
        /// </summary>
        [JsonPropertyName("partner_code")]
        public string MerchantCode { get; init; } = default!;

        /// <summary>
        /// Merchant’s system assigned ID
        /// for the transaction.
        /// </summary>

        [JsonPropertyName("partner_orderid")]
        public Guid MerchantTransactionId { get; init; } = default!;

        /// <summary>
        /// The member that is currently
        /// doing the transaction name.
        /// </summary>

        [JsonPropertyName("member_id")]
        public string MembersId { get; init; } = default!;

        /// <summary>
        /// The IP address of the member
        /// making a deposit.This is to
        /// prevent cross-site scripting
        /// attack.
        /// </summary>

        [JsonPropertyName("member_ip")]
        public string? MembersIp { get; init; }

        /// <summary>
        /// The currency code to represent
        /// the transaction is done in which
        /// currency.May refer the
        /// appendix of list of currencies
        /// supported.
        /// Currencies Accepted: VND, THB, IDR, MYR
        /// </summary>

        [JsonPropertyName("currency")]
        public string TransactionCurrency { get; init; } = default!;

        /// <summary>
        /// The amount that member would
        /// like to deposit.Please note this
        /// value is in cents. (For example,
        /// 1000 in cent value is 100000)

        /// </summary>

        [JsonPropertyName("amount")]
        public int TransactionAmount { get; init; }

        /// <summary>
        /// The URL to implement backend
        /// server to server notification
        /// upon successful transaction
        /// status.
        /// </summary>

        [JsonPropertyName("backend_url")]
        public string BackendUrl { get; init; } = default!;

        /// <summary>
        /// The URL to redirect member’s
        /// browser back to merchant
        /// website upon successful
        /// transaction.
        /// </summary>

        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; init; } = default!;

        /// <summary>
        /// Bank code that the transaction
        /// will come from.May refer the
        /// appendix for a list of bank codes<br/>
        /// VND: TCB.VN, SCM.VN, VCB.VN, ACB.VN, DAB.VN, VTB.VN, BIDV.VN, EXIM.VN, VBARD.VN, ACB.QR.VN, BIDV.QR.VN, VCB.QR.VN<br/>
        /// THB: KTB.TH, SCB.TH, BBL.TH, KBANK.TH, TMB.TH, BAY.TH, GSB.TH, QR.TH<br/>
        /// IDR: BCA.ID, BNI.ID, MDR.ID, BRI.ID, BNI.VA.ID, MDR.VA.ID, BRI.VA.ID, MBBI.VA.ID<br/>
        /// MYR: CIMB.MY, HLB.MY, MBB.MY, PBB.MY, RHB.MY
        /// </summary>
        [JsonPropertyName("bank_code")]
        public string? MemberDepositBankCode { get; init; }

        /// <summary>
        /// The transaction date and time.
        /// </summary>
        [JsonPropertyName("trans_time")]
        public DateTime? TransactionTime { get; init; }

        /// <summary>
        /// Additional remarks regarding
        /// this transaction that the
        /// merchant would like to input.
        /// NOTE: Do not need to put into
        /// signature string before generate
        /// sign.
        /// </summary>
        [JsonPropertyName("remarks")]
        public string? Remarks { get; init; }
    }
}