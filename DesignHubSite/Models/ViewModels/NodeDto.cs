using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace DesignHubSite.Models
{


    public class NodeDTO
    {



        public string Id { get; set; }

        public string ChangeInfo { get; set; }
        public byte[] Image { get; set; }


        public int? ProjectId { get; set; }

        public bool? Root { get; set; }

        public bool? Head { get; set; }

        public virtual int? ParentId { get; set; }
        public virtual ICollection<int> ChildrensId { get; set; } = new List<int>();


        // graph data

        public int positionX { get; set; } = 0;
        public int positionY { get; set; } = 0;



    }

}