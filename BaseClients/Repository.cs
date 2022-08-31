using BaseClients.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClients
{
    public class Repository
    {
        private List<BaseClient> _ClientBase = new();
        private string _Path;

        public Repository(string path)
        {
            _Path = path;
            _ClientBase = JsonBase.LoadBase(_Path);
            if (_ClientBase == null) _ClientBase = new List<BaseClient>();
        }

        public List<BaseClient> GetClient()
        {
            List<BaseClient> obsClient = new();
            foreach (var e in _ClientBase.Where(e => e.IsActive))
            {
                obsClient.Add(e);
            }

            return obsClient;
        }

        public void SaveBase()
        {
            JsonBase.SaveBase(_ClientBase, _Path);
        }

        public void AddClient(string first, string last, string phone, string passport, string town)
        {
            _ClientBase.Add(new BaseClient(first, last, phone, passport, town));
            JsonBase.SaveBase(_ClientBase, _Path);
        }

        public ulong GenId()
        {
            ulong maxId = 1_000_000_000;
            foreach (var client in _ClientBase)
            {
                    if ((client as BaseClient).Account?.Count > 0)
                    {
                        ulong max = (client as BaseClient).Account?.Max(i => i.NumAccount) ?? 0;
                        maxId = maxId < max ? max : maxId;
                    }
            }
            return ++maxId;

        }



    }
}
