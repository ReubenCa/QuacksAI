﻿using System;
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
            int index = Consts.r.Next(values.Length);
            TokenColor color;
            do
            {
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
            for(int i = 0; i < NonWhiteTokens; i++)
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
            PlayerBrewData Data = new PlayerBrewData(tokens, tokensonboard, 1);
            return PlayTestRound(ai, Data, out Exploded, loggame);
        }
        public static PlayerBrewData PlayTestRound(IBrew ai,PlayerBrewData Data, out bool Exploded, bool loggame)
        {




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
    }
}