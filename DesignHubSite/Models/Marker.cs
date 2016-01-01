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


    public class Marker
    {

        [Required]
        public int Id { get; set; }

        [Range(0, 1)]
        public float X { get; set; } = 0.5f;

        [Range(0, 1)]
        public float Y { get; set; } = 0.5f;

        public int Width { get; set; } = 50;
        public int Height { get; set; } = 50;


        public int Number { get; set; }

        public ICollection<MarkerOpinion> Opinions { get; set; } = new List<MarkerOpinion>();

        public Node Node { get; set; }



    }

    public class MarkerOpinion
    {

        [Required]
        public int Id { get; set; }
      
        public Marker Marker { get; set; }

       
        public ApplicationUser Author { get; set; }
        public string Opinion { get; set; }

        public DateTime Timestamp { get; set; }

    }

}