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


    public class Notification
    {

        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int Priority { get; set; }
        public bool visited { get; set; } = false;
        public string Icon { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime CreateDate {get; set; }

    }

}