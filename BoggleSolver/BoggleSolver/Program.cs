using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoggleSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = System.IO.File.Exists("input.txt") ? System.IO.File.ReadAllLines("input.txt") : System.IO.File.ReadAllLines("../../input.txt");
            string gridString = "";
            foreach (string line in input)
                gridString += line;

            foreach (string word in BoggleGrid.Solution(gridString).Distinct().OrderBy(w => w))
                Console.WriteLine(word);

            Console.ReadLine();
        }
    }
}
