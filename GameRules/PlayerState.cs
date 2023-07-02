using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameRules.Tokens;

namespace GameRules
{
    public struct PlayerState
    {
       public readonly ImmutableHashSet<Token> Bag;

        public readonly int Money;

        public readonly int VP;

        public readonly int Rubies;

        public readonly bool Flask;

       
        public PlayerState(ImmutableHashSet<Token> bag, int money, int vp, int rubies, bool flask)
        {
            Bag = bag;
            Money = money;
            VP = vp;
            Rubies = rubies;
            Flask = flask;
        }
    }
}
