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

        public int? HeadNodeId { get; set; }
        public byte[] HeadImage { get; set; }

        public int NodesNumber { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public int AssignedUsersCount { get; set; }

        public bool Accepted { get; set; }
        public bool Rejected { get; set; }

        public DateTime Timestamp { get; set; }



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
                    HeadNodeId = p.nodeHeadId,
                    Owner = p.Owner,
                    NodesNumber = p.Nodes.Count,
                    Accepted = p.Accepted,
                    Rejected = p.Rejected,
                    Timestamp = p.CreateDate,
                    AssignedUsersCount = p.AssignedUsers.Count
                });

            }
            return projectsList;
        }

    }

}