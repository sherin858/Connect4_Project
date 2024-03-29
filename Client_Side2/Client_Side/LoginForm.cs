﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using static Client_Side.Game;
using System.Data.Common;

namespace Client_Side
{
    public delegate void Notify(string Msg);
    public partial class LoginForm : Form
    {

        TcpClient tcpClient;
        byte[] serverAddress;
        IPAddress ip;
        int port;
        StreamReader br;
        StreamWriter bw;
        NetworkStream nstream;
        string LoginName;
        List<string> availableRoomsId;
        List<string> availableRoomsBoardSize;
        public string Msg { set; get; }
        public Notify ReadMsgg;
        public LoginForm()
        {
            InitializeComponent();


        }

        private async void button1_Click(object sender, EventArgs e)
        {
            serverAddress = new byte[] { 127, 0, 0, 1 };
            ip = new IPAddress(serverAddress);
            port = 5500;
            tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);
            nstream = tcpClient.GetStream();
            br = new StreamReader(nstream);
            availableRoomsId = new List<string>();
            availableRoomsBoardSize = new List<string>();
            if (textBox1.Text == "")
            {
                LoginName = "Anonymous Player";
            }
            else
            { LoginName = textBox1.Text; }
            bw = new StreamWriter(nstream);
            bw.WriteLine(LoginName);
            bw.Flush();
            Rooms roomsDialog = new Rooms();
            string roomsMsg;
            DialogResult dlgResult;
            


            while (true)
            {
                roomsMsg = await br.ReadLineAsync();
                if (roomsMsg == "Rooms End" || roomsMsg == "Rooms Empty") {break;}

                availableRoomsId.Add(roomsMsg.Split(' ')[0]);
                availableRoomsBoardSize.Add(roomsMsg.Split(' ')[1]);
                roomsDialog.SetAvailableRooms(roomsMsg.Split(' ')[0]);
            }
            
                
            dlgResult = roomsDialog.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {

                bw.WriteLine(roomsDialog.RoomChoice);
                bw.Flush();
            }
            // get col,row
            Game game = new Game();
            if (roomsDialog.RoomChoice == "6*7" || roomsDialog.RoomChoice == "8*12")
            {
                string[] col_row = roomsDialog.RoomChoice.Split('*');
                game.row = Int32.Parse(col_row[0]);
                game.col = Int32.Parse(col_row[1]);
                game.initializeColRow();
                game.Show();
                roomsDialog.PlayerColor = await br.ReadLineAsync();
                if (roomsDialog.PlayerColor == "Yellow")
                {
                    game.Mycolor = Color.Yellow;
                    game.MyBrush = new SolidBrush(game.Mycolor);
                    game.MyCompBrush = new SolidBrush(Color.Red);
                }
                else
                {
                    game.Mycolor = Color.Red;
                    game.MyBrush = new SolidBrush(game.Mycolor);
                    game.MyCompBrush = new SolidBrush(Color.Yellow);
                }
            }
            if (roomsDialog.RoomChoice.Contains("id"))
            {
                string roomSize = availableRoomsBoardSize[int.Parse(roomsDialog.RoomChoice.Split(' ')[1]) - 1];//Split(' ')[0]
                game.row = Int32.Parse(roomSize.Split('*')[0]);
                game.col = Int32.Parse(roomSize.Split('*')[1]);
                game.initializeColRow();
                if (roomsDialog.PlayerColor == "Yellow")
                {
                    game.Mycolor = Color.Yellow;
                    game.MyBrush = new SolidBrush(game.Mycolor);
                    game.MyCompBrush = new SolidBrush(Color.Red);
                    bw.WriteLine(roomsDialog.PlayerColor);
                    bw.Flush();
                }
                else
                {
                    game.Mycolor = Color.Red;
                    game.MyBrush = new SolidBrush(game.Mycolor);
                    game.MyCompBrush = new SolidBrush(Color.Yellow);
                    bw.WriteLine(roomsDialog.PlayerColor);
                    bw.Flush();
                }
                game.Show();
            }
            game.SendGameMsgs += SendLastMove;
            
            this.ReadMsgg += new Notify(game.ReadInfo);
            while (true)
            {

                Msg = await br.ReadLineAsync();
                game.Dimmed = false;
                ReadMsgg(Msg);
            }
        }
        
        public void SendLastMove(object sender, EventData e)
        {
            if (e.GameEnd == "GameEnd")
            {
                bw.WriteLine(e.GameEnd);
                bw.Flush();
            }
            if (e.GameEnd == "Red Player Won" || e.GameEnd == "Yellow Player Won")
            {
                bw.WriteLine(e.GameEnd);
                bw.Flush();
            }
            if (e.GameEnd == "PlayAgain")
            {
                bw.WriteLine(e.GameEnd);
                bw.Flush();
            }
            else
            {
                bw.WriteLine(e.columnPlayed.ToString());
                bw.Flush();
            }
        }
        


    }
}
