namespace DddRestaurant
{
    using System;
    using System.Collections.Generic;

    public class Reservation
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<int> TableIds { get; set; }

        public int RestaurantId { get; set; }

        public bool Accepted { get; set; }

        public int ClientId { get; set; }

        public int AcceptedBy { get; set; }



    }
}
