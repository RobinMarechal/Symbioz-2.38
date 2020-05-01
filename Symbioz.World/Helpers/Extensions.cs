using System.Text.RegularExpressions;

namespace Symbioz.World.Helpers {
    public static class Extensions {
        public static string StripMargin(this string s)
        {
            return Regex.Replace(s, @"[ \t]+\|", string.Empty);
        }
    }
}