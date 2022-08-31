using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClients.Account
{
    public class Deposite<T> : BaseAccount<T>, IAccountCovariant<T, BaseAccount <T>>
        where T: BaseClient
    {
        public Deposite() : base() { }
        public Deposite(ulong Num, float Balance)
        {
            this.NumAccount = Num;
            this.Balance = Balance;
        }

        public Deposite(ulong Num) : this(Num, 0)
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
