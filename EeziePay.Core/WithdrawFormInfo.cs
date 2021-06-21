using System;

namespace Tgs.Payment.EeziePay
{
    public record WithdrawFormInfo(
       Guid Id,
       string PlayerCode,
       Guid BrandId,
       Guid? BrandPaymentId,
       decimal Amount,
       int Status,
       Guid BankInfoId,
       string? BankInfo,
       EeziePayWithdrawApplyInfo? ApplyInfo,
       string? PaymentReturn = null);
}