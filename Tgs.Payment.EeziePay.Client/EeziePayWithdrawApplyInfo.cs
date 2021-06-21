namespace Tgs.Payment.EeziePay
{
    public record EeziePayWithdrawApplyInfo(
        EeziePayWithdrawSettings Settings,
        EeziePayWithdrawResponse Response);
}