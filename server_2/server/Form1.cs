using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{   
    public partial class Form1 : Form
    {
        public delegate void ValueHandler(string msg);
        TcpListener tcpListener;
        byte[] bt;
        IPAddress ip;
        int port;
        List<Client> clients;
        List<Room> availableRooms;
        Boolean CreateRoom;
        public Form1()
        {
            InitializeComponent();
            bt = new byte[] { 127, 0, 0, 1 };
            ip = new IPAddress(bt);
            port = 5500;
            tcpListener = new TcpListener(ip, port);
            clients = new List<Client>();
            availableRooms = new List<Room>();
            tcpListener.Start();
            StartConnection();
            
            
        }
        public void ReadInfo(object sender, EventArgs e)
        {
            string Msg = ((Client)sender).Msg;
            if (Msg == "Name")
            {
                clients.Add((Client)sender);
                listBox1.Items.Add(((Client)sender).Name + " " + ((Client)sender).TClient.Client.RemoteEndPoint.ToString());
            }

            //Creates new room by getting the board size
            else if (Msg == "6*7" || Msg == "8*12")
            {
                MessageBox.Show("Test");
                Room NewRoom = new Room((Client)sender);
                NewRoom.ID = availableRooms.Count + 1;
                availableRooms.Add(NewRoom);
                ((Client)sender).CurrentRoom= NewRoom;
            }
            else if (Msg.Contains("id") == true)
            {
                string roomId = Msg.Split(' ')[1];
                foreach(Room room in availableRooms)
                {
                    if (room.ID.ToString() == roomId)
                    {
                        room.Player2 = (Client)sender;
                    }
                }
            }

            //the second player Joins
            //recieves the selected room id and checks if it's a number
            //else if ( int.TryParse(((Client)sender).Msg,out int Result)==true)
            //{

            //}
            else if (Msg == "Red" || Msg == "Yellow")
            {
                foreach (Room room in availableRooms)
                {
                    if (room.Player2 == (Client)sender)
                    {
                        room.Player2_Color = Msg;
                        room.Player1_Color = Msg=="Yellow"?"Yellow":Msg;
                        ((Client)sender).CurrentRoom= room;
                    }
                }
            }
        }
        public async void StartConnection()
        {

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                Client NewClient = new Client(tcpClient);
                NewClient.Msg = "Name";
                NewClient.ReadMsg += ReadInfo;
                if (availableRooms.Count > 0)
                {
                    foreach (Room room in availableRooms)
                    {
                        NewClient.bw.WriteLine(room.ID.ToString());
                        NewClient.bw.Flush();

                    }
                    NewClient.bw.WriteLine("Rooms End");
                    NewClient.bw.Flush();
                }
                else
                {
                    NewClient.bw.WriteLine("Rooms Empty");
                    NewClient.bw.Flush();
                }
                
            }
        }
    }



}
