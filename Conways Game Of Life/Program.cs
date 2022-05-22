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
            Console.WriteLine("Enter height: 20 is recommended");
            height = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Enter width: 40 is recommended");
            width = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            DrawStartArea();
        }
        static char[,] current; // Current state of the game
        static char[,] coming; // Next state of the game

        static void DrawStartArea()
        {
            current = new char[height, width];
            coming = new char[height, width];

            //generate a random number to decide what cells starts as living
            //Random rng = new Random();
            //int randomState = rng.Next(0, 1);

            //Write the actual play area
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    current[h, w] = coming[h, w] = deadCell;

                    //current[5, 5] = liveCell;
                    //current[5, 6] = liveCell;
                    //current[5, 7] = liveCell;
                    //current[4, 6] = liveCell;
                    
                    //current[5, 5] = liveCell;
                    //current[5, 6] = liveCell;
                    //current[5, 7] = liveCell;
                    //current[6, 7] = liveCell;
                    //current[6, 6] = liveCell;
                    //current[6, 5] = liveCell;
                    //current[7, 5] = liveCell;
                    
                    current[5, 5] = liveCell;
                    current[5, 6] = liveCell;
                    current[5, 7] = liveCell;
                    current[5, 8] = liveCell;
                    current[5, 9] = liveCell;
                    current[5, 10] = liveCell;

                    Console.Write(current[h, w]);
                    coming[h, w] = current[h, w];
                }
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            StartGame();
        }

        static void StartGame()
        {
            int generation = 1;

            //sets the title to the generation
            Console.Title = "Generation: " + generation;

            while (true)
            {
                //Copy current array to the next gen
                Array.Copy(current, coming, height * width);
                int neighbors = 0;

                //Select what dot to check for neighbors
                for (int h = 0; h < height; ++h)
                {
                    for (int w = 0; w < width; ++w)
                    {
                        if (h.Equals(0) || h.Equals(height - 1) || w.Equals(0) || w.Equals(width - 1))
                            continue;

                        char cellState = current[h, w];//Set state of current cell

                        neighbors = CountNeighbors(h, w, cellState);//Get current cells neighbors


                        //Rules for cell reproduction and decay
                        if (cellState == deadCell && neighbors == 3) //Dead and 3 neighbors -> Lives next round
                        {
                            coming[h, w] = liveCell;
                        }
                        else if (cellState == liveCell && (neighbors < 2 || neighbors > 3)) //Live and less than 2 or more than 3 neighbors ---> Dead next round
                        {
                            coming[h, w] = deadCell;
                        }
                        else if (cellState == liveCell && (neighbors == 2 || neighbors == 3)) // Live cell has 2 or 3 neighbors survives
                        {
                            coming[h, w] = liveCell;
                        }
                        else
                            coming[h, w] = cellState;
                        

                        neighbors = 0;

                        //Print the next generation after all cells has been checked
                        Console.Write(coming[h, w]);
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
                Thread.Sleep(100);
                Console.Clear();

                //reset for next check
                neighbors = 0;
            }
        }

        static int CountNeighbors(int h, int w, char cellState)
        {
            int neighbors = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)//Skip the center cell
                        continue;

                    if (current[h + i, w + j] == liveCell)
                        neighbors++;
                }
            }
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
