using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class EmgDataset
    {
        public int Id { get; set; }

        public string DatasetName { get; set; }

        public virtual User ConcreteUser { get; set; }

        public virtual ICollection<EmgDataPoint> EmgPointsCollection { get; set; }
    }
}