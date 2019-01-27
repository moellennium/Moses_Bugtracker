﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moses_Bugtracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public string Property { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public DateTime Changed { get; set; }

        public string UserId { get; set; }

        // Navagational properties (aka the relationship)
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}