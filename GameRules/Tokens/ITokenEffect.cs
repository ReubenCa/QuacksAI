using GameRules;
using System;
using System.Collections.Immutable;

namespace GameRules.Tokens
{
	public abstract class ITokenEffect
	{
		public abstract BoardState ImmediateAfterPlacementEffect(BoardState state);

		public abstract PlayerState EndOfRoundeffect(PlayerState oldplayerstate, BoardState finalboardstate);


	}

	public abstract class DecisionRequiredEffect : ITokenEffect
	{
		protected abstract Decision Decision { get; }
		public override BoardState ImmediateAfterPlacementEffect(BoardState state)
		{
			return state.UpdatePlayerState(state.PlayerState.AddDecision(Decision));
			
		}

		public override PlayerState EndOfRoundeffect(PlayerState oldplayerstate, BoardState finalboardstate)
		{
			return oldplayerstate;
		}



	}

	public abstract class Decision
	{
		public abstract ImmutableList<IChoice> Choices { get; }
		public BoardState MakeDecision(BoardState previousState, IChoice choice)
		{
			BoardState statewithoutdecision = previousState.UpdatePlayerState(
				previousState.PlayerState.RemoveDecision(this));
				
			return choice.ApplyChoice(statewithoutdecision);
		}
	
	}

	public interface  IChoice
	{
		public BoardState ApplyChoice(BoardState state);
	}

	//Decision system allows decision to be defined in terms of the choices that can be made and how each of these choices 
	//effects the board state
	//This allows the AI to be able to make choices by default through just brute forcing each choice

	//There are cases where choices when generated select random parameters when they are generated
	//An example Being a BlueOne-4 token. 
	//Here four tokens are drawn and the AI can play one.
	//There is a large amount of possible sets of draws so the AI needs to do its own optimisations when calcuting the value 
	//of this choice.

	//This Project however has the aim of just applying utitlity methods that can be used both by the AI for making decisions
	//and the program for running the game. 
	//It should not save any data itself.

}