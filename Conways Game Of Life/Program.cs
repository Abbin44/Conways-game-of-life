using System;
using System.Threading;
using System.Drawing;
//using Console = Colorful.Console;

namespace Cells
{
    class Program
    {
        static int width;
        static int height;

        static char liveCell = '█';
        static char deadCell = '.';
        static char frame = '░';

        static void Main(string[] args)
        {
            string numbers = "0123456789";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Enter width: 20 is recommended");
            width = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Enter height: 40 is recommended");
            height = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            DrawStartArea();
        }
        static char[,] current; // Current state of the game
        static char[,] coming; // Next state of the game

        static void DrawStartArea()
        {
            current = new char[width, height];
            coming = new char[width, height];

            //generate a random number to decide what cells starts as living
            //Random rng = new Random();
            //int randomState = rng.Next(0, 1);

            //Write the actual play area
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    current[w, h] = coming[w, h] = deadCell;

                    //Only for testing, makes a square which should not move or die

                    current[5, 5] = liveCell;
                    current[5, 6] = liveCell;
                    current[5, 7] = liveCell;
                    current[6, 7] = liveCell;
                    current[6, 6] = liveCell;
                    current[6, 5] = liveCell;
                    current[7, 5] = liveCell;

                    
                    current[5, 5] = liveCell;
                    current[5, 6] = liveCell;
                    current[5, 7] = liveCell;
                    current[5, 8] = liveCell;
                    current[5, 9] = liveCell;
                    current[5, 10] = liveCell;
                    
                    Console.Write(current[w, h]);
                    coming[w, h] = current[w, h];
                }
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            StartGame();
        }

        static int neighbors = 0;
        static void StartGame()
        {
            int generation = 1;

            //sets the title to the generation
            Console.Title = "Generation: " + generation;

            while (true)
            {
                //Copy current array to the next gen
                Array.Copy(current, coming, height * width);

                //Select what dot to check for neighbors
                for (int w = 1; w < width; w++)
                {
                    for (int h = 1; h < height; h++)
                    {
                        //Set state of current cell
                        char cellState = current[w, h];

                        //Get current cells neighbors
                        neighbors = CountNeighbors(w, h, cellState);

                        //Rules for cell reproduction and decay
                        if (cellState == deadCell && neighbors == 3) //Dead and 3 neighbors -> Lives next round
                        {
                            coming[w, h] = liveCell;
                        }
                        else if (cellState == liveCell && (neighbors < 2 || neighbors > 3)) //Live and less than 2 or more than 3 neighbors ---> Dead next round
                        {
                            coming[w, h] = deadCell;
                        }
                        else if (cellState == liveCell && (neighbors == 2 || neighbors == 3)) // Live cell has 2 or 3 neighbors survives
                        {
                            coming[w, h] = liveCell;
                        }
                        else
                            coming[w, h] = cellState;
                        

                        neighbors = 0;

                        //Print the next generation after all cells has been checked
                        Console.Write(coming[w, h]);
                    }
                    Console.WriteLine();
                }
                //Copy the next array to current one to prepare for the next generation
                Array.Copy(coming, current, height * width);

                //sets the title to the current generation
                Console.Title = "Generation: " + generation;
                generation++;

                //Change color
                Console.ForegroundColor = ChangeColor();

                //Add some delay to be able to see anything
                Thread.Sleep(50);
                Console.Clear();

                //reset for next check
                neighbors = 0;
            }
        }

        static int CountNeighbors(int w, int h, char cellState)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int widthIndex = w + i;
                    int heightIndex = h + j;

                    //If the current index is outside the array, just ignore it
                    if (widthIndex > height - 1 || heightIndex > width - 1)
                        continue;

                    int p = (h + j);
                    int s = (w + i);

                    if (current[s, p] == liveCell)
                        neighbors++;
                }
            }

            //remove the cell being checked
            if (cellState == liveCell)
                neighbors--;

            return neighbors;
        }

        private static Random random = new Random();
        static ConsoleColor ChangeColor()
        {
            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            return (ConsoleColor)consoleColors.GetValue(random.Next(consoleColors.Length));
        }
    }
}
