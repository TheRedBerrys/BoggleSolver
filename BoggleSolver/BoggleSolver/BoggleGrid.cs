using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoggleSolver
{
    /// <summary>
    /// Represents a square Boggle grid of any length side.
    /// </summary>
    class BoggleGrid
    {
        /// <summary>
        /// The letters in the current grid.
        /// </summary>
        private string Letters { get; set; }
        /// <summary>
        /// The list of all words found in this grid.
        /// </summary>
        private List<string> Words { get; set; }
        /// <summary>
        /// The number of rows and columns (which will be the same because it's a square grid).
        /// </summary>
        private int RowsColumns { get; set; }
        /// <summary>
        /// The minimum length of words to accept in this grid.
        /// </summary>
        private int MinimumLength { get; set; }

        /// <summary>
        /// Generates a Boggle grid with a given set of letters and a minimum word length to accept.
        /// </summary>
        /// <param name="letters">The letters to form the grid.</param>
        /// <param name="minimumLength">The minimum length of words to accept for this grid. Defaults to 3.</param>
        private BoggleGrid(string letters, int minimumLength = 3)
        {
            double rowsColumns = Math.Sqrt(letters.Length);
            if (Convert.ToInt32(rowsColumns) != rowsColumns)
            {
                Console.WriteLine("Not a valid input string.");
                return;
            }

            this.RowsColumns = Convert.ToInt32(rowsColumns);
            this.Letters = letters.ToLower();
            this.Words = new List<string>();
            this.MinimumLength = minimumLength;
        }

        /// <summary>
        /// Gives the string formed from a given list of indexes.
        /// </summary>
        /// <param name="indexes">A List of ints representing the indexes from which to form the string.</param>
        /// <returns>A string formed from the given indexes on the Boggle grid.</returns>
        private string CurrentString(List<int> indexes)
        {
            string answer = "";

            for (int i = 0; i < indexes.Count; i++)
                answer += this.Letters[indexes[i]];

            return answer;
        }

        /// <summary>
        /// Gives a list of possible next indexes from a given list of indexes.
        /// If the given list of indexes is empty, returns all the indexes in the grid.
        /// </summary>
        /// <param name="indexes">A List of ints representing the indexes for which to find the next index.</param>
        /// <returns>The list of all the indexes that could be the next index of the given list.</returns>
        private List<int> PossibleNextIndexes(List<int> indexes)
        {
            List<int> answer = new List<int>();

            if (indexes.Count == 0)
            {
                for (int i = 0; i < this.Letters.Length; i++)
                    answer.Add(i);
            }
            else
            {
                int currIndex = indexes.LastOrDefault();

                bool hasLeft = currIndex % this.RowsColumns > 0;
                bool hasRight = currIndex % this.RowsColumns < this.RowsColumns - 1;
                bool hasAbove = currIndex - this.RowsColumns > 0;
                bool hasBelow = currIndex + this.RowsColumns < this.Letters.Length;

                if (hasAbove)
                {
                    //Index above
                    if (!indexes.Contains(currIndex - this.RowsColumns))
                        answer.Add(currIndex - this.RowsColumns);

                    //Index left above
                    if (hasLeft && !indexes.Contains(currIndex - this.RowsColumns - 1))
                        answer.Add(currIndex - this.RowsColumns - 1);

                    //Index right above
                    if (hasRight && !indexes.Contains(currIndex - this.RowsColumns + 1))
                        answer.Add(currIndex - this.RowsColumns + 1);
                }

                //Index left
                if (hasLeft && !indexes.Contains(currIndex - 1))
                    answer.Add(currIndex - 1);

                //Index right
                if (hasRight && !indexes.Contains(currIndex + 1))
                    answer.Add(currIndex + 1);

                if (hasBelow)
                {
                    //Index below
                    if (!indexes.Contains(currIndex + this.RowsColumns))
                        answer.Add(currIndex + this.RowsColumns);

                    //Index below left
                    if (hasLeft && !indexes.Contains(currIndex + this.RowsColumns - 1))
                        answer.Add(currIndex + this.RowsColumns - 1);

                    //Index below right
                    if (hasRight && !indexes.Contains(currIndex + this.RowsColumns + 1))
                        answer.Add(currIndex + this.RowsColumns + 1);
                }
            }

            return answer;
        }

        /// <summary>
        /// Recursively finds all the English words starting from a given chain in the grid.
        /// </summary>
        /// <param name="indexes">The list of indexes that represent the chain from which to start.</param>
        /// <param name="print">Whether to print the words found to the console immediately. Defaults to false.</param>
        private void Solve(List<int> indexes, bool print = false)
        {
            string currString = CurrentString(indexes);

            WordFinder.WordValidity validity = WordFinder.FindWord(currString);

            //If the string is a real English word, add it to the list of words found
            if (validity == WordFinder.WordValidity.Real && currString.Length >= this.MinimumLength)
            {
                this.Words.Add(currString);
                if (print)
                    Console.WriteLine(currString);
            }

            //If the string is possibly the start of a word, find all the indexes that could be the next letter in the word and test if each is a word.
            //Also, recursively try to find all words that can be built from that chain, and so on.
            if (validity != WordFinder.WordValidity.Invalid)
            {
                List<int> possibleNextIndexes = PossibleNextIndexes(indexes);

                foreach (int nextIndex in possibleNextIndexes)
                {
                    List<int> newIndexes = new List<int>();
                    foreach (int index in indexes)
                        newIndexes.Add(index);

                    newIndexes.Add(nextIndex);

                    Solve(newIndexes, print);
                }
            }
        }

        /// <summary>
        /// Solves the grid from start to finish.
        /// </summary>
        /// <param name="print">Whether to print the words as they are found. Defaults to false.</param>
        private void Solve(bool print = true)
        {
            List<int> indexes = new List<int>();
            Solve(indexes, print);
            Console.WriteLine("Execution complete.");
        }

        /// <summary>
        /// Creates a new grid and returns all the words found in that grid.
        /// </summary>
        /// <param name="letters">The letters from which to form the grid.</param>
        /// <param name="minimumLength">The minimum length of words to find in the grid. Defaults to 3.</param>
        /// <returns>The list of words found in the grid formed from the given letters.</returns>
        public static List<string> Solution(string letters, int minimumLength = 3)
        {
            BoggleGrid grid = new BoggleGrid(letters, minimumLength);
            grid.Solve(false);
            return grid.Words;
        }
    }
}
