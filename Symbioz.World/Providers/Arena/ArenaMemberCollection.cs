using SSync.Messages;
using Symbioz.Protocol.Enums;
using Symbioz.World.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Arena {
    public class ArenaMemberCollection {
        private List<ArenaMember> m_members = new List<ArenaMember>();

        public bool Empty {
            get { return this.m_members.Count == 0; }
        }

        public ArenaGroup Group { get; set; }

        public bool IsFull {
            get { return this.m_members.Count == (int) this.Group.Type; }
        }

        public bool Accepted {
            get { return this.m_members.All(x => x.Accepted); }
        }

        public ArenaMemberCollection(ArenaGroup group) {
            this.Group = group;
        }

        public long[] GetMemberIds() {
            return Array.ConvertAll(this.m_members.ToArray(), x => x.Character.Id);
        }

        public void ForEach(Action<ArenaMember> action) {
            foreach (var member in this.GetMembers()) {
                action(member);
            }
        }

        public void Send(Message message) {
            this.ForEach(x => x.Send(message));
        }

        public ArenaMember AddMember(ArenaGroup group, Character character) {
            if (this.MemberExist(character)) {
                throw new Exception("Member is already registered is this ArenaMemberCollection");
            }

            ArenaMember member = new ArenaMember(character, this);
            this.m_members.Add(member);

            return member;
        }

        public ArenaMember RemoveMember(Character character) {
            ArenaMember member = this.GetMember(character);

            if (member == null) {
                throw new Exception("Cannot remove member, he is not registered in this ArenaMemberCollection");
            }

            this.m_members.Remove(member);

            return member;
        }

        public ArenaMember GetMember(Character character) {
            return this.m_members.FirstOrDefault(x => x.Character == character);
        }

        public ArenaMember[] GetMembers() {
            return this.m_members.ToArray();
        }

        private bool MemberExist(Character character) {
            return this.GetMember(character) != null;
        }
    }
}