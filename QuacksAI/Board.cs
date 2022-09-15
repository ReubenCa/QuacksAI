using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuacksAI
{
    /// <summary>
    /// Provides Utility functions for implementing rules of game
    /// </summary>
    public static class Board
    {
        /// <summary>
        /// Randomly Selects a token and returns the BrewData state after it is drawn
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="BlueDecisionNeeded"></param>
        /// <returns></returns>
        public static PlayerBrewData DrawChip(PlayerBrewData Data, out bool BlueDecisionNeeded, out bool Exploded, out Token TokenDrawn)
        {
            TokenDrawn = Data.tokensinbag.ElementAt(Consts.r.Next(Data.tokensinbag.Count));
            return DrawChip(Data, TokenDrawn, out BlueDecisionNeeded, out Exploded);
        }
        public static PlayerBrewData DrawChip(PlayerBrewData Data, Token t, out bool BlueDecisionNeeded, out bool Exploded)
        {
            Exploded = false;
            BlueDecisionNeeded = false;
            //PlayerBrewData PBD = new PlayerBrewData();

            List<Token> bag = new List<Token>(Data.tokensinbag);
            List<Token> tokens = new List<Token>(Data.PlacedTokens);

            //PBD.tokensinbag = bag;
            //PBD.PlacedTokens = tokens;

            bag.Remove(t);
            tokens.Add(t);
            int NewCurrentTile = Data.CurrentTile + t.Value;
            switch (t.Color)
            {
                case TokenColor.red:
                    int OrangeCount = tokens.Where(i => i.Color == TokenColor.orange).Count();
                    if (OrangeCount == 1 || OrangeCount == 2)
                        NewCurrentTile++;
                    else if (OrangeCount > 2)
                        NewCurrentTile += 2;
                    break;
                case TokenColor.yellow:
                    if (Data.PlacedTokens.Count > 0 && Data.PlacedTokens[tokens.Count - 1].Color == TokenColor.white)
                    {
                        bag.Add(Data.PlacedTokens[tokens.Count - 1]);
                        tokens.RemoveAt(tokens.Count - 1);
                    }
                    break;
                case TokenColor.blue:
                    BlueDecisionNeeded = true;
                    break;
                case TokenColor.white:
                    Exploded = tokens.Select<Token, int>(t => t.Color == TokenColor.white ? t.Value : 0).Sum() > 7;
                    break;
            }
            //PBD.CurrentTile = NewCurrentTile;
            return new PlayerBrewData(bag, tokens,NewCurrentTile);
        }



    }
}
