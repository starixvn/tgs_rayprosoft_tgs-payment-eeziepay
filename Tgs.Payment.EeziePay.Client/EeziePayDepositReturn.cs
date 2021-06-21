namespace Tgs.Payment.EeziePay
{
    public class EeziePayDepositReturn
    {
        public string ServiceVersion { get; init; } = default!;
        public string Sign { get; init; } = default!;
        public string BillNo { get; init; } = default!;
        public string PartnerOrderId { get; init; } = default!;
        public string Currency { get; init; } = default!;
        public string RequestAmount { get; init; } = default!;
        public string Fee { get; init; } = default!;
    }
}