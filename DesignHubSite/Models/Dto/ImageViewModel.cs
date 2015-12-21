using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace DesignHubSite.Models
{


    public class ImageViewModel
    {
       

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int ProjectId { get; set; }



    }

}