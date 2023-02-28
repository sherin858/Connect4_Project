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

        public Room(Client roomOwner)
        {
            player1= new Client(roomOwner);
        }

    }
}
