using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.World.Models.Entities;
using Symbioz.World.Providers.Criterias;
using Symbioz.World.Providers.Maps.Npcs;
using Symbioz.World.Records.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Protocol.Selfmade.Enums;

namespace Symbioz.World.Models.Dialogs
{
    public class NpcTalkDialog : Dialog
    {
        public override DialogTypeEnum DialogType
        {
            get { return DialogTypeEnum.DIALOG_DIALOG; }
        }

        private Npc Npc { get; set; }

        private NpcActionRecord Action { get; set; }

        private ushort MessageId { get; set; }

        private List<NpcReplyRecord> Replies { get; set; }

        public NpcTalkDialog(Character character, Npc npc, NpcActionRecord action)
            : base(character)
        {
            this.Npc = npc;
            this.Action = action;
            this.MessageId = ushort.Parse(this.Action.Value1);
            this.Replies = this.GetPossibleReply(NpcReplyRecord.GetNpcReplies(this.MessageId));
        }
        public override void Open()
        {
            this.Character.Client.Send(new NpcDialogCreationMessage(this.Npc.SpawnRecord.MapId, (int) this.Npc.Id));
            this.DialogQuestion();
            
            // Admin purposes
            // if (this.Character.Client.Account.Role >= ServerRoleEnum.Administrator) {
            //     this.Character.Reply($"Npc Message {this.MessageId} replies:");
            //     foreach (var reply in this.Replies) {
            //         this.Character.Reply($"{reply.ReplyId}");
            //     }
            // }
        }
        public void DialogQuestion()
        {
            this.Character.Client.Send(new NpcDialogQuestionMessage(this.MessageId, new string[] { "0" }, this.Replies.ConvertAll<ushort>(x => x.ReplyId).Distinct().ToArray()));
        }
        public override void Close()
        {
            base.Close();
            this.LeaveDialogMessage();
        }
        public void Reply(ushort replyId)
        {
            this.Close();

            List<NpcReplyRecord> replies = this.Replies.FindAll(x => x.ReplyId == replyId);

            foreach (var reply in replies)
            {
                if (reply != null && reply.ActionType != string.Empty)
                    NpcReplyProvider.Handle(this.Character, reply);
            }

        }
        private List<NpcReplyRecord> GetPossibleReply(List<NpcReplyRecord> replies)
        {
            List<NpcReplyRecord> results = new List<NpcReplyRecord>();

            foreach (var reply in replies)
            {
                if (CriteriaProvider.EvaluateCriterias(this.Character.Client, reply.Condition))
                {
                    results.Add(reply);
                }
                else
                {
                    if (reply.ConditionExplanation != null && reply.ConditionExplanation != string.Empty)
                    {
                        this.Character.Reply(reply.ConditionExplanation);
                    }
                }
            }
            return results;
        }
    }
}
