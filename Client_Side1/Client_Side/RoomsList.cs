﻿using System;
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
        public string PlayerColor { get; set; }
        
        public void SetAvailableRooms(string roomId)
        {
            listBox1.Items.Add(roomId);
        }
        //join
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                RoomChoice = "id " + listBox1.SelectedItem.ToString();
                JoiningDialog joiningDialog = new JoiningDialog();
                DialogResult dlgResult;
                dlgResult = joiningDialog.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {
                    PlayerColor = joiningDialog.Player_Color;
                    joiningDialog.Close();
                }


                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch { MessageBox.Show("No Room Is Selected"); }

        }

        // create
        private void button1_Click(object sender, EventArgs e)
        {
            BoardSize BoardSizeDialog= new BoardSize();
            DialogResult dlgResult;
            dlgResult = BoardSizeDialog.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                RoomChoice=BoardSizeDialog.Board_size;
                BoardSizeDialog.Close();
            }
            
            this.DialogResult = DialogResult.OK;

            this.Close();
        }
    }
}
