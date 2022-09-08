using QuacksAI;

namespace Tests
{
    [TestClass]
    public class BrewTests
    {
        private Token RandomTokenGenerator(bool AllowWhites)
        {
            int[] vals = new int[] { 1, 2, 4 };
            Type type = typeof(TokenColor);
            Array values = type.GetEnumValues();
            int index = Consts.r.Next(values.Length);
            TokenColor color = TokenColor.orange;
            while (AllowWhites || color != TokenColor.white)
            {
                color = (TokenColor)values.GetValue(index);
            }
            if (color == TokenColor.white)
                return new Token(color, Consts.r.Next(0, 4));
            return new Token(color, vals[Consts.r.Next(0, vals.Length)]);
        }
        
        private float CalculateScore(float RubyWeight, float MoneyWeight, float VPWeight, PlayerBrewData Data)
        {
            List<Token> Bag = Data.tokensinbag;
            List<Token> TokensOnBoard = Data.PlacedTokens;
            int CurrentTile = Data.CurrentTile;


            (int Money, int VP, bool Ruby) tile = Consts.Board[CurrentTile];


            //if blown up just return the current score

            float VPUtil = VPWeight * tile.VP;
            float MoneyUtil = MoneyWeight * tile.Money;
            float Rubies = 0f;

            if (TokensOnBoard.Count == 0)
                Rubies = 0f;
            else if (TokensOnBoard.Count == 1)
                Rubies = TokensOnBoard[0].Color == TokenColor.green ? 1f : 0f;
            else
            {
                Rubies += TokensOnBoard[TokensOnBoard.Count - 1].Color == TokenColor.green ? 1f : 0f;
                Rubies += TokensOnBoard[TokensOnBoard.Count - 2].Color == TokenColor.green ? 1f : 0f;
            }
            Rubies += tile.Ruby ? 1f : 0f;
            float RubyUtil = Rubies * RubyWeight;
            if (TokensOnBoard.Where(t => t.Color == TokenColor.white).Select<Token, int>(t => t.Value).Sum() > 7)
            {
                return RubyUtil + Math.Max(MoneyUtil, VPUtil);
            }
            return RubyUtil + MoneyUtil + VPUtil;
        }
        private PlayerBrewData PlayTestRound(IBrew ai, (Token, int)[] TokensInBag, out bool Exploded, bool loggame)
        {
            List<Token> tokens = new List<Token>();
            foreach ((Token, int) token in TokensInBag)
            {
                for (int i = 0; i < token.Item2; i++)
                {
                    tokens.Add(token.Item1);
                }
            }
            TokensInBag = null;
            List<Token> tokensonboard = new List<Token>(35);
            PlayerBrewData Data = new PlayerBrewData(tokens, tokensonboard, 1);

            

            Exploded = false;

            while (!Exploded && ai.Brew(Data))
            {
                Data = Board.DrawChip(Data, out _, out Exploded, out Token Drawn);
                if (loggame)
                {
                    Console.WriteLine(Drawn);
                }
            }
            if (loggame)
            {
                if (Exploded)
                {
                    Console.WriteLine("Exploded :(");
                }
                else
                {
                    Console.WriteLine("Stopped");
                }
                Console.WriteLine(Consts.Board[Data.CurrentTile]);
            }
            return Data;
        }
        
        private PlayerBrewData PlayTestRound(float RubyWeight, float MoneyWeight, float VPWeight, (Token,int)[] TokensInBag, out bool Exploded, bool loggame)
        {
            AI ai = new AI(new AIDynamicBrewingParameters(VPWeight, MoneyWeight,RubyWeight), new AIStaticBrewingParameters());
            return PlayTestRound(ai, TokensInBag, out Exploded, loggame);
        }

