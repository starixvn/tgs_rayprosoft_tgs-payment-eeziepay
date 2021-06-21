using System;

namespace Tgs.Payment.EeziePay.Services
{
    public record SaveWithdrawReturnCommand(Guid FormId, EeziePayWithdrawReturn WithdrawReturn);
}