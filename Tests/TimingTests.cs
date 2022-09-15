using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Tests
{
    [TestClass]
    public class TimingTests
    {
        [TestMethod]
        public void TimeGames()
        {
            const int MaxTokenTypes = 1;
            const int TrialsPerType = 20;
            const int Tokens = 3;
            StringBuilder sb = new StringBuilder(400);
            
         
            StreamWriter sw = new StreamWriter("PreviousTimeTestOutput.txt");
            Stopwatch timer = new Stopwatch();
            for(int TokenTypes = 1; TokenTypes<= MaxTokenTypes; TokenTypes++ )
            {
                for (int i = 0; i < TrialsPerType; i++)
                {
                    List<Token> startbag = TestUtilities.CreateRandomStaringBag(TokenTypes, Tokens);
                    AIDynamicBrewingParameters DBP = new AIDynamicBrewingParameters((float)Consts.r.NextDouble()*5, (float)Consts.r.NextDouble(),(float)Consts.r.NextDouble()*3);
                    AI ai = new AI(DBP, new AIStaticBrewingParameters());
                    PlayerBrewData PBD = new PlayerBrewData(startbag, new List<Token>(), 1);
                    timer.Start();
                    TestUtilities.PlayTestRound(ai, PBD, out _,false);
                    timer.Stop();
                }
                sb.AppendLine("With " + MaxTokenTypes + " Token Types (not including whites)");
                sb.AppendLine("Average Time Taken To play an entire round over " + TrialsPerType + " Trials is " + (timer.ElapsedMilliseconds / (float)TrialsPerType).ToString() + "ms");
            }
            Console.WriteLine(sb.ToString());
        }


    }
}
