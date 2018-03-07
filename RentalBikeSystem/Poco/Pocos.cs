using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalBikeSystem.Poco
{
    //************************************** Customer **************************************
    public class CustomerPoco
    {
        public int CustomerId { get; set; }
        public String Gender { get; set; }
        public String Firstname { get; set; }
        public String Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public String Street { get; set; }
        public int? Housenumber { get; set; }
        public int ZipCode { get; set; }
        public String Town { get; set; }
    }
    //************************************** Bike **************************************
    public class BikePoco
    {
        public int BikeId { get; set; }
        public String Brand { get; set; }
        public DateTime PurchaseDate { get; set; }
        public String Notes { get; set; }
        public DateTime? DateOfLastService { get; set; }
        public double PriceFirstHour { get; set; }
        public double PriceAdditionalHour { get; set; }
        public String Category { get; set; }
    }
    //************************************** Rental **************************************
    public class RentalPoco
    {
        public int RentalId { get; set; }
        public virtual CustomerPoco Customer { get; set; }
        public virtual BikePoco Bike { get; set; }
        public DateTime RentalBegin { get; set; }
        private DateTime? rentalEnd;
        public DateTime? RentalEnd
        {
            get
            {
                return rentalEnd;
            }
            set
            {
                if (value <= RentalBegin)
                {
                    value = RentalBegin.AddMilliseconds(1);
                }
                rentalEnd = value;
            }
        }
        public double TotalPrice { get; set; }
        public bool Paid { get; set; }
    }
}
