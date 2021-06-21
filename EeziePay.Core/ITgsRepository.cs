using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.Payment.EeziePay
{
    public interface ITgsRepository
    {
        ValueTask<DepositFormInfo> GetDepositFormByIdAsync(Guid id);

        ValueTask<WithdrawFormInfo> GetWithdrawFormByIdAsync(Guid id);

        ValueTask SaveDepositReturnAsync(Guid id, EeziePayDepositReturn depositReturn);

        ValueTask SaveWithdrawReturnAsync(Guid id, EeziePayWithdrawReturn withdrawReturn);
    }
}
