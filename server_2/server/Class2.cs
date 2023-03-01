using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Room
    {
        int id;
        Client player1;
        Client player2;
        List<Client> viewers;
        String boardsize;

        public Room(Client roomOwner)
        {
            player1= new Client(roomOwner);
        }
        public int ID { set;get;}
        public string BoardSize { set; get;}

    }
}
