﻿using System;
using System.Collections.Generic;

namespace TrainBooking.Entities
{
    public partial class Booking
    {
        public Booking()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
