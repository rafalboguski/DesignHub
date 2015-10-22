using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace DesignHubSite.Models
{


    public class Version
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string ChangeInfo { get; set; }


        public byte[] Image { get; set; }





    }

}