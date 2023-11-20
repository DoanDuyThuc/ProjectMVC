

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMVC.Models.Contacts {
    public class Contact
    {
        [Key]
        public int Id {set;get;}

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        [Required]
        public string FullName {set;get;}

        [Required]
        [StringLength(100)]
        public string Email {set;get;}


        public DateTime DataSent{set;get;}

        public string Message {set;get;}

        [StringLength(50)]
        public string phone{set;get;}
        
    }
}