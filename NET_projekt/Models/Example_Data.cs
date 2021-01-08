using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class Example_Data
    {
        public bool ecg  { get; set; }
        

        public bool emg { get; set; }
        

        public int choose_Hz { get; set; }
        public int time { get; set; }

        public int start_time { get; set; }
        public int wyw { get; set;  }
        public List<string> data_list { get; set; }

    }
   
    
  
}