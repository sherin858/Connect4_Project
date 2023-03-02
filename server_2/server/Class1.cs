using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    internal class Client
    {
        //TcpClient tcpClient;
        public event EventHandler ReadMsg;
        StreamReader br;
        NetworkStream nstream;
        
        public Client() { }
        public Client(Client client) 
        { 
            this.Name=client.Name;
            this.TClient = client.TClient;
        }
        public Client(TcpClient tcpClient)
        {
            TClient = tcpClient;
            nstream = TClient.GetStream();
            bw = new StreamWriter(nstream);
            bw.Flush();
            br = new StreamReader(nstream);
            ReadInfo(EventArgs.Empty);
        }
        protected virtual async void ReadInfo(EventArgs e)
        {
            Name = await br.ReadLineAsync();
            ReadMsg?.Invoke(this,e);
            while (true)
            {
                Msg = await br.ReadLineAsync();
                ReadMsg?.Invoke(this, e);
            }
           
        }
        public string Name { get; set; }
        public string Msg { get; set; }
        public StreamWriter bw { set; get; }
        public TcpClient TClient { get; set; }

        public Room CurrentRoom { set; get; }
    }
}   

