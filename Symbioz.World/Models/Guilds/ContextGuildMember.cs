using Symbioz.Protocol.Enums;
using Symbioz.World.Models.Entities;
using Symbioz.World.Network;

namespace Symbioz.World.Models.Guilds {
    public class ContextGuildMember {
        public long CharacterId { get; set; }
        public ushort Rank { get; set; }
        public ulong GivenExperience { get; set; }

        /// <summary>
        /// Majuscule sur le e u_u
        /// </summary>
        public sbyte ExperienceGivenPercent { get; set; }

        public uint Rights { get; set; }

        public bool Connected {
            get { return WorldServer.Instance.IsOnline(CharacterId); }
        }

        public ushort MoodSmileyId { get; set; }
        public int AchievementPoints { get; set; }


        public static ContextGuildMember New(Character character, bool isBoss) {
            return new ContextGuildMember() {
                AchievementPoints = 0,
                CharacterId = character.Id,
                ExperienceGivenPercent = 0,
                GivenExperience = 0,
                MoodSmileyId = 0,
                Rank = (ushort) (isBoss ? 1 : 0),
                Rights = (uint) (isBoss ? GuildRightsBitEnum.GUILD_RIGHT_BOSS : GuildRightsBitEnum.GUILD_RIGHT_NONE),
            };
        }
    }
}