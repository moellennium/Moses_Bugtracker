using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moses_Bugtracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //Navigational Properties (aka the Relationship)
        //Navigational properties pointing to all the Ticket Children
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Users = new HashSet<ApplicationUser>();
        }
         

    }
}