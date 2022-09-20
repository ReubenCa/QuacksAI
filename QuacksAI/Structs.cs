using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuacksAI
{
    public readonly struct AIDynamicBrewingParameters
    {
        //AI Attempts to maximise VP*VPWeight + Money*MoneyWeight + Rubies*RubyWeight
        public readonly float VPWeight;
        public readonly float MoneyWeight;
        public readonly float RubyWeight;
        public readonly float DropletMoveWeight; //ignroe for now
        public readonly float DiceWeight;//ignored for now

        public AIDynamicBrewingParameters(float vPWeight, float moneyWeight, float rubyWeight, float dropletMoveWeight =0f, float diceWeight=0f)
        {
            VPWeight = vPWeight;
            MoneyWeight = moneyWeight;
            RubyWeight = rubyWeight;
            DropletMoveWeight = dropletMoveWeight;
            DiceWeight = diceWeight;
        }
    }
    public struct AIStaticBrewingParameters
    {

    }

    public struct PlayerGlobalGameData
    {
        public int Score;
        public int Rubies;
    }

    public readonly struct PlayerBrewData
    {
        public readonly List<Token> tokensinbag; //HAS TO BE A LIST TO ALLOW FOR DUPLICATES
        public readonly List<Token> PlacedTokens;
        ///// <summary>
        ///// The tile that would the AI would gain money from if it decided to not play anything in the round (because of droplet moves and rats tails)
        ///// </summary>
        //public int StartingTile; Current tile should just be set to this at the start
        public readonly int CurrentTile;
        
        public PlayerBrewData(List<Token> tokensinbag, List<Token> PlacedTokens, int CurrentTile)
        {
            this.tokensinbag = tokensinbag;
            this.PlacedTokens = PlacedTokens;
            this.CurrentTile = CurrentTile;
        }
    }

    public readonly struct Token : IEquatable<Token>, IComparable<Token>
    {
        public readonly TokenColor Color;
        public readonly int Value;
        public Token(TokenColor Color, int Value)
        {
            this.Color = Color;
            this.Value = Value;
        }
        public bool Equals(Token other)
        {
           return this.Color == other.Color && this.Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Token && Equals((Token)obj);
        }


        public static bool operator ==(Token left, Token right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Token left, Token right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return (Color, Value).GetHashCode();
        }
        public override string ToString()
        {
            return  Color.ToString() + " " + Value.ToString() ;
        }
        private static readonly int TokenColorCount = Enum.GetValues(typeof(TokenColor)).Length;

        public int CompareTo(Token other)
        {
            return (Color + TokenColorCount * Value) - (other.Color + TokenColorCount* other.Value);
        }
    }

    public enum TokenColor
    {
        white,
        orange,
        blue,
        red,
        yellow,
        black,
        green,
        purple
    }
}
