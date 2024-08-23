using System;

namespace Tgs.Payment.EeziePay.Services
{
    public record ApproveDepositFormCommand(Guid FormId, Guid BrandId, decimal? ActualAmount);
}
