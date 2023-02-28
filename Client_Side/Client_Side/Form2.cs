using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Side
{
    public partial class Rooms : Form
    {

        public Rooms()
        {
            InitializeComponent();
        }
        public string RoomChoice { get; set; }
        private void button2_Click(object sender, EventArgs e)
        {

            //RoomChoice = "Join Room";
            //this.DialogResult = DialogResult.OK;
            //this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RoomChoice = "New Room";
            this.DialogResult= DialogResult.OK;
            this.Close();
        }
    }
}
