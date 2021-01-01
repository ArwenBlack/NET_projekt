using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class EcgDataPoint
    {
        public long Id { get; set; }

        public double Point { get; set; }

        public virtual EcgDataset ConcreteDataset { get; set; }
    }
}