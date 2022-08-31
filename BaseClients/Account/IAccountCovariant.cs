using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClients.Account
{
    public interface IAccountCovariant<R, out T>
        where R : BaseClient
        where T : BaseAccount<R>
    {
        public void PutMoney(float Money);
    }
}
