namespace DddRestaurant
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Events;

    public class ReservationRequest
    {
        private IEventDispatcher eventDispatcher;
        private ITableAvailabilityService tableAvailabilityService;

        public ReservationRequest(IEventDispatcher eventDispatcher,
            ITableAvailabilityService tableAvailabilityService)
        {
            this.eventDispatcher = eventDispatcher;
            this.tableAvailabilityService = tableAvailabilityService;
        }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<int> TableIds { get; set; }

        public int RestaurantId { get; set; }

        public bool? Accepted { get; set; }

        public int ClientId { get; set; }

        public int AcceptedBy { get; set; }

        public void ReservationRequestAcceptedHandler(AcceptReservationRequest command)
        {
            if (tableAvailabilityService.CheckAvailability(this.TableIds, this.StartDate, this.EndDate))
            {
                Accepted = true;
                var reservationAcceptedEvent = new ReservationRequestAccepted();
                eventDispatcher.DispatchReservationRequestAcceptedEvent(reservationAcceptedEvent);
                return;
            }


            Accepted = false;
            var reservationDeniedEvent = new ReservationRequestDenied();
            eventDispatcher.DispatchReservationRequestDeniedEvent(reservationDeniedEvent);


        }

        public void ReservationRequestDeniedHandler(DenyReservationRequest command)
        {
            Accepted = false;

            var reservationDeniedEvent = new ReservationRequestDenied();
            eventDispatcher.DispatchReservationRequestDeniedEvent(reservationDeniedEvent);
        }
    }
}
