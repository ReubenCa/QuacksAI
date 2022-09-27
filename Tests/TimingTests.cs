


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
            const int MaxTokenTypes = 5;
            const int TrialsPerType = 10;
            const int Tokens = 15;
            const bool loggames = false;
            const bool AllowBlue = true;
            StringBuilder sb = new StringBuilder(4000);
            sb.AppendLine("Caching: " + Parameters.Caching.ToString() + "\nShared Caching: " + Parameters.SharedCaching);
         
            
            Stopwatch timer = new Stopwatch();
            for(int TokenTypes = 1; TokenTypes<= MaxTokenTypes; TokenTypes++ )
            {
                for (int i = 0; i < TrialsPerType; i++)
                {
                    List<Token> startbag = TestUtilities.CreateRandomStaringBag(TokenTypes, Tokens, AllowBlue);
                    AIDynamicBrewingParameters DBP = new AIDynamicBrewingParameters((float)Consts.r.NextDouble()*5, (float)Consts.r.NextDouble(),(float)Consts.r.NextDouble()*3);
                    AI ai = new AI(DBP, new AIStaticBrewingParameters());
                    PlayerBrewData PBD = new PlayerBrewData(startbag, new List<Token>(), 1, 0);
                    timer.Start();
                    TestUtilities.PlayTestRound(ai, PBD, out _, loggames);
                    timer.Stop();
                }
                sb.AppendLine("\nWith " + TokenTypes + " Token Types (not including whites)");
                sb.AppendLine("Average Time Taken To play an entire round over " + TrialsPerType + " Trials is " + (timer.ElapsedMilliseconds / (float)TrialsPerType).ToString() + "ms");
                if (AI.Cache_Stats)
                {
                    sb.AppendLine("Cache Accesses: " + AI.CacheAccesses + "\tCache Hits: " + AI.CacheHits + "\tCache Misses: " + AI.CacheMisses + "Hit Rate: " + ((100f) * (float)(AI.CacheHits) / (float)(AI.CacheAccesses)).ToString() + "%");
                    sb.AppendLine("CacheHitsOnBlue: " + AI.CacheHitsOnBlue);
                    AI.ResetCacheStats();
                }
            }
           
           sb.AppendLine("\n--------------\n");
            const string logpath = "PreviousTimeTestOutput.txt";

            try
            {
                StreamReader sr = new StreamReader(logpath);
                for (int i = 0; i < 1000; i++)
                {
                    sb.AppendLine(sr.ReadLine());
                    if (sr.EndOfStream)
                        break;
                }
                sr.Close();
            }
            catch (Exception e)
            {

            }
            Console.WriteLine(sb.ToString());
            StreamWriter sw = new StreamWriter(logpath);
            sw.Write(sb.ToString());
            sw.Close();

        }


    }
}
