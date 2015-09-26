using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
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


        //[Required]
        //public virtual string OwnerId { get; set; }

        //public string OwnerId { get; set; }
        //public string OwnerName { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser Owner { get; set; }

        [JsonIgnore]
        public virtual ICollection<ApplicationUser> Watchers { get; set; } = new List<ApplicationUser>();


    }

}