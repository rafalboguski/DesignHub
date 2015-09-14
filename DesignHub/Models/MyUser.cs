using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace DesignHub.Models
{
    public class MyUser : IdentityUser
    {

        [JsonIgnore]
        public virtual ICollection<Project> Projects { get; set; } 

    }
}