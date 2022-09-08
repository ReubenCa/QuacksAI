using System.Linq;

namespace QuacksAI
{
    public partial class AI//Has an interace for brewing and buying to allow for easy UI that can also be made for players
    {
        private AIDynamicBrewingParameters DynamicBrewingParameters;
        private AIStaticBrewingParameters StaticBrewingParameters;

        /// <summary>
        /// Takes in Data and decides if the AI should draw from its bag or stop
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Brew(PlayerBrewData data)
        {
            

            HashSet<Token> Bag = data.tokensinbag;
            List<Token> placed = data.PlacedTokens;
            if (Bag.Count == 0)
                return false;

            int CurrentTile = data.CurrentTile;
            if (CurrentTile > 34)
                return false;//Cant brew board is full


            int WhiteTotal = placed.Where(t => t.Color == TokenColor.white).Select<Token, int>(t => t.TokenValue).Sum();

            //TODO: update this so it factors in the chance of getting rubies - low priority as it is mainly hypothetical of stopping here for rubies
            if (WhiteTotal <= 4)
                return true;//Always brew when chance of blowing up is zero
            

        }
        //Get Expected Score Assuming you Brew
        private float GetExpectedScore(List<Token> Bag, List<Token> TokensOnBoard, int CurrentTile)
        {
            (int Money, int VP, bool Ruby) tile = Consts.Board[CurrentTile];


            //if blown up just return the current score
            
                float VPUtil = DynamicBrewingParameters.VPWeight * tile.VP;
                float MoneyUtil = DynamicBrewingParameters.MoneyWeight * tile.Money;
                float Rubies = 0f;
                if (TokensOnBoard.Count == 0)
                    Rubies = 0f;
                else if (TokensOnBoard.Count == 1)
                    Rubies = TokensOnBoard[0].Color == TokenColor.green ? 1f : 0f;
                else
                {
                    Rubies += TokensOnBoard[TokensOnBoard.Count-1].Color == TokenColor.green ? 1f : 0f;
                    Rubies += TokensOnBoard[TokensOnBoard.Count - 2].Color == TokenColor.green ? 1f : 0f;
                }
                Rubies += tile.Ruby ? 1f : 0f;
                float RubyUtil = Rubies * DynamicBrewingParameters.RubyWeight;
            if (TokensOnBoard.Where(t => t.Color == TokenColor.white).Select<Token, int>(t => t.TokenValue).Sum() > 7)
            {
                return RubyUtil + Math.Max(MoneyUtil ,VPUtil);
            }

            //Now Average expected score of each possible draw and return Max(that, sum(Utils))


        }


        /// <summary>
        /// Decides if the AI should Flask
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Flask(PlayerBrewData data)
        {

        }
        /// <summary>
        /// Decides which if any token should be played after a blue
        /// </summary>
        /// <param name="data"></param>
        /// <param name="TokensToChoose"></param>
        /// <returns></returns>
        public Token BlueDrawn(PlayerBrewData data, List<Token> TokensToChoose)
        {

        }

    }
}