

namespace Tests
{
    [TestClass]
    public class BrewTests
    {


        
 

        [TestMethod]
        public void CanDecideToBrew()
        {
            (Token, int)[] startingtokens = new (Token, int)[] { (Tokens.White3, 1), (Tokens.White2, 2), (Tokens.White1, 4), (Tokens.Green1, 1), (Tokens.Orange1, 1) };
            TestUtilities.PlayTestRound(0, 1f, 4f, startingtokens, out _, true);
        }
        [TestMethod]
        public void AlwaysExplodesWhenVPAreWorthless()
        {
            for (int i = 0; i < 100; i++)
            {
                bool Exploded;
                (Token, int)[] startingtokens = new (Token, int)[] { (Tokens.White3, 1), (Tokens.White2, 2), (Tokens.White1, 4), (Tokens.Green1, 1), (Tokens.Orange1, 1) };
                TestUtilities.PlayTestRound(0, 1f, 0f, startingtokens, out Exploded, true);
                Assert.IsTrue(Exploded);
            }
        }

        [TestMethod]
        public void AlmostAlwaysExplodeswithlowVPWeight()
        {
            const int trials = 200;
            const float PassCriteria = 0.95f;
            int exploded = 0;
            for (int i = 0; i < trials; i++)
            {
                bool Exploded;
                (Token, int)[] startingtokens = new (Token, int)[] { (Tokens.White3, 1), (Tokens.White2, 2), (Tokens.White1, 4), (Tokens.Green1, 1), (Tokens.Orange1, 1) };
                TestUtilities.PlayTestRound(0, 100f, 10f, startingtokens, out Exploded, false);
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
            const float PassCriteria = 0.55f;
            const int Scenarios = 100;
            const int GamesPerScenario = 100;
            const int MaxTokenTypes = 4;
            const int MaxTokensofType = 5;

            int AIwins = 0;
            int Randomwins=0;
            int Draws = 0;
            int AIScenarioWins = 0;
            int RandomScenarioWins = 0;
            int DrawnScenarios = 0;
            for (int i = 0; i < Scenarios; i++)
            {
                (Token, int)[] StartingBag = new (Token, int)[Consts.r.Next(3, MaxTokenTypes)];
                StartingBag[0] = (Tokens.White3, 1);
                StartingBag[1] = (Tokens.White2, 2);
                StartingBag[2] = (Tokens.White1, 4);
                for(int j = 3; j<StartingBag.Length; j++)
                {
                    StartingBag[j] = (TestUtilities.RandomTokenGenerator(false), Consts.r.Next(1, MaxTokensofType+1));
                }
                float RubyWeight = (float)(Consts.r.NextDouble()*Consts.r.Next(0,20));
                float MoneyWeight = (float)(Consts.r.NextDouble() * Consts.r.Next(0, 4));
                float VPWeight = (float)(Consts.r.NextDouble() * Consts.r.Next(0, 8));
                AI ai = new AI(new AIDynamicBrewingParameters(VPWeight, MoneyWeight, RubyWeight, 0f, 0f), new AIStaticBrewingParameters());
                RandomAI random = new RandomAI(0.6 + 0.3*Math.Max(VPWeight,MoneyWeight)/(VPWeight+MoneyWeight));


                int AIWinsThisScenario = 0;
                int RandomWinsThisScenario = 0;
                for (int k = 0;k <GamesPerScenario;k++)
                {

                    //Console.WriteLine("--------\nAI:");
                    PlayerBrewData AIDat = TestUtilities.PlayTestRound(ai, StartingBag, out _, false);
                    float AIScore = AI.ScoreIfStopOrExplode(RubyWeight, MoneyWeight, VPWeight, AIDat, out bool _);
                    //Console.WriteLine("AI Score: " + AIScore);
                    //Console.WriteLine("--------\nRandom:");
                    PlayerBrewData RandomDat = TestUtilities.PlayTestRound(random,StartingBag, out _, false);
                   
                    
                    float randomScore = AI.ScoreIfStopOrExplode(RubyWeight, MoneyWeight, VPWeight, RandomDat, out bool _);
                    //Console.WriteLine("Random Score" + randomScore);
                    if (AIScore == randomScore)
                        Draws++;
                    else if (AIScore > randomScore)
                    {
                        AIwins++;
                        AIWinsThisScenario++;
                    }
                    else
                    {
                        Randomwins++;
                        RandomWinsThisScenario++;
                    }
                }
                if (AIWinsThisScenario == RandomWinsThisScenario)
                    DrawnScenarios++;
                else if (AIWinsThisScenario < RandomWinsThisScenario)
                    RandomScenarioWins++;
                else
                    AIScenarioWins++;
            }
            float OverallWinRAte = (float)(AIwins) / (float)(AIwins + Randomwins);
            float ScenarioWinRate = (float)AIScenarioWins / (float)(AIScenarioWins + RandomScenarioWins);
            Console.WriteLine("AIWins: {0} vs RandomWins: {1} (Draws: {2})\nWin Rate:\t{3}%\nScenarioWinRate:\t{4}%", AIwins, Randomwins, Draws, OverallWinRAte,ScenarioWinRate);
            Assert.IsTrue(OverallWinRAte > PassCriteria);
        }


        [TestMethod]
        public void CachingTest()
        {
            AI.ResetCacheStats();
            AI ai = new AI(new AIDynamicBrewingParameters(5, 2, 8), new AIStaticBrewingParameters());

            List<Token> Bag = new List<Token>();
            List<Token> BoardList = new List<Token>();
            BoardList.Add(new Token(TokenColor.white, 7));
            Bag.Add(new Token(TokenColor.white, 1));
            Bag.Add(new Token(TokenColor.orange, 1));
            Bag.Add(new Token(TokenColor.purple, 1));
            PlayerBrewData Data = new PlayerBrewData(Bag, BoardList, 1);
            ai.Brew(Data);
            Console.WriteLine(AI.CacheHits + ", " + AI.CacheAccesses);
            PlayerBrewData newData = Board.DrawChip(Data, new Token(TokenColor.orange, 1), out _, out _);
            ai.Brew(newData);
            Console.WriteLine(AI.CacheHits + ", " + AI.CacheAccesses);
            return;
        }
    }
}