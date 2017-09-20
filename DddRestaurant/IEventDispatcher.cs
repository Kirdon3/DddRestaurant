using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddRestaurant
{
    using Events;

    public interface IEventDispatcher
    {
        void DispatchReservationRequestAcceptedEvent(ReservationRequestAccepted reservationRequestAccepted);

        void DispatchReservationRequestDeniedEvent(ReservationRequestDenied reservationRequestDenied);

        void DispatchReservationCancelledEvent(ReservationCancelledEvent reservationCancelledEvent);

        void DispatchReservationCalledOffEvent(ReservationCalledOff reservationCalledOffEvent);
        void DispatchCallOffDeniedEvent(CallOffDeniedEvent callOffDeniedEvent);
    }
}
