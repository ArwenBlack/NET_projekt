using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class EmgDataPoint
    {
        public long Id { get; set; }

        public double Point { get; set; }

        public virtual EmgDataset ConcreteDataset { get; set; }
    }
}