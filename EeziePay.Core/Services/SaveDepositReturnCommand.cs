using System;

namespace Tgs.Payment.EeziePay.Services
{
    public record SaveDepositReturnCommand(Guid FormId, EeziePayDepositReturn DepositReturn);
}