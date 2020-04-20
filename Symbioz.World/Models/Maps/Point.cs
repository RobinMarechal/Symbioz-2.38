#pragma warning disable 659

namespace Symbioz.World.Models.Maps {
    public class Point {
        public int X;

        public int Y;

        public Point(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        public Point() { }

        public override bool Equals(object obj) {
            return obj != null
                   && obj is Point pt
                   && this.X == pt.X
                   && this.Y == pt.Y;
        }
    }
}