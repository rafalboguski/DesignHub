using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace DesignHubSite.Models
{


    public class Project
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageName { get; set; }
        public byte[] Image { get; set; }

 
        [Required]
        public virtual string OwnerId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser Owner { get; set; } = null;

        
    }

}