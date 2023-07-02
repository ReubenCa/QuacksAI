using GameRules;
using System;

namespace GameRules.Tokens
{
	public interface ITokenEffect
	{
		public BoardState afterPlacement(BoardState state);

		public PlayerState afterRound(PlayerState oldplayerstate, BoardState finalboardstate);

	}

}