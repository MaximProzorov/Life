using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Life
{
    class Cell
    {
        public int is_live;
        public int neighbors_count;
        public Cell(int is_live)
        {
            this.is_live = is_live;
        }
        public void Next_step(ref int flag)
        {
            if (is_live ==1)
            {
                switch(neighbors_count)
                {
                    case 0:
                        is_live = 0;
                        flag = 0;
                        return;
                    case 1:
                        goto case 0;
                    case 2:
                        return;
                    case 3:
                        goto case 2;
                    default:
                        goto case 0;
                }
            }
            else
            {
                if (neighbors_count == 3)
                {
                    is_live = 1;
                    flag = 0;
                }
            }
        }
    }
    
    class Game
    {
        Cell[,] field;
        int n;
        int m;
        public Game(int n, int m)
        {

            field = new Cell[n, m];
            this.n = n;
            this.m = m;
            Random rand = new Random();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    field[i, j] = new Cell(rand.Next(2));
                }
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    field[i, j].neighbors_count = Neighbors_count(i,j);
                }
        }
        public Game(int n, int m, int[,] field_from_file)
        {

            field = new Cell[n, m];
            this.n = n;
            this.m = m;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    field[i, j] = new Cell(field_from_file[i,j]);
                }
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    field[i, j].neighbors_count = Neighbors_count(i, j);
                }
        }
        int Neighbors_count(int x, int y)
        {
            int count = 0;
            for (int i = x - 1; i < x + 2; i++)
            {
                if (i < 0 || i >= n)
                    continue;
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (j < 0 || j >= m || ((i == x) && (j == y)))
                        continue;
                    if (field[i, j].is_live == 1)
                        count++;
                }

            }
            return count;
        }
        public void Game_step(out int flag)
        {
            flag = 1;
            for(int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    field[i, j].Next_step(ref flag);
                }
            
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    field[i, j].neighbors_count = Neighbors_count(i, j);
                }
        }
        public void Display()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(" {0} ", field[i, j].is_live);
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int n = 10;
            int m = 10;

            

            int flag;
            Game gm;
            int[,] field = new int[n, m];

            

            if (args.Length > 1)
            {
                Console.WriteLine("Неверно указаны параметры запуска");
                return;
            }
            else if (args.Length == 1)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(args[0]))
                    {
                        string line;
                        int j = -1;
                        while((line=sr.ReadLine()) != null)
                        {
                            j++;
                            if (line.Length!=n || j > m)
                            {
                                Console.WriteLine("Неверно указана стартовая позиция в файле");
                                return;
                            }
                            for(int i = 0; i < n; i++)
                            {
                                field[j, i] = (int)(line[i] - '0');
                            }
 
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Ошибка чтения файла");
                }
            }
            else
            {
                Random rand = new Random();
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                    {
                        field[i, j] = rand.Next(2);
                    }

            }
            gm = new Game(n, m, field);
            Console.Clear();
            do
            {
                
                Console.SetCursorPosition(0,0);
                gm.Display();
                gm.Game_step(out flag);
                System.Threading.Thread.Sleep(1000);
            } while (flag != 1);
            Console.ReadKey();
            
        }
   
    }
}
