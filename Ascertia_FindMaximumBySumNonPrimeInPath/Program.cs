using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascertia_FindMaximumBySumNonPrimeInPath
{
    class Program
    {
        static void Main(string[] args)
        {
            //Input from File 
            string input = ReadFromFile();

            // break input into multiple rows
            string[] inputArrayByLines = input.Split('\n');

            // convert rows to proper flate triangle to table format.
            var inputTable = ConvertTriToTable(inputArrayByLines);

            //find maximum sum by moving down , exclude prime items , move diagonally 
            int result = MoveDownThroughTheNodes(inputArrayByLines, inputTable);

            Console.WriteLine($"The Maximum Sum Of Non-Prime Numbers From Top To Bottom Is:  {result}");

            Console.ReadKey();
        }
        private static string ReadFromFile()
        {
            string line;
            string input = "";
            int counter = 0;

            // Read the file and display it line by line.  
            Console.WriteLine("Enter proper complete File Location");
            string FilePath = Console.ReadLine();
            System.IO.StreamReader file = new System.IO.StreamReader(@"E:\input.txt");//FilePath
            while ((line = file.ReadLine()) != null)
            {
                //use "\n" for new line differentiate which help in converting to table / 2D Array
                input += line + "\n";
                counter++;
            }
            return input;
        }
        private static int[,] ConvertTriToTable(string[] inputArrayByLines)
        {
            int[,] inputTable = new int[inputArrayByLines.Length, inputArrayByLines.Length + 1];

            for (int r = 0; r < inputArrayByLines.Length; r++)
            {
                var eachCharactersInRow = inputArrayByLines[r].Trim().Split(' ');

                //File the table
                for (int c = 0; c < eachCharactersInRow.Length; c++)
                {
                    int number;
                    int.TryParse(eachCharactersInRow[c], out number);
                    inputTable[r, c] = number;
                }
            }
            return inputTable;
        }

        private static int MoveDownThroughTheNodes(string[] inputArrayByLines, int[,] inputTable)
        {
            // storing the sum of parent and child node and store as key value pair 
            // key => index of Parent node  and Value = sum of parent and child node 
            Dictionary<string, int> paths = new Dictionary<string, int>();

            // validate if root node is prime or non prime
            if ((IsPrime(inputTable[0, 0])))
            {
                return inputTable[0, 0]; //false
            }
            //Adding first value as total value and save as key as i:0 j:0
            paths.Add("i: " + 0 + " j: " + 0, inputTable[0, 0]);
            // Moving Down through the non-prime node
            for (int i = 0; i <= inputArrayByLines.Length - 2; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                   // checked left child is Prime or Non-Prime and left child Node value 
                    bool left = (!IsPrime(inputTable[i + 1, j]));
                    int childNodeleft = inputTable[i + 1, j];

                    // checked right child is Prime or Non-Prime and Right Node Value.
                    bool right = (!IsPrime(inputTable[i + 1, j + 1]));
                    int childNodeRight = inputTable[i + 1, j + 1];

                    //only moving through the non-prime node and Sum them
                    if ((IsPrime(inputTable[i, j])))
                    {
                        inputTable[i, j] = 2;

                        // if parent node is left most Prime and left most child should not able to part of path so make them Prime.
                        if (j == 0 && left == true)
                            inputTable[i + 1, j] = 2;

                        // if parent node is Right most Prime and Right most child should not able to part of path so make them Prime.
                        if (j == i && right == true)
                            inputTable[i + 1, j + 1] = 2;

                        continue; //false
                    }

                    // get previous summed value which was saved as to current indexing by previous cycle.
                    int pre = paths.ContainsKey(("i: " + i + " j: " + j).ToString()) ? paths[("i: " + i + " j: " + j).ToString()] : inputTable[i, j];

                    // When both child are Non Prime , add there value to parent value and store on dictionary as key will be as per child indexing.
                    if (left == true && right == true)
                    {
                        //Working for  Left child , add previous to current result and save in dictioary on key as to next row current location. 
                        int previous = (pre + inputTable[i + 1, j]);
                        int indexI = i + 1;
                        int indexJ = j;
                        // paths.Add("i: " + indexI + " j: " + indexJ, previous);
                        if (paths.ContainsKey("i: " + indexI + " j: " + indexJ))
                        {
                            paths["i: " + indexI + " j: " + indexJ] = paths["i: " + indexI + " j: " + indexJ] > previous ? paths["i: " + indexI + " j: " + indexJ] : previous;
                        }
                        else
                            paths.Add("i: " + indexI + " j: " + indexJ, previous);


                        //Working for  Right child , add previous to current result and save in dictioary on key as to next row current location. 
                        previous = (pre + inputTable[i + 1, j + 1]);
                        indexI = (i + 1);
                        indexJ = (j + 1);
                        //paths.Add("i: " + indexI + " j: " + indexJ, previous);
                        if (paths.ContainsKey("i: " + indexI + " j: " + indexJ))
                        {
                            paths["i: " + indexI + " j: " + indexJ] = paths["i: " + indexI + " j: " + indexJ] > previous ? paths["i: " + indexI + " j: " + indexJ] : previous;
                        }
                        else
                            paths.Add("i: " + indexI + " j: " + indexJ, previous);

                    }
                    else // When left is Non Prime and add parent result to current result and save in dictioary on key as to Child indexing. 
                    if (left == true && right == false)
                    {
                        int previous = paths.ContainsKey("i: " + i + " j: " + j) ? (paths["i: " + i + " j: " + j] + inputTable[i + 1, j]) : inputTable[i + 1, j];
                        int indexI = i + 1;

                        //paths.Add("i: " + indexI + " j: " + j, previous);
                        if (paths.ContainsKey("i: " + indexI + " j: " + j))
                        {
                            paths["i: " + indexI + " j: " + j] = paths["i: " + indexI + " j: " + j] > previous ? paths["i: " + indexI + " j: " + j] : previous;
                        }
                        else
                            paths.Add("i: " + indexI + " j: " + j, previous);
                        //tableHolder[i + 1, j] = (tableHolder[i, j] + tableHolder[i + 1, j]);
                    }
                    else if (left == false && right == true)
                    {
                        int previous = paths.ContainsKey("i: " + i + " j: " + j) ? (paths["i: " + i + " j: " + j] + inputTable[i + 1, j + 1]) : inputTable[i + 1, j + 1];
                        int indexI = i + 1;
                        int indexJ = j + 1;
                        //paths.Add("i: " + indexI + " j: " + indexJ, previous);

                        if (paths.ContainsKey("i: " + indexI + " j: " + indexJ))
                        {
                            paths["i: " + indexI + " j: " + indexJ] = paths["i: " + indexI + " j: " + indexJ] > previous ? paths["i: " + indexI + " j: " + indexJ] : previous;
                        }
                        else
                            paths.Add("i: " + indexI + " j: " + indexJ, previous);

                        //tableHolder[i + 1, j + 1] = (tableHolder[i, j] + tableHolder[i + 1, j + 1]);
                    }
                    else
                    {
                        // if both child are Prime Numbers,  ignore and move to next steps 
                        continue;
                    }
                }
            }
            // get the Maximum Value. 
            int maxValue = paths.Values.Max();
            return maxValue;
        }

        public static bool IsPrime(int number)
        {
            // Test whether the parameter is a prime number.
            if ((number & 1) == 0)
            {
                if (number == 2)
                {
                    return true;
                }
                return false;
            }

            for (int i = 3; (i * i) <= number; i += 2)
            {
                if ((number % i) == 0)
                {
                    return false;
                }
            }
            return number != 1;
        }
    }
}
