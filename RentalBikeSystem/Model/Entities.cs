using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using RentalBikeSystem.Poco;

namespace RentalBikeSystem.Model
{
    public class Customer
    {
        //Properties
        [Key]
        public int CustomerId { get; set; }
        public String Gender { get; set; }
        [MaxLength(50)]
        [Required]
        public String Firstname { get; set; }
        [MaxLength(75)]
        [Required]
        public String Lastname { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }
        [MaxLength(75)]
        [Required]
        public String Street { get; set; }
        [MaxLength(10)]
        public int? Housenumber { get; set; }
        [MaxLength(10)]
        [Required]
        public int ZipCode { get; set; }
        [MaxLength(75)]
        [Required]
        public String Town { get; set; }

        //Konstruktoren
        public Customer()
        {

        }
        public Customer(CustomerPoco customerPoco)
        {
            this.Birthday = customerPoco.Birthday;
            this.CustomerId = customerPoco.CustomerId;
            this.Firstname = customerPoco.Firstname;
            this.Gender = customerPoco.Gender;
            this.Housenumber = customerPoco.Housenumber;
            this.Lastname = customerPoco.Lastname;
            this.Street = customerPoco.Street;
            this.Town = customerPoco.Town;
            this.ZipCode = customerPoco.ZipCode;
        }
    }

    public class Bike
    {
        //Proberties
        [Key]
        public int BikeId { get; set; }
        [MaxLength(10)]
        [Required]
        public String Brand { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime PurchaseDate { get; set; }
        [MaxLength(1000)]
        public String Notes { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfLastService { get; set; }
        [Required]
        [Range(0,Double.PositiveInfinity)]
        public double PriceFirstHour { get; set; }
        [Required]
        [Range(1, Double.PositiveInfinity)]
        public double PriceAdditionalHour { get; set; }
        public String Category { get; set; }

        //Konstruktoren
        public Bike()
        {

        }
        public Bike(BikePoco bikePoco)
        {
            this.BikeId = bikePoco.BikeId;
            this.Brand = bikePoco.Brand;
            this.PurchaseDate = bikePoco.PurchaseDate;
            this.Notes = bikePoco.Notes;
            this.DateOfLastService = bikePoco.DateOfLastService;
            this.PriceFirstHour = bikePoco.PriceFirstHour;
            this.PriceAdditionalHour = bikePoco.PriceAdditionalHour;
            this.Category = bikePoco.Category;
        }
    }
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }
        public virtual Customer Customer{ get; set; }
        public virtual Bike Bike { get; set; }
        [Required]
        public DateTime RentalBegin { get; set; }
        private DateTime? rentalEnd;
        [Required]
        public DateTime? RentalEnd {
            get
            {
                return rentalEnd;
            }
            set {
                if (value <= RentalBegin)
                {
                    value = RentalBegin.AddMilliseconds(1);
                }
                rentalEnd = value;
            }
        }
        [Range(1, Double.PositiveInfinity)]
        public double TotalPrice { get; set; }
        public bool Paid { get; set; }
    }
}
