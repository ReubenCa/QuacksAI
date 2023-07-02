using GameRules;
using System.Collections.ObjectModel;
using GameRules.Tokens;
using System.Collections.Immutable;

public readonly struct Ruleset
{

	public readonly BoardLayout Layout;

	public readonly ReadOnlyDictionary<Colour, ITokenEffect> TokenSet;

	public readonly int MaxWhites;
	
	//Constructor
	public Ruleset(BoardLayout layout, ReadOnlyDictionary<Colour, ITokenEffect> tokenSet, int maxWhites)
	{
        Layout = layout;
        TokenSet = tokenSet;
        MaxWhites = maxWhites;
    }

}
