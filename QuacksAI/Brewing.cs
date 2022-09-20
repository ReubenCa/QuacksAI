#define CACHE_STATS

using System.Linq;

namespace QuacksAI
{
    public partial class AI : IBrew
        //Has an interace for brewing and buying to allow for easy UI that can also be made for players
    {
#if CACHE_STATS
        public const bool Cache_Stats = Parameters.Caching;
        public static long CacheAccesses = 0;
        public static long CacheMisses = 0;
        public static long CacheHits = 0;
        public static void ResetCacheStats()
        { CacheAccesses = 0; CacheMisses = 0; CacheAccesses = 0; }
#else
        public const bool Cache_Stats = false;
#endif


        //Static so AIs all share a Cache
        private readonly static Dictionary<(List<Token>, int, int, int, int, int, int), (float, bool)> 
            _sharedcache = new Dictionary<(List<Token>, int, int, int, int, int, int), (float, bool)>
            (new CacheKeyCompararer()) ;

        private readonly Dictionary<(List<Token>, int, int, int, int, int, int), (float, bool)> 
            _privatecache = new Dictionary<(List<Token>, int, int, int, int, int, int),
                (float, bool)>(new CacheKeyCompararer());
        
        private Dictionary<(List<Token>, int, int, int, int, int, int), (float, bool)> Cache
        {
            get
            {
                if (Parameters.SharedCaching)
                    return _sharedcache;
                return _privatecache;
            }
        }
        const bool Caching = Parameters.Caching;


        private AIDynamicBrewingParameters DynamicBrewingParameters;
        private AIStaticBrewingParameters StaticBrewingParameters;

        public AI(AIDynamicBrewingParameters dynamicBrewingParameters, AIStaticBrewingParameters staticBrewingParameters)
        {
            this.DynamicBrewingParameters = dynamicBrewingParameters;
            this.StaticBrewingParameters = staticBrewingParameters;
        }

        public bool Brew(PlayerBrewData data)
        {
            return Brew(data, out float _);
        }

        /// <summary>
        /// Takes in Data and decides if the AI should draw from its bag or stop
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        public bool Brew(PlayerBrewData data,  out float Score)
        {
            
            Score = 0f;
            List<Token> Bag = data.tokensinbag;
            List<Token> placed = data.PlacedTokens;
            if (Bag.Count == 0)
                return false;

            int CurrentTile = data.CurrentTile;
            if (CurrentTile >= Consts.Board.Length-1)
                return false;//Cant brew board is full


            int WhiteTotal = placed.Where(t => t.Color == TokenColor.white).Select<Token, int>(t => t.Value).Sum();

           
            if (Parameters.AutoBrewWhenCantDie &&  WhiteTotal + data.tokensinbag.Select<Token,int>(t => t.Color==TokenColor.white ? t.Value : 0).Max() <= 7)
                return true;//Always brew when chance of blowing up is zero

            Score = GetExpectedScore(data, out bool Brew);
            return Brew;
        }
        //Get Expected Score Assuming you Brew
     
        private float GetExpectedScore( PlayerBrewData Data, out bool Brew)
        {



            (List<Token>, int, int, int, int, int, int) Key =(null, 0,0,0,0,0,0);
            List<Token> Bag = Data.tokensinbag;
            List<Token> TokensOnBoard = Data.PlacedTokens;
            int CurrentTile = Data.CurrentTile;
            float ScoreIfNotBrew = ScoreIfStopOrExplode(DynamicBrewingParameters, Data, out bool explode);
            if (explode)
            {
                Brew = false; 
                return ScoreIfNotBrew;
            }

            if (Caching)
            {
#if CACHE_STATS
                CacheAccesses++;
#endif
                Key = GetKey(Data);
                if (Cache.TryGetValue(Key, out (float, bool) Result))
                {
#if CACHE_STATS
                    CacheHits++;
#endif
                    Brew = Result.Item2;
                    return Result.Item1;
                }
#if CACHE_STATS
                else
                {
                    CacheMisses++;
                }
#endif
            }




            //Now Average expected score of each possible draw and return Max(that, sum(Utils))
            float total = 0f;
            int count = Bag.Count;
            //Garuntees token of the same type will be next to each other
            Bag.Sort();
            Queue<Token> BagList = new Queue<Token>(Bag);
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
            Brew = ScoreIfBrew > ScoreIfNotBrew;
            float value = Math.Max(ScoreIfNotBrew, ScoreIfBrew);
            if (Caching)
            {
                Cache.Add(Key!, (value, Brew));
            }
            return value;
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

        public static float ScoreIfStopOrExplode(AIDynamicBrewingParameters AIP, PlayerBrewData Data, out bool Exploded)
        {
            return ScoreIfStopOrExplode(AIP.RubyWeight, AIP.MoneyWeight, AIP.VPWeight, Data, out Exploded);
        }

        public static float ScoreIfStopOrExplode(float RubyWeight, float MoneyWeight, float VPWeight, PlayerBrewData Data, out bool Exploded)
        {
            List<Token> Bag = Data.tokensinbag;
            List<Token> TokensOnBoard = Data.PlacedTokens;
            int CurrentTile = Data.CurrentTile;

            //If PAst the max Assume you just get the last tile
            (int Money, int VP, bool Ruby) tile = Consts.Board[Math.Min(CurrentTile, Consts.Board.Length-1)];


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
                Exploded = true;
                return RubyUtil + Math.Max(MoneyUtil, VPUtil);
            }
            if (Bag.Count == 0)
                throw new Exception("Bag Empty, likely caused by brewing with less than seven whites in the bag");
            Exploded = false;
            return RubyUtil + MoneyUtil + VPUtil;
        }
    
    
        private (List<Token>, int ,int ,int, int, int, int) GetKey(PlayerBrewData PBD)
        {
            //Ensures that any strategically identical situations will have the same key
            //This Extra data is used instead of the list of tokens on the board as it allows for more matches
            //and still should always result in the right decision
            List<Token> Bag = PBD.tokensinbag;
            Bag.Sort();
            List<Token> Board = PBD.PlacedTokens;
            int Tile = PBD.CurrentTile;
            int RubyWeight = (int)(DynamicBrewingParameters.RubyWeight * 1024f);
            int MoneyWeight = (int)(DynamicBrewingParameters.MoneyWeight * 1024f);
            int VPWeight = (int)(DynamicBrewingParameters.VPWeight * 1024f);
            int RedInBagCount = Bag.Where(t => t.Color == TokenColor.red).Count();
            int OrangesInBagCount = Bag.Where(t => t.Color == TokenColor.orange).Count();
            int OrangesOnBoard = Board.Where(t => t.Color == TokenColor.orange).Count();

            //ORANGES
            int AdjustedOrangesOnBoard;
            if (RedInBagCount == 0 || (OrangesInBagCount + OrangesOnBoard) == 0)
                AdjustedOrangesOnBoard = -1;
            else if (OrangesOnBoard + OrangesInBagCount < 3)
                AdjustedOrangesOnBoard = Math.Min(OrangesOnBoard, 1);//If you cant get the third orange makes no difference if you have one or two
            else
                AdjustedOrangesOnBoard = Math.Min(OrangesOnBoard, 3);

            bool YellowInBag = Bag.Where(t => t.Color == TokenColor.yellow).Count() > 0;
            int WhiteLast = YellowInBag ? Bag[Bag.Count - 1].Color == TokenColor.white ? Bag[Bag.Count - 1].Value : 0 : 0;
            int GreensLastTwo = 0;
            if(Bag.Count > 0)
            {
                GreensLastTwo += Bag[Bag.Count - 1].Color == TokenColor.green ? 1 : 0;
                if(Bag.Count > 1)
                    GreensLastTwo += Bag[Bag.Count - 2].Color == TokenColor.green ? 1 : 0;
            }

            return (Bag, RubyWeight, MoneyWeight, VPWeight, AdjustedOrangesOnBoard, WhiteLast, GreensLastTwo);
        }
    
    }

