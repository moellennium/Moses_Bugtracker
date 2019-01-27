using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moses_Bugtracker.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //Navigational Properties (aka the Relationship)
        //Navigational properties pointing to all the Ticket Children
        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketPriority()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    }
}