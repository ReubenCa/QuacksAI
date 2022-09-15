using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Tests
{
    internal class RandomAI : IBrew
    {
        double p;
        public RandomAI(double BrewProb)
        {
            p = BrewProb;
        }

        public bool Brew(PlayerBrewData data)
        {
            int CurrentTile = data.CurrentTile;
            if (CurrentTile >= Consts.Board.Length - 1)
                return false;//Cant brew board is full
            List<Token> Bag = data.tokensinbag;
            List<Token> placed = data.PlacedTokens;
            int WhiteTotal = placed.Where(t => t.Color == TokenColor.white).Select<Token, int>(t => t.Value).Sum();
            if (WhiteTotal + data.tokensinbag.Select<Token, int>(t => t.Color == TokenColor.white ? t.Value : 0).Max() <= 7)
                return true;//Always brew when chance of blowing up is zero

            return Consts.r.NextDouble() < p;
        }

        public Token DecideOnBlue(PlayerBrewData data, List<Token> TokensToChoose)
        {
            throw new NotImplementedException();
        }

        public bool Flask(PlayerBrewData data)
        {
            throw new NotImplementedException();
        }
    }
}
