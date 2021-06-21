namespace Tgs.Payment.EeziePay
{
    public class EeziePayWithdrawReturn
    {
        public string ServiceVersion { get; init; } = default!;
        public string Sign { get; init; } = default!;
        public string BillNo { get; init; } = default!;
        public string PartnerOrderId { get; init; } = default!;
        public string Currency { get; init; } = default!;
        public string Amount { get; init; } = default!;
        public string AccountName { get; init; } = default!;
        public string AccountNumber { get; init; } = default!;
        public string BankCode { get; init; } = default!;
        public string Status { get; init; } = default!;
        public string Fee { get; init; } = default!;
        public string BankCharge { get; init; } = default!;
        public string? ErrorCode { get; init; }
        public string? ErrorDescription { get; init; }
    }
}