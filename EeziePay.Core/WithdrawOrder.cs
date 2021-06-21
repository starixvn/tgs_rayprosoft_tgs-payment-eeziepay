using System;
using System.Text.Json.Serialization;

namespace Tgs.Payment.EeziePay
{
    public class WithdrawOrder
    {
        /// <summary>
        /// Version number of the API
        /// current used for this request.
        /// Must set as “3.0”.
        /// </summary>
        [JsonPropertyName("service_version")]
        public string ApiVersion { get; init; } = default!;

        /// <summary>
        /// Signature of the request
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
        /// Customer’s bank account name.
        /// Note for THB Payout:
        /// Account name in Thai, English or
        /// both.To pass Thai and English
        /// name, e.g. "อาทิตย์||John Doe"
        /// or else just pass "อาทิตย์" or
        /// "John Doe". Recommended to
        /// provide account name in Thai
        /// whenever possible for better
        /// accuracy.
        /// </summary>
        [JsonPropertyName("account_name")]
        public string MemberAccountName { get; init; } = default!;

        /// <summary>
        /// Customer’s bank account
        /// number
        /// </summary>
        [JsonPropertyName("account_number")]
        public string MemberAccountNumber { get; init; } = default!;

        /// <summary>
        /// The province of account opening
        /// branch.
        /// </summary>
        [JsonPropertyName("bank_province")]
        public string MemberBankProvince { get; init; } = default!;

        /// <summary>
        /// The city of account opening
        /// branch.
        /// </summary>
        [JsonPropertyName("bank_city")]
        public string MemberBankCity { get; init; } = default!;

        /// <summary>
        /// Address of the account opening
        /// branch.
        /// </summary>
        [JsonPropertyName("bank_branch")]
        public string MemberBankBranch { get; init; } = default!;

        /// <summary>
        /// Bank code that the transaction
        /// will come from.See appendix for
        /// a list of bank codes.
        /// VND: TCB.VN, SCM.VN, VCB.VN, ACB.VN, DAB.VN, VTB.VN, BIDV.VN, EXIM.VN, VBARD.VN <br>
        /// THB: KTB.TH, SCB.TH, BBL.TH, KBANK.TH, TMB.TH, BAY.TH, CIM.TH, GSB.TH, TBK.TH, UOB.TH, GHB.TH, HSBC.TH, SDCB.TH, LHB.TH<br>
        /// IDR: BRI.ID, BNI.ID, BCA.ID, MDR.ID, BAG.ID, BBKP.ID, BCW.ID, BDNM.ID, BM.ID, BMP.ID, BMTK.ID, BPNN.ID, BSMT.ID, BSNM.ID, BTPN.ID, CIMBN.ID, HSBC.ID, MBBI.ID, OCBC.ID, PMTB.ID, UOB.ID<br>
        /// MYR: AFB.MY, ALB.MY, ARB.MY, BIMB.MY, BKR.MY, BSN.MY, CIMB.MY, CITI.MY, HLB.MY, HSBC.MY, MBB.MY, OCBC.MY, PBB.MY, RHB.MY, SCB.MY, UOB.MY
        /// </summary>
        [JsonPropertyName("bank_code")]
        public string MemberDepositBankCode { get; init; } = default!;

        /// <summary>
        /// The transaction date and time
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

        /// <summary>
        /// This is the URL that will be used
        /// to notify of successful
        /// transactions status
        /// </summary>
        [JsonPropertyName("notify_url")]
        public string NotificationUrl { get; init; } = default!;
    }
}