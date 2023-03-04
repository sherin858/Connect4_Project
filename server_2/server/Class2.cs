using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Room
    {
        Client player1;
        //Client player2;
        //string player1_Color;
        //string player2_Color;
        public List<Client> viewers;
        public List<int> FirstPlayerColumns;
        public List<int> SecondPlayerColumns;
        String boardsize;
        bool full;
        public Room(Client roomOwner)
        {
            player1= new Client(roomOwner);
            boardsize = roomOwner.Msg;
            full = false;
        }
        public int ID { set;get;}
        public string BoardSize { set; get;}
        public Client Player2 { set; get; }
        public Client Player1 { set; get; }
        public String Player1_Color { set; get; }
        public String Player2_Color { set; get;}
      
    }
}
