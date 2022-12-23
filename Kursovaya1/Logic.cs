using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovaya1
{
    class Logic
    {
         Square[,] gamefield = new Square[sizeField, sizeField];
        int X = -1;
        int Y = -1;

        const int sizeField = 10;
        const int empty = -1;
        const int moves = 7;

        public int leftmoves = moves; 

        int activeGem1 = -1;
        int activeGem2 = -1;

        bool switchs;
        public int score { get; set; } 

        List<Square> listOfGems = new List<Square>();
        Random rng = new Random();
        public EventHandler Falling;

        public Logic(Square[,] gamefield)
        {
            this.gamefield = gamefield;
        }

        public void GameSetScore(int score) //!
        {
            this.score = score;
        }

        private void FallGem()
        {
            Complite();
            if (EmptyGem())
            {
                while (EmptyGem())
                {
                    Fallings();
                    Falling(this, null);
                    Thread.Sleep(200);
                }
                BeginFalling();
            }
        }

        public void BeginFalling()
        {
            Thread newThread = new Thread(new ThreadStart(FallGem));
            newThread.Start();
        }


        public int getScore()
        {
            return score;
        }

        public void moveCell(int i, int j)
        {
            listOfGems.Clear();      

            if ((X == -1) && (Y == -1))
            {
                X = i;
                Y = j;
                activeGem1 = gamefield[i, j].whichGem;
            }
            else
            {
                if (((X == i) && (Math.Abs(Y - j) == 1)) || ((Y == j) && (Math.Abs(X - i) == 1)))
                {
                    activeGem2 = gamefield[i, j].whichGem;

                    gamefield[X, Y].whichGem = activeGem2;
                    gamefield[i, j].whichGem = activeGem1;

                    List<Square> row = Complite();  
                  
                    if (row.Count() != 0)
                    {
                       
                        Complite();
                        switchs = true;
                        leftmoves--;
                    }
                    else
                    {
                       
                        gamefield[X, Y].whichGem = activeGem1;
                        gamefield[i, j].whichGem = activeGem2;
                    }
                   
                }
                else
                {
                    switchs = false;
                }
          
                if (switchs == true)
                {
                    X = -1;
                    Y = -1;

                    activeGem1 = -1;
                    activeGem2 = -1;

                    BeginFalling();
                }
            }
        }
        public List<Square> Complite()
        {
            listOfGems.Clear();
            int count; 
           
            for (int i = 0; i < sizeField; i++)
            {
                int type = gamefield[0, i].whichGem;
                count = 1;
                for (int j = 0; j < sizeField; j++)
                {
                    if ((type == gamefield[i, j].whichGem) && (j != 0))
                        count++;
                    else
                        count = 1;
                 
                    if (count > 2)
                    {
                        listOfGems.Add(gamefield[i, j - 2]);
                        listOfGems.Add(gamefield[i, j - 1]);
                        listOfGems.Add(gamefield[i, j]);
                    }
                    type = gamefield[i, j].whichGem;
                }
            }
       
            for (int j = 0; j < sizeField; j++)
            {
                int type = gamefield[j, 0].whichGem;
                count = 1;
                for (int i = 0; i < sizeField; i++)
                {
                    if ((type == gamefield[i, j].whichGem) && (i != 0))
                        count++;
                    else
                        count = 1;
                    if (count > 2)
                    {
                        listOfGems.Add(gamefield[i - 2, j]);
                        listOfGems.Add(gamefield[i - 1, j]);
                        listOfGems.Add(gamefield[i, j]);
                    }
                    type = gamefield[i, j].whichGem;
                }
            }
            foreach (Square sqq in listOfGems) 
            {
                sqq.whichGem = empty;
            }
            score += listOfGems.Count() * 10;
            return listOfGems;
        }

        public bool EmptyGem()
        {
            for (int j = 0; j < sizeField; j++)
            {
                for (int i = 0; i < sizeField; i++)
                {
                    if (gamefield[i, j].whichGem == empty) return true;
                }
            }
            return false;
        }

        public void Fallings()
        {
            int gem1 = -1;

            for (int j = sizeField - 1; j >= 0; j--)
            {
                for (int i = sizeField - 2; i >= 0; i--)
                {
                    if (gamefield[i + 1, j].whichGem == empty)
                    {
                        gem1 = gamefield[i, j].whichGem;

                        gamefield[i + 1, j].whichGem = gem1;
                        gamefield[i, j].whichGem = empty;
                    }
                }
            }
            for (int j = 0; j < sizeField; j++)
                for (int i = 0; i < sizeField; i++)
                {
                    if (gamefield[i, j].whichGem == empty)
                    {
                        gamefield[i, j].whichGem = rng.Next(0, 5);
                    }
                    break;
                }
        }
    }
}
