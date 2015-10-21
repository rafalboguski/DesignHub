

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DesignHubSite.Models
{

    [JsonObject(MemberSerialization.OptIn)]
    public class ApplicationUser : IdentityUser
    {

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<Project> WatchedProjects { get; set; } = new List<Project>();



        #region ----Json Mapped Properties-----

        [NotMapped]
        [JsonProperty]
        public string Number => Id;

        [NotMapped]
        [JsonProperty]
        public string Name => UserName;

        //todo: later exclude from serializing
  //      [NotMapped]
  //      [JsonProperty]
  //      public string OwnedProjects
  //      {
  //          get
  //          {
  //              var json = Json.Encode(
  //                  Projects.Select(x => new ProjectViewModel { Id = x.Id, Name = x.Name }).ToList()
  //                  );
  //              json = json.Replace("\"", "\'");
  //              json = json.Replace(@"\", "");
  //              return json;
  //          }
  //      }

        #endregion

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        private class ProjectViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}