using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;

namespace DesignHubSite.Models
{

    public class Project
    {

        [Key]
        [Required]
        public int Id { get; set; }


        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        //public string ImageName { get; set; }
        //public byte[] Image { get; set; }


        public virtual Version Root { get; set; }
        public virtual Version Head { get; set; }


        public virtual ICollection<Version> Versions { get; set; } = new List<Version>();




        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<ApplicationUser> Watchers { get; set; } = new List<ApplicationUser>();


        [Timestamp]
        public Byte[] Timestamp { get; set; }


    }

}