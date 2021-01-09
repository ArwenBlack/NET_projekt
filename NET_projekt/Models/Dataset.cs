using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class Dataset
    {
        //-----------------------------------------------------------------
        public int Id { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public string DatasetName { get; set; }
        //-----------------------------------------------------------------
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DateAdded { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public string DatasetColumnsInfo { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public int DatasetHzFrequency { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public string Reference { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public virtual User ConcreteUser { get; set; }
        //-----------------------------------------------------------------
    }
}