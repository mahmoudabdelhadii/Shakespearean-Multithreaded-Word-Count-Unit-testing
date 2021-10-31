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
            Console.WriteLine("                                                                     RunTime for Single Thread " + elapsedTime_single);

            Console.WriteLine( "                                                                    SingleThread is Done!");
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
            Console.WriteLine("                                                                           RunTime for MultiThreading" + elapsedTime_multi);




            Console.WriteLine( "                                                                          MultiThread is Done!");
           //return 0;
        }
    }
}
using System;
using System.Collections.Generic;
namespace Lab3Q1
{
    public class WordCountTester
    {
        public static int Main()
        {

            List<Tuple<int, int, string>> LineList = new List<Tuple<int, int, string>>()
            {
                Tuple.Create(0,0,  "."),
                Tuple.Create(0,0,  ""),
                Tuple.Create(1,0,  "           Hi"),
                Tuple.Create(1,0,  "1          2"),
                Tuple.Create(1,5,  "1          2 "),
                Tuple.Create(16,7, "1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 "),
                Tuple.Create(0,0,  " "),
                Tuple.Create(0,0,  "                "),
                Tuple.Create(3,0,  "  this           that    thus      "),
                Tuple.Create(6, 0, "This sentence is 6 words long"),
                Tuple.Create(42,0,  " This report outlines the status quo and future prospects regarding nuclear arms control by specifically discussing the New START  treaty between the US and Russia, the JCPOA  between Iran and the P5+1, the NPT, CTBT, and various other treaties between its signatories."),
                Tuple.Create(80, 10,"< <> ! !@ this is a very very very very <> ! !@ very very very <> ! !@  very . very . very @ very % <> ! !@ very very very <> ! !@ very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very <> ! !@ very very very <> ! !@ very very  very very very very long sentence")
            };
            
          
           
            string line;  // for storing each line read from the file


            for (int i = 0; i < LineList.Count; i++)
            { 
                line = LineList[i].Item3;
                int startIdx = LineList[i].Item2;
                int expectedResults = LineList[i].Item1;
                try
                {


                    //=================================================
                    // Implement your tests here. Check all the edge case scenarios.
                    // Create a large list which iterates over WCTester
                    //=================================================

                    WCTester(line, startIdx, expectedResults);

                }
                catch (UnitTestException e)
                {
                    Console.WriteLine(e);
                }

                
            }
            return 0;
        }
        /**
         * Tests word_count for the given line and starting index
         * @param line line in which to search for words
         * @param start_idx starting index in line to search for words
         * @param expected expected answer
         * @throws UnitTestException if the test fails
         */
          static void WCTester(string line, int start_idx, int expected) {

            //=================================================
            // Implement: comparison between the expected and
            // the actual word counter results
            //=================================================
            int result;
            result =Lab3Q1.HelperFunctions.WordCount(ref line, start_idx);
            if (result != expected)
            {
              throw new Lab3Q1.UnitTestException(ref line, start_idx, result, expected, String.Format("UnitTestFailed: result:{0} expected:{1}, line: {2} starting from index {3}", result, expected, line, start_idx));
            }

           }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;


namespace Lab3Q1
{
    public class HelperFunctions
    {
        /**
         * Counts number of words, separated by spaces, in a line.
         * @param line string in which to count words
         * @param start_idx starting index to search for words
         * @return number of words in the line
         */
        public static int WordCount(ref string line, int start_idx)
        {
            // YOUR IMPLEMENTATION HERE
            //stackoverflow
            int count = 0;
            bool wasInWord = false;
            bool inWord = false;

            for (int i = start_idx; i < line.Length; i++)
            {
                if (inWord)
                {
                    wasInWord = true;
                }

                if (Char.IsWhiteSpace(line[i]))
                {
                    if (wasInWord)
                    {
                        count++;
                        wasInWord = false;
                    }
                    inWord = false;
                }
                else
                {
                    inWord = true;
                }
            }

            // Check to see if we got out with seeing a word
            if (wasInWord)
            {
                count++;
            }

            return count;

        }