    public class CacheKeyCompararer : IEqualityComparer<(List<Token>, int, int, int, int, int, int)>
    {
        //bool IEqualityComparer<(List<Token>, int, int, int, int, int, int)>.Equals((List<Token>, int, int, int, int, int, int) x, (List<Token>, int, int, int, int, int, int) y)
        //{
        //    if (!x.Item1.SequenceEqual(y.Item1))
        //        return false;
        //    return x.Item1 == y.Item1 && x.Item2 == y.Item2 && x.Item3 == y.Item3 && x.Item4 == y.Item4 && x.Item5 == y.Item5 && x.Item6 == y.Item6 && x.Item7 == y.Item7;
        //}

        public bool Equals((List<Token>, int, int, int, int, int, int) x, (List<Token>, int, int, int, int, int, int) y)
        {
            if (!x.Item1.SequenceEqual(y.Item1))
                return false;
            return x.Item2 == y.Item2 && x.Item3 == y.Item3 && x.Item4 == y.Item4 && x.Item5 == y.Item5 && x.Item6 == y.Item6 && x.Item7 == y.Item7;
        }
        public int GetHashCode((List<Token>, int, int, int, int, int, int) x)
        {
            (int, int, int, int, int, int) y = (x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7);
            int val = 0;
            x.Item1.ForEach(t => val ^= t.GetHashCode());
            return y.GetHashCode() ^ val;
                
        }
    
    }
}