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

    public class ProjectListViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public byte[] HeadImage { get; set; }

        public int NodesNumber { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public int WatchersNumber { get; set; }

        public Byte[] Timestamp { get; set; }



        public static List<ProjectListViewModel> Map(IEnumerable<Project> projects)
        {
            var projectsList = new List<ProjectListViewModel>();

            foreach (var p in projects)
            {
                projectsList.Add(new ProjectListViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    //HeadImage = p.Head?.Image,
                    Owner = p.Owner,
                    NodesNumber = p.Nodes.Count,
                    Timestamp = p.Timestamp,
                    WatchersNumber = p.Watchers.Count
                });

            }
            return projectsList;
        }

    }

}