using System;	

namespace GameRules.Tokens
{ 
public readonly struct Token
{
	
		//Have a private variable for the colour of the token
		//Also have an int value for token and a constructor to set both

		public readonly int Value;
		public readonly Colour Colour;
		public Token(Colour colour, int value)
		{
            Colour = colour;
            Value = value;
		
        }

		

		
	}

	public enum Colour
	{
		White,
		Orange,
		Blue,
		Red,
		Yellow,
		Purple,
		Black
	}
}
