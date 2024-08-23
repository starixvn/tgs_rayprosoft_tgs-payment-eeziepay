using System;

namespace Tgs.Payment.EeziePay;
public record EeziePayWithdrawResponse(
    Guid FormId)
{
    public string ThirdPartyOrderNo { get; set; }
}
