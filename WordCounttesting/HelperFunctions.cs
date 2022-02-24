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


             string line;  // for storing each line read from the file
             string character = "";  // empty character to start
            int wordcount;
            int start_index;
             System.IO.StreamReader file = new System.IO.StreamReader(filename);

             while ((line = file.ReadLine()) != null)
             {

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
