using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal static class TestUtilities
    {
        public static Token RandomTokenGenerator(bool AllowWhites)
        {
            int[] vals = new int[] { 1, 2, 4 };
            Type type = typeof(TokenColor);
            Array values = type.GetEnumValues();
            
            TokenColor color;
            do
            {
                int index = Consts.r.Next(values.Length);
                color = (TokenColor)values.GetValue(index);
            } while (!AllowWhites && color == TokenColor.white);
            if (color == TokenColor.white)
                return new Token(color, Consts.r.Next(0, 4));
            return new Token(color, vals[Consts.r.Next(0, vals.Length)]);
        }

        public static List<Token> CreateRandomStaringBag(int NonWhiteTokenTypes, int NonWhiteTokens)
        {
            List<Token> bag = new List<Token>();
            Token[] types = new Token[NonWhiteTokenTypes];
            for (int i = 0; i < types.Length; i++)
                types[i] = RandomTokenGenerator(false);
            for (int i = 0; i < NonWhiteTokenTypes; i++)
                bag.Add(types[i]);
            for(int i = bag.Count; i < NonWhiteTokens; i++)
            {
                bag.Add(types[Consts.r.Next(types.Length)]);
            }
            bag.Add(new Token(TokenColor.white, 3));
            bag.Add(new Token(TokenColor.white, 2));
            bag.Add(new Token(TokenColor.white, 2));
            bag.Add(new Token(TokenColor.white, 1));
            bag.Add(new Token(TokenColor.white, 1));
            bag.Add(new Token(TokenColor.white, 1));
            bag.Add(new Token(TokenColor.white, 1));
            return bag;

        }

        public static PlayerBrewData PlayTestRound(float RubyWeight, float MoneyWeight, float VPWeight, (Token, int)[] TokensInBag, out bool Exploded, bool loggame)
        {
            AI ai = new AI(new AIDynamicBrewingParameters(VPWeight, MoneyWeight, RubyWeight), new AIStaticBrewingParameters());
            return PlayTestRound(ai, TokensInBag, out Exploded, loggame);
        }

        public static PlayerBrewData PlayTestRound(IBrew ai, (Token, int)[] TokensInBag, out bool Exploded, bool loggame)
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
            PlayerBrewData Data = new PlayerBrewData(tokens, tokensonboard, 1, 0);
            return PlayTestRound(ai, Data, out Exploded, loggame);
        }
        public static PlayerBrewData PlayTestRound(IBrew ai,PlayerBrewData Data, out bool Exploded, bool loggame)
        {
            long CurrentCacheHits = 0;
            long CurrentCacheAccesses = 0;
            int Round = 0;
            if (loggame && AI.Cache_Stats)
            {
                CurrentCacheAccesses = AI.CacheAccesses;
                CurrentCacheHits = AI.CacheHits;
            }
           

            Exploded = false;

            while (!Exploded && (Data.BlueLast > 0 || ai.Brew(Data)))
            {
                Round++;
                if( Data.BlueLast <= 0)
                    Data = Board.DrawToken(Data,  out Exploded, out _);
                else
                {
                    //throw new NotImplementedException();
                    //Pick n random tokens and then Board.DrawChip the AIs decision out of those tokens.
                    List<Token> Choices = Data.tokensinbag.OrderBy(x => Consts.r.Next()).Take(Data.BlueLast).ToList();
                    Token? Choice = ai.DecideOnBlue(Data, Choices);
                    if(Choice!= null)
                    {
                        Data = Board.DrawToken(Data, (Token)Choice, out Exploded);
                    }
                }
                if (loggame)
                {
                    Console.WriteLine("Drawn");
                }
                if(loggame && AI.Cache_Stats)
                {
                    Console.WriteLine("Round {0}:\nCache Accesses: {1}\tCache Hits: {2}", Round, AI.CacheAccesses - CurrentCacheAccesses, AI.CacheHits - CurrentCacheHits);
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
    }
}
