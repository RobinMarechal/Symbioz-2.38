using Symbioz.Protocol.Types;

#pragma warning disable 659

namespace Symbioz.World.Models.Guilds {
    public class ContextGuildEmblem {
        public ushort SymbolShape { get; set; }
        public int SymbolColor { get; set; }
        public sbyte BackgroundShape { get; set; }
        public int BackgroundColor { get; set; }

        public static ContextGuildEmblem New(GuildEmblem guildEmblem) {
            return new ContextGuildEmblem {
                BackgroundColor = guildEmblem.backgroundColor,
                BackgroundShape = guildEmblem.backgroundShape,
                SymbolColor = guildEmblem.symbolColor,
                SymbolShape = guildEmblem.symbolShape,
            };
        }

        public GuildEmblem ToGuildEmblem() {
            return new GuildEmblem(this.SymbolShape, this.SymbolColor, this.BackgroundShape, this.BackgroundColor);
        }

        public override bool Equals(object obj) {
            return obj != null
                   && obj is ContextGuildEmblem emblem
                   && emblem.BackgroundColor == this.BackgroundColor
                   && emblem.BackgroundShape == this.BackgroundShape
                   && emblem.SymbolColor == this.SymbolColor
                   && emblem.SymbolShape == this.SymbolShape;
        }
    }
}