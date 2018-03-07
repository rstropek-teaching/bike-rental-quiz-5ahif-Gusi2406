using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentalBikeSystem.Model;

namespace RentalBikeSystem.DTO
{
    //Data Transfer Objects
    //Wenn man mehrere Attribute bei über den Body bei den HTTP Methoden übergeben will, braucht man ein Data Transfer Object
    public class GetAllAvailableBikesDTO
    {
        public bool SortByPriceOfFirstHourAsc { get; set; }
        public bool SortByPriceOfAdditionalHoursAsc { get; set; }
        public bool SortByPurchaseDateDsc { get; set; }
    }
    public class StartRentalDTO
    {
        public int BikeId { get; set; }
        public int CustomerId { get; set; }
    }
    public class GetRentalsDTO
    {

        public GetRentalsDTO(Rental rental)
        {
            this.CustomerId = rental.RentalId;
            this.FirstName = rental?.Customer?.Firstname;
            this.LastName = rental?.Customer?.Lastname;
            this.RentalId = rental.RentalId;
            this.StartDate = rental.RentalBegin;
            this.EndDate = rental?.RentalEnd;
            this.TotalPrice = rental.TotalPrice;
        }

        public int CustomerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int RentalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double TotalPrice { get; set; }
    }
}
