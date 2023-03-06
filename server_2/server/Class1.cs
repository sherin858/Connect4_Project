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
        public event EventHandler ReadMsg;
        StreamReader br;
        
        public Client() { }
        public Client(Client client) 
        { 
            this.Name=client.Name;
            this.TClient = client.TClient;
            this.br=client.br;
            this.Nstream = client.Nstream;
            this.bw= client.bw;
            this.CurrentRoom= client.CurrentRoom;
            this.Msg = client.Msg;
        }
        public Client(TcpClient tcpClient)
        {
            TClient = tcpClient;
            Nstream = TClient.GetStream();
            bw = new StreamWriter(Nstream);
            bw.Flush();
            br = new StreamReader(Nstream);
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
        public NetworkStream Nstream { get; set; }
    }
}   

