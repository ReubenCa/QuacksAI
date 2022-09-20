


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
            const int MaxTokenTypes = 6;
            const int TrialsPerType = 100;
            const int Tokens = 10;
            StringBuilder sb = new StringBuilder(4000);
            sb.AppendLine("Caching: " + Parameters.Caching.ToString() + "\nShared Caching: " + Parameters.SharedCaching);
         
            
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
                sb.AppendLine("With " + TokenTypes + " Token Types (not including whites)");
                sb.AppendLine("Average Time Taken To play an entire round over " + TrialsPerType + " Trials is " + (timer.ElapsedMilliseconds / (float)TrialsPerType).ToString() + "ms");
                if(AI.Cache_Stats)
                    sb.AppendLine("Cache Accesses: " + AI.CacheAccesses + "\tCache Hits: " + AI.CacheHits + "\tCache Misses: " + AI.CacheMisses + "Hit Rate: " + ((100f)*(float)(AI.CacheHits)/(float)(AI.CacheAccesses)).ToString() + "%");
            }
           
           sb.AppendLine("\n--------------\n");
            const string logpath = "PreviousTimeTestOutput.txt";

            StreamReader sr = new StreamReader(logpath);
            for(int i = 0; i <1000; i++)
            {
               sb.AppendLine(sr.ReadLine());
                if (sr.EndOfStream)
                    break;
            }
            sr.Close();

            Console.WriteLine(sb.ToString());
            StreamWriter sw = new StreamWriter(logpath);
            sw.Write(sb.ToString());
            sw.Close();

        }


    }
}