        /**
        * Reads a file to count the number of words each actor speaks.
        *
        * @param filename file to open
        * @param mutex mutex for protected access to the shared wcounts map
        * @param wcounts a shared map from character -> word count
        */
        public static void CountCharacterWords(string filename,
                                 Mutex mutex,
                                 Dictionary<string, int> wcounts)
        {

            //===============================================
            //  IMPLEMENT THIS METHOD INCLUDING THREAD SAFETY
            //===============================================

             string line;  // for storing each line read from the file
             string character = "";  // empty character to start
            int wordcount;
            int start_index;
             System.IO.StreamReader file = new System.IO.StreamReader(filename);

             while ((line = file.ReadLine()) != null)
             {

                //=================================================
                // YOUR JOB TO ADD WORD COUNT INFORMATION TO MAP
                //=================================================
                if (IsDialogueLine(line, ref character) > 0)
                {
                    start_index = IsDialogueLine(line, ref character);
                    
                    if (start_index > 0 & character != null)
                    {
                        wordcount = WordCount(ref line, start_index);
                        if (wcounts.ContainsKey(character))
                        {
                            mutex.WaitOne();
                            wcounts[character] = wcounts[character] + wordcount;
                            mutex.ReleaseMutex();
                        }
                        else if (character != "")
                        {
                            mutex.WaitOne();
                            wcounts.Add(character, wordcount);
                            mutex.ReleaseMutex();
                        }
                        

                    }
                    //character = "";

                }
                // Is the line a dialogueLine?
                //    If yes, get the index and the character name.
                //      if index > 0 and character not empty
                //        get the word counts
                //          if the key exists, update the word counts
                //          else add a new key-value to the dictionary
                //    reset the character
                //character = "";
            }
            // Close the file
            file.Close();
            file.Dispose();
        }



        /**
         * Checks if the line specifies a character's dialogue, returning
         * the index of the start of the dialogue.  If the
         * line specifies a new character is speaking, then extracts the
         * character's name.
         *
         * Assumptions: (doesn't have to be perfect)
         *     Line that starts with exactly two spaces has
         *       CHARACTER. <dialogue>
         *     Line that starts with exactly four spaces
         *       continues the dialogue of previous character
         *
         * @param line line to check
         * @param character extracted character name if new character,
         *        otherwise leaves character unmodified
         * @return index of start of dialogue if a dialogue line,
         *      -1 if not a dialogue line
         */
        static int IsDialogueLine(string line, ref string character)
        {

            //stage direction/ character entrance
            if (line.Length >= 7 && line[0] == ' '
              && line[1] == ' ' && line[2] == ' '
              && line[3] == ' ' && line[4] == ' '
               && line[5] == ' ' && line[6] == ' ')
            {
                //not dialogue line
                character = "";
                return 0;
                
            }
            else
            {
                if (line.Length >= 3 && line[0] == ' '
                    && line[1] == ' ' && line[2] != ' ')
                {
                    // extract character name

                    int start_idx = 2;
                    int end_idx = 3;
                    while (end_idx <= line.Length && line[end_idx - 1] != '.')
                    {
                        ++end_idx;
                    }

                    // no name found
                    if (end_idx >= line.Length)
                    {
                        return 0;
                    }

                    // extract character's name
                    character = line.Substring(start_idx, end_idx - start_idx - 1);
                    return end_idx;
                }

                // previous character
                if (line.Length >= 5 && line[0] == ' '
                    && line[1] == ' ' && line[2] == ' '
                    && line[3] == ' ' && line[4] != ' ')
                {
                    // continuation
                    return 4;
                }
            }
            return 0;
        }

        /**
         * Sorts characters in descending order by word count
         *
         * @param wcounts a map of character -> word count
         * @return sorted vector of {character, word count} pairs
         */
        public static List<Tuple<int, string>> SortCharactersByWordcount(Dictionary<string, int> wordcount)
        {

            wordcount = wordcount.OrderByDescending(key => key.Value).ToDictionary(x => x.Key, x => x.Value); ;
            List<Tuple<int, string>> sortedByValueList = new List<Tuple<int, string>>();
            // Implement sorting by word count here
          for (int i = 0; i < wordcount.Count; i++)
          {
                sortedByValueList.Add(new Tuple<int, string>(wordcount.ElementAt(i).Value, wordcount.ElementAt(i).Key));
          }


            return sortedByValueList;

        }


        /**
         * Prints the List of Tuple<int, string>
         *
         * @param sortedList
         * @return Nothing
         */
        public static void PrintListofTuples(List<Tuple<int, string>> sortedList)
        {

          // Implement printing here
          for (int i = 0; i < sortedList.Count; i++)
          {
                
                
                
                Console.WriteLine("CHARACTER: {0}    |    WORDS SPOKEN: {1}", sortedList[i].Item2.ToString(), sortedList[i].Item1.ToString());
                Console.WriteLine("----------------------------------------------");
            }

        }
    }
}
