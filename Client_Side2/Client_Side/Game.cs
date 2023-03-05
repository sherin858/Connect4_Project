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

        int playedColumn;
        //public event Action<object, EventData> ColumnChanged;
        //public delegate void ColumnChangedHandler(object source, EventData e);
        //public event ColumnChangedHandler ColumnChanged;


        //draw rectangle
        Color RectColor;
        Rectangle Table;
        Point TableUPoint;
        int TableXSpace = 70;
        int TableYSpace = 70;
        int startX = 50;
        int startY = 50;
        //int row = 7;
        //int col = 8;
        Brush RctBrush;
        // draw circle
        Color CircleColor;
        Pen CirclePen;
        Brush CircleBrush;
        Color playerOneColor;
        Color playerTwoColor;
        Brush playerOneBrush;
        Brush playerTwoBrush;
        // lists
        List<Color>[] disk;
        Boolean playerOne = true;
        Color[,] whiteDisks;

        public Game()
        {
            InitializeComponent();
            //this.col = col;
            //this.row = row;
            //whiteDisks = new Color[row, col];
            //disk = new List<Color>[col];
            // draw rectangle
            RectColor = Color.Blue;
            RctBrush = new SolidBrush(RectColor);
            TableUPoint = new Point(startX, startY);
            //Table = new Rectangle(TableUPoint, new Size(col * TableXSpace, row * TableYSpace));
            // draw circle
            CircleColor = Color.White;
            CirclePen = new Pen(CircleColor, 5);
            CircleBrush = new SolidBrush(CircleColor);
            //playerOneColor = Color.Red;
            //playerTwoColor = Color.Yellow;
            //playerOneBrush = new SolidBrush(playerOneColor);

            //for (int i = 0; i < col; i++)
            //{
            //    disk[i] = new List<Color>();
            //}

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
        public void initializeColor()
        {
            playerTwoColor = playerTwo;
            playerOneColor = playerTwoColor == Color.Yellow ? Color.Red : Color.Yellow;
            playerOneBrush = new SolidBrush(playerOneColor);
            playerTwoBrush = new SolidBrush(playerTwoColor);
        }

     

        private void Game_MouseClick_1(object sender, MouseEventArgs e)
        {
            //  GenerateColumn(MousePosition);

            // to know which column will be fulled with a disk
            for (int i = 0; i < col; i++)
            {
                if (e.X > (startX + i * TableXSpace) && e.X < (startX + (i + 1) * TableXSpace))
                {
                    MessageBox.Show("player one");
                    Graphics g = this.CreateGraphics();
                    if (disk[i].Count < row && playerOne == true)
                    {
                        disk[i].Add(Color.Red);
                        g.FillEllipse(playerOneBrush, startX + TableXSpace * i + 5, startY + TableYSpace * (row - disk[i].Count) + 5, 50, 50);
                        playerOne = false;
                        whiteDisks[(row - disk[i].Count), i] = Color.Red;

                    }
                    else if (disk[i].Count < row && playerOne == false)
                    {
                        MessageBox.Show("player one");
                        disk[i].Add(Color.Yellow);
                        g.FillEllipse(playerTwoBrush, startX + TableXSpace * i + 5, startY + TableYSpace * (row - disk[i].Count) + 5, 50, 50);
                        playerOne = true;
                        whiteDisks[(row - disk[i].Count), i] = Color.Yellow;
                    }
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
                        MessageBox.Show("yellow player win");
                        clearGame();
                        Invalidate();
                        break;
                    }
                    else if (countRed == 3)
                    {
                        MessageBox.Show("red player win");
                        clearGame();
                        Invalidate();
                        break;
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
                        MessageBox.Show("yellow player win");
                        clearGame();
                        Invalidate();
                        break;
                    }
                    else if (countRed == 3)
                    {
                        MessageBox.Show("red player win");
                        clearGame();
                        Invalidate();
                        break;
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
                                MessageBox.Show("yellow won");
                                clearGame();
                                Invalidate();
                                break;
                            }
                            else if (start == Color.Red)
                            {
                                MessageBox.Show("red won");
                                clearGame();
                                Invalidate();
                                break;
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
                                MessageBox.Show("yellow won");
                                clearGame();
                                Invalidate();
                                break;
                            }
                            else if (element == Color.Red)
                            {
                                MessageBox.Show("red won");
                                clearGame();
                                Invalidate();
                                break;
                            }
                        }

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

        //public int GenerateColumn(Point point)
        //{
        //    point = new Point(point.X, point.Y);
        //    if (point.X > Table.X && point.X < Table.Width + Table.X && point.Y > Table.Y && point.Y < Table.Height + Table.X)
        //    {
        //        int ColWidth = Table.Width / col;
        //        int colsPointsMin = 0;
        //        int colsPointsMax = ColWidth;
        //        for (int i = 0; i < col; i++)
        //        {
        //            if (colsPointsMin <= point.X && point.X < colsPointsMax)
        //            {
        //                ColumnPlayed = i + 1;
        //                colsPointsMin = colsPointsMin + ColWidth;
        //                colsPointsMax = colsPointsMax + ColWidth;
        //            }
        //        }
        //    }
        //    return ColumnPlayed;
        //}

        ////public int ColumnPlayed
        ////{
        ////    set
        ////    {
        ////        playedColumn = value;
        ////        OnColumnChanged(new EventData { columnPlayed = playedColumn });
        ////    }

        ////    get { return playedColumn; }
        ////}
        //protected virtual void OnColumnChanged(EventData e)
        //{
        //    ColumnChanged?.Invoke(this, e);
        //}


        public int row { get; set; }
        public int col { get; set; }
        public Color playerTwo { get; set; }

       
    }

}
