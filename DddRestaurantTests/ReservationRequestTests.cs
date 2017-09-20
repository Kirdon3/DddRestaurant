namespace DddRestaurantTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DddRestaurant;
    using DddRestaurant.Commands;
    using DddRestaurant.Events;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ReservationRequestTests
    {
        [Test]
        public void WhenReservationRerquestAcceptedAndTableAvailableThenAcceptedIsTrue()
        {
            //Given
            Mock<IEventDispatcher> eventDispatcherMock = new Mock<IEventDispatcher>();
            Mock<ITableAvailabilityService> tableAvailabilityService = new Mock<ITableAvailabilityService>();
            Mock<IRestaurantConfigurationService> restaurantConfigurationMock = new Mock<IRestaurantConfigurationService>();
            tableAvailabilityService.Setup(
                x => x.CheckAvailability(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
            var reservationRequest = new ReservationRequest(eventDispatcherMock.Object, tableAvailabilityService.Object, restaurantConfigurationMock.Object);
            var acceptReservationRequest = new AcceptReservationRequest();

            //When
            reservationRequest.ReservationRequestAcceptedHandler(acceptReservationRequest);

            //Then
            Assert.IsTrue(reservationRequest.Accepted);
        }

        [Test]
        public void WhenReservationRerquestDeniedThenAcceptedIsFalse()
        {
            //Given
            Mock<IEventDispatcher> eventDispatcherMock = new Mock<IEventDispatcher>();
            Mock<ITableAvailabilityService> tableAvailabilityService = new Mock<ITableAvailabilityService>();
            Mock<IRestaurantConfigurationService> restaurantConfigurationMock = new Mock<IRestaurantConfigurationService>();

            var reservationRequest = new ReservationRequest(eventDispatcherMock.Object, tableAvailabilityService.Object, restaurantConfigurationMock.Object);
            var denyReservationRequest = new DenyReservationRequest();

            //When
            reservationRequest.ReservationRequestDeniedHandler(denyReservationRequest);

            //Then
            Assert.IsFalse(reservationRequest.Accepted);
        }

        [Test]
        public void WhenReservationRerquestAcceptedThenEmitsReservationAccepted()
        {
            // Given
            Mock<IEventDispatcher> eventDispatcherMock = new Mock<IEventDispatcher>();
            Mock<ITableAvailabilityService> tableAvailabilityService = new Mock<ITableAvailabilityService>();
            Mock<IRestaurantConfigurationService> restaurantConfigurationMock = new Mock<IRestaurantConfigurationService>();

            tableAvailabilityService.Setup(
                x => x.CheckAvailability(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
            var reservationRequest = new ReservationRequest(eventDispatcherMock.Object, tableAvailabilityService.Object, restaurantConfigurationMock.Object);
            var acceptReservationRequest = new AcceptReservationRequest();
            // When
            reservationRequest.ReservationRequestAcceptedHandler(acceptReservationRequest);

            // Then
            eventDispatcherMock.Verify(x => x.DispatchReservationRequestAcceptedEvent(It.IsAny<ReservationRequestAccepted>()), Times.Once);
        }

        [Test]
        public void WhenReservationRerquestDeniedThenEmitsReservationDenied()
        {
            // Given
            Mock<IEventDispatcher> eventDispatcherMock = new Mock<IEventDispatcher>();
            Mock<ITableAvailabilityService> tableAvailabilityService = new Mock<ITableAvailabilityService>();
            Mock<IRestaurantConfigurationService> restaurantConfigurationMock = new Mock<IRestaurantConfigurationService>();

            var reservationRequest = new ReservationRequest(eventDispatcherMock.Object, tableAvailabilityService.Object, restaurantConfigurationMock.Object);

            var denyReservationRequest = new DenyReservationRequest();

            // When
            reservationRequest.ReservationRequestDeniedHandler(denyReservationRequest);

            // Then
            eventDispatcherMock.Verify(x => x.DispatchReservationRequestDeniedEvent(It.IsAny<ReservationRequestDenied>()), Times.Once);
        }

        [Test]
        public void WhenTryingToCallOffReservationAfterBlockDate_ThenSendsCallOffDeniedEvent()
        {
            // Given
            Mock<IEventDispatcher> eventDispatcherMock = new Mock<IEventDispatcher>();
            Mock<ITableAvailabilityService> tableAvailabilityService = new Mock<ITableAvailabilityService>();
            Mock<IRestaurantConfigurationService> restaurantConfigurationMock = new Mock<IRestaurantConfigurationService>();

            var reservationRequest = new ReservationRequest(eventDispatcherMock.Object, tableAvailabilityService.Object, restaurantConfigurationMock.Object);
            reservationRequest.StartDate = DateTime.UtcNow;

            var callOffReservationRequest = new CallOffReservationRequest
            {
                RequestDate = DateTime.UtcNow
            };

            restaurantConfigurationMock.Setup(x => x.CallOffPossibleInDays()).Returns(1);
            // When
            reservationRequest.ReservationCalledOffHandler(callOffReservationRequest);

            // Then
            eventDispatcherMock.Verify(x => x.DispatchCallOffDeniedEvent(It.IsAny<CallOffDeniedEvent>()), Times.Once);
        }

        [Test]
        public void WhenTryingToCallOffReservationBeforeBlockDate_ThenSendsCallOffDeniedEvent()
        {
            // Given
            Mock<IEventDispatcher> eventDispatcherMock = new Mock<IEventDispatcher>();
            Mock<ITableAvailabilityService> tableAvailabilityService = new Mock<ITableAvailabilityService>();
            Mock<IRestaurantConfigurationService> restaurantConfigurationMock = new Mock<IRestaurantConfigurationService>();

            var reservationRequest = new ReservationRequest(eventDispatcherMock.Object, tableAvailabilityService.Object, restaurantConfigurationMock.Object);
            reservationRequest.StartDate = DateTime.UtcNow;

            var callOffReservationRequest = new CallOffReservationRequest
            {
                RequestDate = DateTime.UtcNow.AddDays(-5)
            };

            restaurantConfigurationMock.Setup(x => x.CallOffPossibleInDays()).Returns(1);
            // When
            reservationRequest.ReservationCalledOffHandler(callOffReservationRequest);

            // Then
            eventDispatcherMock.Verify(x => x.DispatchReservationCalledOffEvent(It.IsAny<ReservationCalledOff>()), Times.Once);
        }
    }
}
