using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentalBikeSystem.Model;
using RentalBikeSystem.DataAccess;
using RentalBikeSystem.Poco;
using RentalBikeSystem.DTO;

namespace RentalBikeSystem.BusinessLogic
{
    public class RentalBusinessLogic
    {
        private RentalDataAccess rentalDataAccess;

        public RentalBusinessLogic()
        {
            this.rentalDataAccess = new RentalDataAccess();
        }
        //************************************** Customer **************************************
        public List<Customer> GetAllCustomersWithFilter(string lastnameFilter)
        {
            return this.rentalDataAccess.GetAllCustomersWithFilter(lastnameFilter);
        }

        public void CreateNewCustomer(CustomerPoco newPocoCustomer)
        {
            Customer newCustomer = new Customer(newPocoCustomer);
            this.rentalDataAccess.CreateNewCustomer(newCustomer);
        }

        public void UpdateCustomer(CustomerPoco customer)
        {
            var oldCustomer = this.rentalDataAccess.GetCustomerById(customer.CustomerId);
            if (oldCustomer != null)
            {
                this.rentalDataAccess.UpdateCustomer(oldCustomer, customer);
            }
            else
            {
                throw new Exception("Customer does not exist!");
            }
            
        }

        public void DeleteCustomer(int customerId)
        {
            Customer customer = this.rentalDataAccess.GetCustomerById(customerId);
            if (customer != null)
            {
                this.rentalDataAccess.DeleteCustomer(customer);
            }
            else
            {
                throw new Exception("Customer is null!");
            }
        }

        public List<Rental> GetAllRentalsByCustomerId(int customerId)
        {
            List<Rental> rentals = this.rentalDataAccess.GetAllRentalsByCustomerId(customerId);
            foreach(Rental rental in rentals)
            {
                this.CalculateCostOfRental(rental);
            }
            return rentals;
        }
        //************************************** Bike **************************************
        public List<Bike> GetAllAvailableBikes(GetAllAvailableBikesDTO getAllAvailableBikesDTO)
        {
            return this.rentalDataAccess.GetAllAvailableBikes(getAllAvailableBikesDTO);
        }

        public void CreateNewBike(BikePoco newPocoBike)
        {
            Bike newBike = new Bike(newPocoBike);
            this.rentalDataAccess.CreateNewBike(newBike);
        }
        public void UpdateBike(BikePoco bike)
        {
            var oldBike = this.rentalDataAccess.GetBikeById(bike.BikeId);
            if(oldBike != null)
            {
                this.rentalDataAccess.UpdateCustomer(oldBike, bike);
            }
            else
            {
                throw new Exception("Bike does not exist!");
            }
        }

        public void StartRental(StartRentalDTO startRentalDTO)
        {
            Bike bike = this.rentalDataAccess.GetBikeById(startRentalDTO.BikeId);
            Customer customer = this.rentalDataAccess.GetCustomerById(startRentalDTO.CustomerId);

            Rental rental = new Rental();
            rental.Paid = false;
            rental.RentalBegin = DateTime.Now;
            rental.RentalEnd = null;
            rental.TotalPrice = 0;
            rental.Bike = bike;
            rental.Customer = customer;

            this.rentalDataAccess.CreateRental(rental);
        }

        public List<GetRentalsDTO> GetRentals()
        {
            var rentals = this.rentalDataAccess.GetAllEndedRentalsThatArePaid();
            List<GetRentalsDTO> ret = new List<GetRentalsDTO>();

            rentals.ForEach(r => CalculateCostOfRental(r));

            foreach(Rental rental in rentals.Where(r => r.TotalPrice > 0))
            {
                ret.Add(new GetRentalsDTO(rental));
            }
            return ret;

        }

        public void MarkRentalAsPaid(int rentalId)
        {
            var oldRental = this.rentalDataAccess.GetRentalById(rentalId);
            var rental = this.rentalDataAccess.GetRentalById(rentalId);
            if (oldRental != null)
            {
                CalculateCostOfRental(oldRental);
                CalculateCostOfRental(rental);

                if (rental.TotalPrice > 0)
                {
                    rental.Paid = true;
                    this.rentalDataAccess.UpdateRental(oldRental, rental);
                }
                else
                {
                    throw new Exception("Rental Totalprice is not > 0!");
                }
                
            }
            else
            {
                throw new Exception("Rental does not exist!");
            }
        }

        public void EndRental(int rentalId)
        {
            var oldRental = this.rentalDataAccess.GetRentalById(rentalId);
            var rental = this.rentalDataAccess.GetRentalById(rentalId);
            if (oldRental != null)
            {
                rental.RentalEnd = DateTime.Now;
                this.CalculateCostOfRental(rental);
                this.rentalDataAccess.UpdateRental(oldRental,rental);
            }
            else
            {
                throw new Exception("Rental does not exist!");
            }

        }

        

        //Delete
        public void DeleteBike(int bikeId)
        {
            Bike bike = this.rentalDataAccess.GetBikeById(bikeId);
            if(bike != null)
            {
                this.rentalDataAccess.DeleteBike(bike);
            }
            else
            {
                throw new Exception("Bike is null!");
            }
        }

        private void CalculateCostOfRental(Rental rental)
        {
            if(rental.RentalEnd != null)
            {
                TimeSpan rentalDuration = rental.RentalEnd.Value.Subtract(rental.RentalBegin);
                double rentalDurationInHours = rentalDuration.TotalHours;

                if(rentalDurationInHours <= 0.25)
                {
                    rental.TotalPrice = 0;
                }
                else if(rentalDurationInHours <= 1)
                {
                    rental.TotalPrice = 3;
                }
                else
                {
                    var addidionalRentalDurationInHours = Math.Ceiling(rentalDurationInHours - 1);  //Math.Ceiling Rundet auf

                    rental.TotalPrice = 3 + (addidionalRentalDurationInHours * 5);
                }
            }
        }
    }
}
