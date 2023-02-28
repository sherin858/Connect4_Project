using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            clients.ForEach(client => { listBox1.Items.Add(client.Name); });
            
        }
        public void ReadInfo(object sender, EventArgs e)
        {
            if (((Client)sender).Msg == "Name")
            {
                clients.Add((Client)sender);
                listBox1.Items.Add(((Client)sender).Name + " " + ((Client)sender).TClient.Client.RemoteEndPoint.ToString());
            }
            else if(((Client)sender).Msg == "New Room")
            {
               availableRooms.Add(new Room((Client)sender));
            }
        }
        public async void StartConnection()
        {

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                Client NewClient = new Client(tcpClient);
                NewClient.Msg = "Name";
                foreach(Client cl in clients)
                {
                    cl.bw.WriteLine(cl.Name);
                };
                NewClient.ReadMsg += ReadInfo;
            }
        }
    }



}
