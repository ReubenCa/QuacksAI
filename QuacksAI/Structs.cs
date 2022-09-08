using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuacksAI
{
    public struct AIDynamicBrewingParameters
    {
        //AI Attempts to maximise VP*VPWeight + Money*MoneyWeight + Rubies*RubyWeight
        public float VPWeight;
        public float MoneyWeight;
        public float RubyWeight;
        public float DropletMoveWeight; //ignroe for now
        public float DiceWeight;//ignored for now

        
    }
    public struct AIStaticBrewingParameters
    {

    }

    public struct PlayerGlobalGameData
    {
        public int Score;
        public int Rubies;
    }

    public struct PlayerBrewData
    {
        public HashSet<Token> tokensinbag;
        public List<Token> PlacedTokens;
        /// <summary>
        /// The tile that would the AI would gain money from if it decided to not play anything in the round (because of droplet moves and rats tails)
        /// </summary>
        //public int StartingTile; Current tile should just be set to this at the start
        public int CurrentTile;
    }

    public struct Token
    {
        public TokenColor Color;
        public int TokenValue;
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
