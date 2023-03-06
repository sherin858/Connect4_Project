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
                Room NewRoom = new Room((Client)sender);
                NewRoom.ID = availableRooms.Count + 1;
                availableRooms.Add(NewRoom);
                NewRoom.BoardSize = Msg;
                ((Client)sender).CurrentRoom = NewRoom;
            }
            else if (Msg.Contains("id") == true)
            {
                string roomId = Msg.Split(' ')[1];
                foreach (Room room in availableRooms)
                {
                    if (room.ID.ToString() == roomId)
                    {
                        room.Player2 = (Client)sender;
                    }
                }
            }

            else if (Msg == "Red" || Msg == "Yellow")
            {
                foreach (Room room in availableRooms)
                {
                    if (room.Player2.Nstream == ((Client)sender).Nstream)
                    {
                        room.Player2_Color = Msg;
                        room.Player1_Color = Msg == "Yellow" ? "Red" : "Yellow";
                        ((Client)sender).CurrentRoom = room;
                        room.Player1.bw.WriteLine(room.Player1_Color);
                        room.Player1.bw.Flush();
                    }
                }
            }

            else if (int.Parse(Msg) >= 1 && int.Parse(Msg) <= 12)
            {

                foreach (Room room in availableRooms)
                {
                    if (room.Player1.Nstream == ((Client)sender).Nstream)
                    {
                        room.Player2.bw.WriteLine(Msg);
                        room.Player2.bw.Flush();
                        //room.FirstPlayerColumns.Add(ColNumber);
                        //room.Player2.bw.WriteLine(ColNumber);
                        //room.Player2.bw.Flush();
                    }
                    else if(room.Player2.Nstream == ((Client)sender).Nstream)
                    {
                        room.Player1.bw.WriteLine(Msg);
                        room.Player1.bw.Flush();
                        //room.SecondPlayerColumns.Add(ColNumber);
                        //room.Player1.bw.WriteLine(ColNumber);
                        //room.Player1.bw.Flush();
                    }
                }
            }



            else if (Msg == "GameEnd")
            {
                foreach (Room room in availableRooms)
                {
                    DateTime dateTime = DateTime.Now;
                    if (room.Player1 == ((Client)sender))
                    {
                        File.WriteAllText("D:\\Desktop\\"+dateTime+".txt", "First Player: "+room.Player1.Name+"is Winner, Second Player: "+ room.Player2.Name+" ,Date: "+dateTime);
                    }
                    else
                    {
                        File.WriteAllText("D:\\Desktop\\" + dateTime + ".txt", "First Player: " + room.Player2.Name + "is Winner, Second Player: " + room.Player1.Name + " ,Date: " + dateTime);
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
                        //NewClient.bw.WriteLine(room.ID.ToString()+" "+ room.BoardSize.ToString());

                        NewClient.bw.WriteLine($"{room.ID} {room.BoardSize}");
                        //NewClient.bw.WriteLine(room.ID.ToString());
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
