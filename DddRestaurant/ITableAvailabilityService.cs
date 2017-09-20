namespace DddRestaurant
{
    using System;
    using System.Collections.Generic;

    public interface ITableAvailabilityService
    {
        bool CheckAvailability(List<int> tablesList, DateTime startDate, DateTime endDate);
    }
}