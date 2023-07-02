using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameRules.Tokens;

namespace GameRules
{
    internal interface IBrewer
    {
        public BrewDecision DecideDraw(BoardState state);

        public Token? DecideBlueOneEffect(List<Token> Drawn, BoardState state)
        {
            throw new Exception("This Player Does not support Blue 1 Effects");
        }


    }

    public enum BrewDecision
    {
        Draw,
        Stop,
        Flask
    }
}
