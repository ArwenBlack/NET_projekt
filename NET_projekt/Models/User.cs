using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Nazwa użytkownika")]
        public string Nickname { get; set; }
        //-----------------------------------------------------------------
        [Required]
        [DisplayName("Adres e-mail")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        //-----------------------------------------------------------------
        [Required]
        [DisplayName("Hasło")]
        public string Password { get; set; }
        //-----------------------------------------------------------------
        [Required]
        public string Salt { get; set; }
        //-----------------------------------------------------------------
        [Required]
        [DisplayName("Status premium")]
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