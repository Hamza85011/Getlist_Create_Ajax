using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace hamzacrud.Models
{
    public class UserModel
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string Last_Name { get; set; }
        [DisplayName("User Age")]
        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
    }
}
