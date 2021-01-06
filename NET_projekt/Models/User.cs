using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class User
    {
        //-----------------------------------------------------------------
        [Key]
        public int UserId { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public string Nickname { get; set; }
        //-----------------------------------------------------------------
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public string Password { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public string Salt { get; set; }
        //-----------------------------------------------------------------
        [Required]
        
        public bool PremiumStatus { get; set; } = false;
        //-----------------------------------------------------------------
        public virtual ICollection<Dataset> Datasets { get; set; }
        //-----------------------------------------------------------------
        /*[NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }*/
        //-----------------------------------------------------------------
    }
}