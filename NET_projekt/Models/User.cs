using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Nickname { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public bool PremiumStatus { get; set; } = false;

        public virtual ICollection<EcgDataset> EcgCollection { get; set; }

        public virtual ICollection<EmgDataset> EmgCollection { get; set; }
    }
}