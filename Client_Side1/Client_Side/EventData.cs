﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Side
{
    public class EventData :EventArgs
    {
        public int columnPlayed { get; set; }
        public string GameEnd { get; set; }
    }
}
