using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace DesignHubSite.Models
{


    public class Node
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string ChangeInfo { get; set; }


        [JsonIgnore]
        public byte[] Image { get; set; }

       
        public virtual Project Project { get; set; }



        public virtual Node Father { get; set; } 
        public virtual ICollection<Node> Childs { get; set; } 
            

        public Node()
        {
            Childs = new List<Node>();
        }



    }

}