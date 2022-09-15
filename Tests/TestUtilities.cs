using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal static class TestUtilities
    {
        public static Token RandomTokenGenerator(bool AllowWhites)
        {
            int[] vals = new int[] { 1, 2, 4 };
            Type type = typeof(TokenColor);
            Array values = type.GetEnumValues();
            int index = Consts.r.Next(values.Length);
            TokenColor color = TokenColor.orange;
            while (AllowWhites || color != TokenColor.white)
            {
                color = (TokenColor)values.GetValue(index);
            }
            if (color == TokenColor.white)
                return new Token(color, Consts.r.Next(0, 4));
            return new Token(color, vals[Consts.r.Next(0, vals.Length)]);
        }

        public static List<Token> 
    }
}
