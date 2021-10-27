using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Lab3Q1;
using static Lab3Q1.HelperFunctions;

namespace WordCounttesting
{
    class Program
    {
        static void Main(string[] args)
        {
          // map and mutex for thread safety
          Mutex mutex = new Mutex();
          Dictionary<string, int> wcountsSingleThread = new Dictionary<string, int>();
           Dictionary<string, int> wcountsMultiThread = new Dictionary<string, int>();
            Stopwatch stopwatch_singlethread = new Stopwatch();
            Stopwatch stopwatch_multithread = new Stopwatch();

            var filenames = new List<string> {
                "../../data/shakespeare_antony_cleopatra.txt",
                "../../data/shakespeare_hamlet.txt",
                "../../data/shakespeare_julius_caesar.txt",
                "../../data/shakespeare_king_lear.txt",
                "../../data/shakespeare_macbeth.txt",
                "../../data/shakespeare_merchant_of_venice.txt",
                "../../data/shakespeare_midsummer_nights_dream.txt",
                "../../data/shakespeare_much_ado.txt",
                "../../data/shakespeare_othello.txt",
                "../../data/shakespeare_romeo_and_juliet.txt",
           };

            //=============================================================
            // YOUR IMPLEMENTATION HERE TO COUNT WORDS IN SINGLE THREAD
            //=============================================================

            stopwatch_singlethread.Start();
            foreach (string i in filenames)
            {
               
                CountCharacterWords(i, mutex, wcountsSingleThread);
        
            }
            List<Tuple<int, string>> sortedlist = new List<Tuple<int, string>>();
            sortedlist = SortCharactersByWordcount(wcountsSingleThread);
            stopwatch_singlethread.Stop();
            PrintListofTuples(sortedlist);
            TimeSpan ts_single = stopwatch_singlethread.Elapsed;

            string elapsedTime_single = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts_single.Hours, ts_single.Minutes, ts_single.Seconds,
                ts_single.Milliseconds / 10);
            Console.WriteLine("RunTime for Single Thread " + elapsedTime_single);

            Console.WriteLine( "SingleThread is Done!");
            //=============================================================
            // YOUR IMPLEMENTATION HERE TO COUNT WORDS IN MULTIPLE THREADS
            //=============================================================
            
            //int num_threads = 12;
            List<Thread> threads = new List<Thread>();
            stopwatch_multithread.Start();
            foreach (string i in filenames)
            {   
                    Thread thread = new Thread(() => CountCharacterWords(i, mutex, wcountsMultiThread));
                    thread.Start();
                    threads.Add(thread);

            }
                foreach (Thread t in threads)
                    t.Join();

            List<Tuple<int, string>> sortedlist_multi = new List<Tuple<int, string>>();
            
            sortedlist_multi = SortCharactersByWordcount(wcountsMultiThread);
            stopwatch_multithread.Stop();
            PrintListofTuples(sortedlist_multi);
            
            
            TimeSpan ts_multi = stopwatch_multithread.Elapsed;
            string elapsedTime_multi = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
               ts_multi.Hours, ts_multi.Minutes, ts_multi.Seconds,
               ts_multi.Milliseconds / 10);
            Console.WriteLine("RunTime for MultiThreading" + elapsedTime_multi);




            Console.WriteLine( "MultiThread is Done!");
           //return 0;
        }
    }
}
