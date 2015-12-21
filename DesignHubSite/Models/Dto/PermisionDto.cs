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

    public class PermisionDto
    {

        public string UserId { get; set; }  
        public int? ProjectId { get; set; }


        public string UserName { get; set; }
        public string ProjectRole { get; set; }

        public bool Readonly { get; set; } = false;
        public bool Message { get; set; } = false;
        public bool LikeOrDislikeChanges { get; set; } = false;
        public bool AddMarkers { get; set; } = false;
        public bool AcceptNodes { get; set; } = false;
        public bool AcceptWholeProject { get; set; } = false;


    }

}