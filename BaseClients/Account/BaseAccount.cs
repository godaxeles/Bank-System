using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BaseClients.Account
{
    public abstract class BaseAccount<T> : INotifyPropertyChanged, IAccountContrvariant<T, BaseAccount<T>>
        where T : BaseClient
    {
        private float _Balance;
        public ulong NumAccount { get; set; }
       

        public float Balance
        {
            get => _Balance;
            set => Set(ref _Balance, value);
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected Action<string> _messageAction;

        public Action<string> MessageAction
        {
            get { return _messageAction; }
            set { _messageAction = value; }
        }

        public bool IsActive { get; set; } = true;

        public bool WithdrawMoney(float moneyCount)
        {
            if (moneyCount < _Balance)
            {
                Balance -= moneyCount;
                return true;
            }
            else
            {
                MessageAction?.Invoke("Недостаточно средств на балансе!");
                return false;
            }
        }

        public void TransAccountToAccount(BaseAccount<T> toAccount, float Summ)
        {
            if (Summ > 10000) throw new MoneyExeption();
            if (toAccount != null)
            {
                if (this.WithdrawMoney(Summ))
                {
                    (toAccount as IAccountCovariant<T, BaseAccount<T>>)?.PutMoney(Summ);
                    MessageAction?.Invoke($"{Summ} переведено со счёта {this.NumAccount} на счёт {toAccount.NumAccount}");
                }
            }
        }

        public BaseAccount() { }


        public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public bool Set<S>(ref S field, S value, [CallerMemberName] string Propertyname = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(Propertyname);
            return true;
        }

        
    }
}
