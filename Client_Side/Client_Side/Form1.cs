using System;
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

namespace Client_Side
{
    public partial class Form1 : Form
    {
        TcpClient tcpClient;
        byte[] serverAddress;
        IPAddress ip;
        int port;
        StreamReader br;
        StreamWriter bw;
        NetworkStream nstream;
        String LoginName;
        public Form1()
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
            if(textBox1.Text=="")
            {
                LoginName = "Anonymous Player";
            }
            else
            {LoginName = textBox1.Text;}
            bw = new StreamWriter(nstream);
            bw.WriteLine(LoginName);
            bw.Flush();
            this.Hide();
            Rooms roomsDialog = new Rooms();
            DialogResult dlgResult ;
            dlgResult = roomsDialog.ShowDialog();
            if(dlgResult == DialogResult.OK)
            {
                bw.WriteLine(roomsDialog.RoomChoice);
                bw.Flush();
            }
            //roomsDialouge.FormClosed += Close_All;
            while (true)
            {
                MessageBox.Show(await br.ReadLineAsync());
            }
            
            
        }
        private void Close_All(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