        [TestMethod]
        public void CanDecideToBrew()
        {
            (Token, int)[] startingtokens = new (Token, int)[] { (Tokens.White3, 1), (Tokens.White2, 2), (Tokens.White1, 4), (Tokens.Green1, 1), (Tokens.Orange1, 1) };
            PlayTestRound(0, 1f, 4f, startingtokens, out _, true);
        }
        [TestMethod]
        public void AlwaysExplodesWhenVPAreWorthless()
        {
            for (int i = 0; i < 100; i++)
            {
                bool Exploded;
                (Token, int)[] startingtokens = new (Token, int)[] { (Tokens.White3, 1), (Tokens.White2, 2), (Tokens.White1, 4), (Tokens.Green1, 1), (Tokens.Orange1, 1) };
                PlayTestRound(0, 1f, 0f, startingtokens, out Exploded, true);
                Assert.IsTrue(Exploded);
            }
        }

        [TestMethod]
        public void AlmostAlwaysExplodeswithlowVPWeight()
        {
            const int trials = 500;
            const float PassCriteria = 0.95f;
            int exploded = 0;
            for (int i = 0; i < trials; i++)
            {
                bool Exploded;
                (Token, int)[] startingtokens = new (Token, int)[] { (Tokens.White3, 1), (Tokens.White2, 2), (Tokens.White1, 4), (Tokens.Green1, 1), (Tokens.Orange1, 1) };
                PlayTestRound(0, 100f, 10f, startingtokens, out Exploded, false);
               if(Exploded)
                {
                    exploded++;
                }
            }
            Console.WriteLine("Exploded {0}/{1} times", exploded, trials);
            Assert.IsTrue(exploded > trials*PassCriteria);
        }

        [TestMethod]
        public void OutPerformsRandom()
        {
            const float PassCriteria = 0.98f;
            const int Scenarios = 200;
            const int GamesPerScenario = 5;
            const int MaxTokenTypes = 3;
            const int MaxTokensofType = 5;

            int AIwins = 0;
            int Randomwins=0;

            for (int i = 0; i < Scenarios; i++)
            {
                (Token, int)[] StartingBag = new (Token, int)[Consts.r.Next(3, MaxTokenTypes)];
                StartingBag[0] = (Tokens.White3, 1);
                StartingBag[1] = (Tokens.White2, 2);
                StartingBag[2] = (Tokens.White1, 4);
                for(int j = 3; j<StartingBag.Length; j++)
                {
                    StartingBag[j] = (RandomTokenGenerator(false), Consts.r.Next(1, MaxTokensofType+1));
                }
                float RubyWeight = (float)(Consts.r.NextDouble()*Consts.r.Next(0,20));
                float MoneyWeight = (float)(Consts.r.NextDouble() * Consts.r.Next(0, 4));
                float VPWeight = (float)(Consts.r.NextDouble() * Consts.r.Next(0, 8));
                AI ai = new AI(new AIDynamicBrewingParameters(VPWeight, MoneyWeight, RubyWeight, 0f, 0f), new AIStaticBrewingParameters());
                RandomAI random = new RandomAI(0.6 + 0.3*Math.Max(VPWeight,MoneyWeight)/(VPWeight+MoneyWeight));

                for (int k = 0;k <GamesPerScenario;k++)
                {
                    PlayerBrewData AIDat = PlayTestRound(ai, StartingBag, out _, false);
                    PlayerBrewData RandomDat = PlayTestRound(random,StartingBag, out _, false);
                    (int Money, int VP, bool Ruby) AITile = Consts.Board[AIDat.CurrentTile];
                    float AIScore = CalculateScore(RubyWeight, MoneyWeight, VPWeight, AIDat);
                    float randomScore = CalculateScore(RubyWeight, MoneyWeight, VPWeight, RandomDat);
                    if (AIScore > randomScore)
                        AIwins++;
                    else
                        Randomwins++;
                }
            }
            Console.WriteLine("AIWins: {0} vs RandomWins: {1}", AIwins, Randomwins);
            Assert.IsTrue((float)(AIwins) / (float)(AIwins + Randomwins) > PassCriteria);
        }
    }
}