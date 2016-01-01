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


    public class MarkerDto
    {


        public int? Id { get; set; } = null;

        [Range(0, 1)]
        public float X { get; set; } = 0.5f;

        [Range(0, 1)]
        public float Y { get; set; } = 0.5f;

        public int Width { get; set; } = 50;
        public int Height { get; set; } = 50;

        public string Text { get; set; }

        public DateTime Timestamp { get; set; } 


        public int NodeId { get; set; }



    }

}