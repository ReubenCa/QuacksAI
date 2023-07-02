using System;
using System.Collections.Immutable;
using GameRules.Tokens;

namespace GameRules
{
    public readonly struct BoardState
    {
       public readonly ImmutableList<IndividualBoardState> OtherPlayerStates;
        public readonly IndividualBoardState PlayerState;
    
            public BoardState(ImmutableList<IndividualBoardState> otherStates, IndividualBoardState localState)
            {
                OtherPlayerStates = otherStates;
                PlayerState = localState;
            }


    }

    public readonly struct IndividualBoardState
    {
        public readonly ImmutableList<Token> Board;
        public readonly ImmutableHashSet<Token> Bag;
        public readonly int Rattails;

    }
}
