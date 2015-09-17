using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace DesignHubSite.Models
{


    public class Project
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }

}