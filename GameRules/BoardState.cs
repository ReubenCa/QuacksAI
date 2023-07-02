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

        public BoardState UpdatePlayerState(IndividualBoardState state)
        {
            return new BoardState(OtherPlayerStates, state);
        }

    }

    public readonly struct IndividualBoardState
    {
        public readonly ImmutableList<Token> Board;
        public readonly ImmutableHashSet<Token> Bag;
        public readonly int Rattails;
        public readonly ImmutableList<Decision> DecisionsNeeded;

        public IndividualBoardState(ImmutableList<Token> board, ImmutableHashSet<Token> bag, int rattails, ImmutableList<Decision> decisionsNeeded)
        {
            Board = board;
            Bag = bag;
            Rattails = rattails;
            DecisionsNeeded = decisionsNeeded;
        }

        public IndividualBoardState AddDecision(Decision decision)
        { 
            return new IndividualBoardState(Board, Bag, Rattails, DecisionsNeeded.Add(decision));
        }
        public IndividualBoardState RemoveDecision(Decision decision)
        {
            return new IndividualBoardState(Board, Bag, Rattails, DecisionsNeeded.Remove(decision));
        }
    }


}

