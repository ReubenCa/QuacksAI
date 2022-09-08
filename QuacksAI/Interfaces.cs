using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuacksAI
{
    public interface IBrew
    {
        public bool Brew(PlayerBrewData data);
        public bool Flask(PlayerBrewData data);
        public Token DecideOnBlue(PlayerBrewData data, List<Token> TokensToChoose);
    }
}
