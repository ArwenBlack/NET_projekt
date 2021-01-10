using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace NET_projekt.Models
{
    public class Dataset
    {
        //-----------------------------------------------------------------
        public int Id { get; set; }
        //-----------------------------------------------------------------
        [Required(ErrorMessage = "Nazwa dataset'u jest wymagana")]
        [DisplayName("Nazwa dataset'u")]
        [StringLength(60)]
        public string DatasetName { get; set; }
        //-----------------------------------------------------------------
        [Required]
        [DisplayName("Data dodania")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DateAdded { get; set; }
        //-----------------------------------------------------------------
        [Required(ErrorMessage = "Należy podać informacje o kolumnach")]
        [DisplayName("Informacje o kolumnach")]
        public string DatasetColumnsInfo { get; set; }
        //-----------------------------------------------------------------
        [Required]
        [DisplayName("Częstotliwość")]
        [Range(1, 1000, ErrorMessage="Częstotliwość musi być z zakresu 1-1000 Hz")]
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