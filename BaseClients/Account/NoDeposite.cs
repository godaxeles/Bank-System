using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClients.Account
{
    public class NoDeposite<T> : BaseAccount<T>, IAccountCovariant<T, BaseAccount<T>>
        where T: BaseClient
    {
        public NoDeposite() : base() { }
        public NoDeposite(ulong Num, float Balance)
        {
            this.NumAccount = Num;
            this.Balance = Balance;
        }

        public NoDeposite(ulong Num) : this(Num, 0)
        {
            this.IsActive = true;
        }

        public void PutMoney(float Money)
        {
            Balance += Money;
            MessageAction?.Invoke($"На счёт {NumAccount} внесено {Money}");
        }

    }
}
