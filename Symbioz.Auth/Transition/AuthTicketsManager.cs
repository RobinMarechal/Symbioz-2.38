using Symbioz.Core;
using Symbioz.Core.DesignPattern;
using Symbioz.Network;
using Symbioz.Protocol.Selfmade.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Auth.Transition {
    public class AuthTicketsManager : Singleton<AuthTicketsManager> {
        private List<AccountData> m_cache = new List<AccountData>();

        public void CacheToGame(AuthClient client) {
            client.Account.Ticket = new AsyncRandom().RandomString(32);
            this.m_cache.Add(client.Account);
        }

        public AccountData GetAccount(string ticket) {
            AccountData account = this.m_cache.Find(x => x.Ticket == ticket);
            this.m_cache.Remove(account);

            return account;
        }
    }
}