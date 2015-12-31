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


    public class Node
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string ChangeInfo { get; set; }


        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }


        public virtual Project Project { get; set; }

        public bool Root { get; set; }

        public bool Head { get; set; }

        public virtual ICollection<Node> Parents { get; set; } = new List<Node>();
        public virtual ICollection<Node> Childrens { get; set; } = new List<Node>();

        public ICollection<Marker> ImageMarkers { get; set; } = new List<Marker>();

        public bool Accepted { get; set; }
        public ApplicationUser whoAccepted { get; set; }
        public bool Rejected { get; set; }
        public ApplicationUser whoRejected { get; set; }

        public ICollection<ApplicationUser> Likes { get; set; } = new HashSet<ApplicationUser>();
        public ICollection<ApplicationUser> Dislikes { get; set; } = new HashSet<ApplicationUser>();


        // graph data

        public int positionX { get; set; } = 100;
        public int positionY { get; set; } = 30;



    }

}