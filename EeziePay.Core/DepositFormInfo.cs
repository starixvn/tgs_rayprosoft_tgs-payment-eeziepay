using System;

namespace Tgs.Payment.EeziePay
{
    public record DepositFormInfo(
       Guid Id,
       string PlayerCode,
       Guid BrandId,
       Guid BrandPaymentId,
       decimal Amount,
       int Status,
       EeziePayDepositApplyInfo? ApplyInfo,
       string? PaymentReturn = null);
}