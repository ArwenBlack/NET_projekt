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
        [Required(ErrorMessage = "Te pole jest wymagane!")]
        [DisplayName("Nazwa użytkownika")]
        [StringLength(50,  MinimumLength = 4, ErrorMessage = "Nazwa użytkownika musi zawierać od 4 do 50 liter")]
        [RegularExpression(@"[A-Za-z0-9]*", ErrorMessage = "Nazwa użytkownika może składać się wyłącznie z cyfr i liter")]
        public string Nickname { get; set; }
        //-----------------------------------------------------------------
        [Required(ErrorMessage = "Te pole jest wymagane!")]
        [DisplayName("Adres e-mail")]
        [EmailAddress(ErrorMessage = "Proszę podać poprawny adres e-mail")]
        public string EmailAddress { get; set; }
        //-----------------------------------------------------------------
        [Required]
        [DisplayName("Hasło")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Hasło musi składać się z minimum 8 znaków")]
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