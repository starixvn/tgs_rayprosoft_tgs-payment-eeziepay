using System;
using System.Threading.Tasks;
using Tgs.Payment.EeziePay.Repositories.Entities;

namespace Tgs.Payment.EeziePay.Repositories
{
    internal class TgsRepository : ITgsRepository
    {
        private readonly TgsContext _dbContext;
        private readonly EeziePaySerializer _serializer;

        public TgsRepository(TgsContext dbContext, EeziePaySerializer serializer)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async ValueTask<DepositFormInfo> GetDepositFormByIdAsync(Guid id)
        {
            var data = await _dbContext.DepositForms.FindAsync(id).ConfigureAwait(false);

            return new DepositFormInfo(
               data.Id,
               data.PlayerCode,
               data.BrandId,
               data.BrandPaymentId,
               data.Amount,
               data.Status,
               _serializer.Deserialize<EeziePayDepositApplyInfo>(data.ApplyInfo!),
               data.PaymentReturn);
        }

        public async ValueTask<WithdrawFormInfo> GetWithdrawFormByIdAsync(Guid id)
        {
            var data = await _dbContext.WithdrawForms.FindAsync(id).ConfigureAwait(false);

            return new WithdrawFormInfo(
                data.Id,
                data.UserId!,
                data.BrandId,
                data.BrandPaymentId,
                data.Amount,
                data.Status,
                data.BankInfoId,
                data.BankInfo,
                _serializer.Deserialize<EeziePayWithdrawApplyInfo>(data.ApplyInfo!),
                data.PaymentReturn);
        }

        public async ValueTask SaveDepositReturnAsync(Guid id, EeziePayDepositReturn depositReturn)
        {
            var form = await _dbContext.DepositForms
                .FindAsync(id)
                .ConfigureAwait(false);

            form.Status = 1000;
            form.ActualAmount = decimal.Parse(depositReturn.RequestAmount);//TODO:需確認PropertyName
            form.PaymentReturn = _serializer.Serialize(depositReturn);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async ValueTask SaveWithdrawReturnAsync(Guid id, EeziePayWithdrawReturn withdrawReturn)
        {
            var form = await _dbContext.WithdrawForms
                .FindAsync(id)
                .ConfigureAwait(false);

            form.PaymentReturn = _serializer.Serialize(withdrawReturn);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}