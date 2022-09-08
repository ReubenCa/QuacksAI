using System.Linq;

namespace QuacksAI
{
    public partial class AI : IBrew
        //Has an interace for brewing and buying to allow for easy UI that can also be made for players
    {
        private AIDynamicBrewingParameters DynamicBrewingParameters;
        private AIStaticBrewingParameters StaticBrewingParameters;

        public AI(AIDynamicBrewingParameters dynamicBrewingParameters, AIStaticBrewingParameters staticBrewingParameters)
        {
            this.DynamicBrewingParameters = dynamicBrewingParameters;
            this.StaticBrewingParameters = staticBrewingParameters;
        }



        /// <summary>
        /// Takes in Data and decides if the AI should draw from its bag or stop
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Brew(PlayerBrewData data)
        {
            

            List<Token> Bag = data.tokensinbag;
            List<Token> placed = data.PlacedTokens;
            if (Bag.Count == 0)
                return false;

            int CurrentTile = data.CurrentTile;
            if (CurrentTile >= Consts.Board.Length-1)
                return false;//Cant brew board is full


            int WhiteTotal = placed.Where(t => t.Color == TokenColor.white).Select<Token, int>(t => t.Value).Sum();

            //TODO: update this so it factors in the chance of getting rubies - low priority as it is mainly hypothetical of stopping here for rubies
            if (WhiteTotal + data.tokensinbag.Select<Token,int>(t => t.Color==TokenColor.white ? t.Value : 0).Max() <= 7)
                return true;//Always brew when chance of blowing up is zero

            GetExpectedScore(data, out bool Brew);
            return Brew;
        }
        //Get Expected Score Assuming you Brew
        private float GetExpectedScore( PlayerBrewData Data, out bool Brew)
        {
            List<Token> Bag = Data.tokensinbag;
            List<Token> TokensOnBoard = Data.PlacedTokens;
            int CurrentTile = Data.CurrentTile;


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
            if (TokensOnBoard.Where(t => t.Color == TokenColor.white).Select<Token, int>(t => t.Value).Sum() > 7)
            {
                Brew = false;
                return RubyUtil + Math.Max(MoneyUtil ,VPUtil);
            }

            //Now Average expected score of each possible draw and return Max(that, sum(Utils))
            float total = 0f;
            int count = Bag.Count;
            //Garuntees token of the same type will be next to each other
            Queue<Token> BagList = new Queue<Token>(Bag.OrderBy<Token, (int, int)>(t => ( (int)t.Color, t.Value)).ToList());
            while(BagList.Count>0)
            {
                Token t = BagList.Dequeue();
                float Count = 1f;//Takes advatnage of having multiple of the same token as the outcome will be the same for each of them
                //No need to calculate outcome for each individually
                while(BagList.Count>0 && BagList.Peek()==t)
                {
                    Count++;
                    BagList.Dequeue();
                }

                PlayerBrewData IfDrawn = Board.DrawChip(Data, t, out bool BlueDecision, out bool Exploded);
                total += GetExpectedScore(IfDrawn, out bool brew)*Count;
            }
            float ScoreIfBrew = total / (float)count;
            float ScoreIfNotBrew = RubyUtil + MoneyUtil+ VPUtil;
            Brew = ScoreIfBrew > ScoreIfNotBrew;
            return Math.Max(ScoreIfNotBrew, ScoreIfBrew);
        }

        private Token BlueDrawn()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decides if the AI should Flask
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Flask(PlayerBrewData data)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Decides which if any token should be played after a blue
        /// </summary>
        /// <param name="data"></param>
        /// <param name="TokensToChoose"></param>
        /// <returns></returns>
        public Token DecideOnBlue(PlayerBrewData data, List<Token> TokensToChoose)
        {
            throw new NotImplementedException();
        }

    }
}