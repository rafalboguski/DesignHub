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
    /*
        List of permisions that single user has to single project

    */

    public class Permision
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public virtual Project Project { get; set; }

        public string ProjectRole { get; set; }

        // Permitions                                                                                
        public bool Readonly { get; set; }
        public bool Message { get; set; }
        public bool LikeOrDislikeChanges { get; set; }
        public bool AddMarkers { get; set; }
        public bool AcceptNodes { get; set; }
        public bool AcceptWholeProject { get; set; }



        public DateTime Timestamp { get; set; }


    }

}