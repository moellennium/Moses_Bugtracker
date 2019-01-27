﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moses_Bugtracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public string FilePath { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public string UserId { get; set; }

        // Navagational properties (aka the relationship)
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    
}