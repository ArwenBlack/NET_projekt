using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class EmgDataset
    {
        //-----------------------------------------------------------------
        public int Id { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public string DatasetName { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public string GoogleReference { get; set; }
        //-----------------------------------------------------------------
        public virtual User ConcreteUser { get; set; }
        //-----------------------------------------------------------------
    }
}