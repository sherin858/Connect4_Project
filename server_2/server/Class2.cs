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
        Client player2;
        List<Client> viewers;
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

    }
}
