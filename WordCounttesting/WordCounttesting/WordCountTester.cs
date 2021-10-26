using System;
using System.Collections.Generic;
namespace Lab3Q1
{
    public class WordCountTester
    {
        public static int Main()
        {

            List<Tuple<int,int, string>> LineList = new List<Tuple<int,int, string>>()
            {
                Tuple.Create(0,0,  "."),
                Tuple.Create(0,0,  ""),
                Tuple.Create(1,0,  "           Hi"),
                Tuple.Create(1,0,  "1          2"),
                Tuple.Create(1,5,  "1          2 "),
                Tuple.Create(16,7,  "1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 "),
                Tuple.Create(0,0,  " "),
                Tuple.Create(0,0,  "                "),
                Tuple.Create(6, 0, "This sentence is 6 words long"),
                Tuple.Create(42,0,  " This report outlines the status quo and future prospects regarding nuclear arms control by specifically discussing the New START  treaty between the US and Russia, the JCPOA  between Iran and the P5+1, the NPT, CTBT, and various other treaties between its signatories.")

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
