using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Client_Side
{
    public partial class Game : Form
    {
        public event Action<object, EventData> SendGameMsgs;
        String WinnerOrLoser;

        //draw rectangle
        Color RectColor;
        Rectangle Table;
        Point TableUPoint;
        int TableXSpace = 70;
        int TableYSpace = 70;
        int startX = 50;
        int startY = 50;
        Brush RctBrush;
        // draw circle
        Color CircleColor;
        Pen CirclePen;
        Brush CircleBrush;

        // lists
        List<Color>[] disk;
        Color[,] whiteDisks;
        public Boolean Dimmed { set; get; }
        public Color Mycolor {set; get;}
        public Brush MyBrush {set; get;}
        public Brush MyCompBrush { set; get;}

        public Game()
        {
            InitializeComponent();
            // draw rectangle
            RectColor = Color.Blue;
            RctBrush = new SolidBrush(RectColor);
            TableUPoint = new Point(startX, startY);
            // draw circle
            CircleColor = Color.White;
            CirclePen = new Pen(CircleColor, 5);
            CircleBrush = new SolidBrush(CircleColor);
            Dimmed = false;
            
           
        }
        public void initializeColRow()
        {
            whiteDisks = new Color[row, col];
            disk = new List<Color>[col];
            Table = new Rectangle(TableUPoint, new Size(col * TableXSpace, row * TableYSpace));
            for (int i = 0; i < col; i++)
            {
                disk[i] = new List<Color>();
            }
        }
        private void Game_MouseClick(object sender, MouseEventArgs e)
        {



            // to know which column will be fulled with a disk
            for (int i = 0; i < col; i++)
            {
                if (e.X > (startX + i * TableXSpace) && e.X < (startX + (i + 1) * TableXSpace))
                {
                    Graphics g = this.CreateGraphics();
                    if (disk[i].Count < row && !Dimmed)
                    {
                        disk[i].Add(Mycolor);
                        g.FillEllipse(MyBrush, startX + TableXSpace * i + 5, startY + TableYSpace * (row - disk[i].Count) + 5, 50, 50);
                        whiteDisks[(row - disk[i].Count), i] = Mycolor;

                    }
                    ColumnUpdated(i);
                }
            }

            checkWinner();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            drawRect();
            DrawCircle();
        }
        public void drawRect()
        {
            Graphics g = this.CreateGraphics();
            g.FillRectangle(RctBrush, Table);

        }
        public void DrawCircle()
        {
            Graphics g = this.CreateGraphics();

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {

                    g.FillEllipse(CircleBrush, startX + j * TableXSpace + 5, startY + TableYSpace * i + 5, 50, 50);
                    whiteDisks[i, j] = Color.White;

                }

            }
        }

        public void checkWinner()
        {
            PlayAgain playAgain = new PlayAgain();
            DialogResult dlgResult;
            
            {
                // horizontal
                int countRed = 0;
                int countYellow = 0;
                for (int i = row - 1; i > 0; i--)//row
                {
                    for (int j = 0; j < col - 1; j++)//col
                    {

                        if (whiteDisks[i, j] == Color.Yellow && whiteDisks[i, j + 1] == Color.Yellow)
                        {
                            countYellow++;

                            if (countYellow == 3)
                            {
                                break;
                            }
                        }
                        else if (whiteDisks[i, j] == Color.Red && whiteDisks[i, j + 1] == Color.Red)
                        {
                            countRed++;
                            if (countRed == 3)
                            {
                                break;
                            }
                        }
                        else
                        {
                            countRed = 0;
                            countYellow = 0;
                        }

                    }
                    if (countYellow == 3)
                    {
                        playAgain.label_4 = "Yellow Player Won";
                        
                    }
                    else if (countRed == 3)
                    {
                        playAgain.label_4 = "Red Player Won";
                        
                    }

                  
                }

                // check for vertical

                for (int j = 0; j < col; j++)// eight column
                {
                    for (int i = row - 1; i > 0; i--)// seven row , const column, varried row
                    {

                        if (whiteDisks[i, j] == Color.Yellow && whiteDisks[i - 1, j] == Color.Yellow)
                        {
                            countYellow++;

                            if (countYellow == 3)
                            {
                                break;
                            }
                        }
                        else if (whiteDisks[i, j] == Color.Red && whiteDisks[i - 1, j] == Color.Red)
                        {
                            countRed++;
                            if (countRed == 3)
                            {
                                break;
                            }
                        }
                        else
                        {
                            countRed = 0;
                            countYellow = 0;
                        }

                    }
                    if (countYellow == 3)
                    {
                        playAgain.label_4 = "Yellow Player Won";
                    }
                    else if (countRed == 3)
                    {
                        playAgain.label_4 = "Red Player Won";
                    }

                }

               
                // check for / diagonal
                for (int i = 0; i < row - 3; i++)// eight column
                {
                    for (int j = 3; j < col; j++)// seven row , const column, varried row
                    {

                        Color start = whiteDisks[i, j];
                        if (
                            start == whiteDisks[i + 1, j - 1] &&
                             start == whiteDisks[i + 2, j - 2] &&
                             start == whiteDisks[i + 3, j - 3])
                        {
                            if (start == Color.Yellow)
                            {
                                playAgain.label_4 = "Yellow Player Won";
                                
                            }
                            else if (start == Color.Red)
                            {
                                playAgain.label_4 = "Red Player Won";
                            }
                        }
                    }
                }

                // check for \ diagonal
                for (int i = 0; i < row - 3; i++)
                {
                    for (int j = 0; j < col - 3; j++)
                    {
                        Color element = whiteDisks[i, j];
                        if (element == whiteDisks[i + 1, j + 1] &&
                            element == whiteDisks[i + 2, j + 2] &&
                            element == whiteDisks[i + 3, j + 3])
                        {
                            if (element == Color.Yellow)
                            {
                                playAgain.label_4 = "Yellow Player Won";
                            }
                            else if (element == Color.Red)
                            {
                                playAgain.label_4 = "Red Player Won";
                            }
                        }
                    }
   
                }
                if (playAgain.label_4 == "Red Player Won" || playAgain.label_4 == "Yellow Player Won")
                {
                    GameChoices(playAgain.label_4);
                    if (WinnerOrLoser == "Winner")
                    {
                        playAgain.label_4 = "Congratulations";
                    }
                    else { playAgain.label_4="Good Luck Next Time";}
                    dlgResult = playAgain.ShowDialog();
                    if (dlgResult == DialogResult.OK)
                    {
                        clearGame();
                        Invalidate();
                        playAgain.label_4 = "";
                        GameChoices("PlayAgain");
                    }
                    else
                    {
                        this.Close();
                        GameChoices("GameEnd");     
                    }
                }


            }
        }
        public void clearGame()
        {
            // fill the two d array with white colors
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    whiteDisks[i, j] = Color.White;
                }
            }
            // free all lists from the colored disks
            for (int j = 0; j < col; j++)
            {
                disk[j].Clear();

            }
        }
        protected virtual void OnSendGameMsgs(EventData e)
        {
            SendGameMsgs?.Invoke(this, e);
        }

        public void ColumnUpdated(int column)
        {
            if (Dimmed == false)
            {
                OnSendGameMsgs(new EventData { columnPlayed = column });
            }
            Dimmed = true;
        }

        public void GameChoices(string choice)
        {   
                OnSendGameMsgs(new EventData { GameEnd=choice });     
        }

        public void ReadInfo(string Msg)
        {
            
            if (Msg== "Winner"|| Msg=="Loser")
            {
                WinnerOrLoser = Msg;
            }
            else
            {
                int col = int.Parse(Msg);
                Graphics g = this.CreateGraphics();
                Color compColor = Mycolor == Color.Red ? Color.Yellow : Color.Red;
                disk[col].Add(compColor);
                g.FillEllipse(MyCompBrush, startX + TableXSpace * col + 5, startY + TableYSpace * (row - disk[col].Count) + 5, 50, 50);
                whiteDisks[(row - disk[col].Count), col] = compColor;
                checkWinner();
            }

        }
        public int row { get; set; }
        public int col { get; set; }
    }

}
