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
    public partial class BoardSize : Form
    {
        public BoardSize()
        {
            InitializeComponent();  
        }
        public string Board_size
        {
            get
            {
                if (radioButton2.Checked)
                {
                    return radioButton2.Text;
                }
                else
                {
                    return radioButton1.Text;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
