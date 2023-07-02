using System;	


public class Token
{
	public Token()
	{
		//Have a private variable for the colour of the token
		//Also have an int value for token and a constructor to set both

		private int Value;
		public Token Token(Colours colour, int value)
		{
            Colour = colour;
            Value = value;
		
        }

		

		
	}

	public enum Colours
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
