﻿using System.Collections.Generic;
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

        [MaxLength(4000)]
        public string Description { get; set; }

        //public string ImageName { get; set; }
        //public byte[] Image { get; set; }


        public bool Accepted { get; set; } = false;
        public ApplicationUser WhoAccepted { get; set; }
        public bool Rejected { get; set; } = false;
        public ApplicationUser WhoRejected { get; set; }


        public int DurationDays { get; set; }


        public virtual ICollection<Node> Nodes { get; set; } = new List<Node>();


        public int? nodeHeadId { get; set; } = null;

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<ApplicationUser> AssignedUsers { get; set; } = new List<ApplicationUser>();

                
        public DateTime CreateDate { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? EndDate { get; set; } = DateTime.Now;

    }

    public class ProjectNote
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string content { get; set; }
    }

}