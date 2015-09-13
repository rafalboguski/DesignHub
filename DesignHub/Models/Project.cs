using System.ComponentModel.DataAnnotations;

namespace DesignHub.Models
{
    public class Project 
    {

       
        public int Id { get; set; }

        [Required]
        [Compare("Name", ErrorMessage = "Project with such name already exists.")]
        public string Name { get; set; }


//        public UserModel Owner { get; set; }

        public string Description { get; set; }
       
    }
}