using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoggleSolver
{
    /// <summary>
    /// A static class providing methods that determine the validity of possible words.
    /// </summary>
    public static class WordFinder
    {
        /// <summary>
        /// An enumerated type indicating possible results of comparing two strings. All possibilities are from the perspective of the first string being compared.
        /// "LessThan" indicates that the first different character, reading left to right, has a lower ASCII number in the first word than in the second word. e.g. car to cat
        /// "GreaterThan" indicates that the first different character, reading left to right, has a higher ASCII number in the first word than in the second word. e.g. hat to bat
        /// "EqualTo" indicates that the two strings are identical. e.g. aardvark to aardvark
        /// "ShorterThan" indicates that all comparable characters are the same, but the first word is shorter than the second word. e.g. jump to jumps
        /// "LongerThan" indicates that all comparable characters are the same, but the first word is longer than the second word. e.g. barmaid to bar
        /// </summary>
        private enum StringCompare
        {
            LessThan,
            GreaterThan,
            EqualTo,
            ShorterThan,
            LongerThan
        }

        /// <summary>
        /// An enumerated type representing validity of words.
        /// "Invalid" refers to a collection of letters that is not a word, and it not found, in order, at the beginning of any word.
        /// "Possible" refers to a collection of letter that is not a word, but is found, in order, at the start of at least one word.
        /// "Real" refers to a collection of letters that is a word (i.e., found in the list of words).
        /// </summary>
        public enum WordValidity
        {
            Invalid,
            Possible,
            Real
        }

        /// <summary>
        /// The list of English words this class will use to determine validity of words
        /// </summary>
        private static string[] words = System.IO.File.ReadAllLines("dictionary.txt");

        /// <summary>
        /// Checks for the validity of a given string, using a list of English words.
        /// </summary>
        /// <param name="check">The string for which to find its validity.</param>
        /// <returns>The WordValidity value that identifies the given string's validity.</returns>
        public static WordValidity FindWord(string check)
        {
            int currChange = (words.Length % 2 == 0) ? (words.Length / 2) : ((words.Length / 2) + 1);
            int currIndex = currChange;

            bool hasShorterThan = false;

            //This is a modified binary search that checks for both Real words and Possible words.
            while (true)
            {
                StringCompare compare = Compare(check, words[currIndex]);
                
                if (compare == StringCompare.EqualTo)
                    return WordValidity.Real;

                if (compare == StringCompare.ShorterThan)
                    hasShorterThan = true;

                //If the current number of indexes to change is 1, all possible words in the list have been checked.
                if (currChange == 1)
                    break;

                currChange = (currChange % 2 == 0) ? (currChange / 2) : ((currChange / 2) + 1);

                //Determine the next index in the word list to check, based on the results of the string comparison.
                if (compare == StringCompare.LessThan || compare == StringCompare.ShorterThan)
                    currIndex -= currChange;

                if (compare == StringCompare.GreaterThan || compare == StringCompare.LongerThan)
                    currIndex += currChange;

                //These statements ensure that no exceptions will occur and that all indexes are checked.
                if (currIndex < 0)
                    currIndex = 0;
                if (currIndex >= words.Length)
                    currIndex = words.Length - 1;
            }

            //If the string to check for has been ShorterThan at least one word, it is a Possible word.
            //Otherwise, it is an Invalid word.
            if (hasShorterThan)
                return WordValidity.Possible;
            else
                return WordValidity.Invalid;
        }

        /// <summary>
        /// Compares two strings and gives the StringCompare value that identifies the relationship between the two words.
        /// </summary>
        /// <param name="compareFor">The first word to compare.</param>
        /// <param name="compareAgainst">The second word to compare.</param>
        /// <returns>The WordValidity value that identifies the relationship between the two given words.</returns>
        private static StringCompare Compare(string compareFor, string compareAgainst)
        {
            int length;

            if (compareFor.Length < compareAgainst.Length)
                length = compareFor.Length;
            else
                length = compareAgainst.Length;

            for (int i = 0; i < length; i++)
            {
                if (compareFor[i] > compareAgainst[i])
                    return StringCompare.GreaterThan;
                if (compareFor[i] < compareAgainst[i])
                    return StringCompare.LessThan;
            }

            if (compareFor.Length < compareAgainst.Length)
                return StringCompare.ShorterThan;

            if (compareFor.Length > compareAgainst.Length)
                return StringCompare.LongerThan;

            return StringCompare.EqualTo;
        }
    }
}
