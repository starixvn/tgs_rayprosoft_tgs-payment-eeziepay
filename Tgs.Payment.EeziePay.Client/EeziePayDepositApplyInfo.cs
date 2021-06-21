namespace Tgs.Payment.EeziePay
{
    public record EeziePayDepositApplyInfo(
        EeziePayDepositSettings Settings,
        EeziePayDepositResponse Response);
}