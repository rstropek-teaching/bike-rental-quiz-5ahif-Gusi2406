using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentalBikeSystem.Model;
using RentalBikeSystem.Context;
using RentalBikeSystem.Poco;
using Microsoft.EntityFrameworkCore;
using RentalBikeSystem.DTO;

namespace RentalBikeSystem.DataAccess
{
    public class RentalDataAccess
    {
        private BikeContext bikeContext;

        public RentalDataAccess()
        {
            this.bikeContext = new BikeContext();
        }
        //************************************** Customer **************************************
        //Get
        public List<Customer> GetAllCustomersWithFilter(string lastnameFilter)
        {
            return this.bikeContext.Customers.Where(c => c.Lastname == lastnameFilter).ToList();
        }
        //Create
        public void CreateNewCustomer(Customer newCustomer)
        {
            this.bikeContext.Customers.Add(newCustomer);
            this.bikeContext.SaveChanges();
        }
        //Update
        public void UpdateCustomer(Customer oldCustomer, CustomerPoco customer)
        {
            this.bikeContext.Entry(oldCustomer).CurrentValues.SetValues(customer);
            this.bikeContext.SaveChanges();
        }
        //Delete
        public void DeleteCustomer(Customer customer)
        {
            List<Rental> rentals = this.bikeContext.Rentals.Include("Bike").Where(r => r.Customer.CustomerId == customer.CustomerId).ToList();
            List<Bike> bikes = rentals.Select(r => r.Bike).ToList();
            this.bikeContext.Bikes.RemoveRange(bikes);
            this.bikeContext.Rentals.RemoveRange(rentals);

            this.bikeContext.Customers.Remove(customer);
            this.bikeContext.SaveChanges();
        }
        //Optionale Methoden
        public Customer GetCustomerById(int id)
        {
            return this.bikeContext.Customers.FirstOrDefault(i => i.CustomerId == id);
        }

        public List<Rental> GetAllRentalsByCustomerId(int customerId)
        {
            return this.bikeContext.Rentals.Where(c => c.Customer.CustomerId == customerId).ToList();
        }
        //************************************** Bike **************************************
        //Get
        public List<Bike> GetAllAvailableBikes(GetAllAvailableBikesDTO getAllAvailableBikesDTO)
        {
            //1. alle rentals selektieren
            var selectRentals = this.bikeContext.Rentals.ToList();
            //2. alle bikes selektieren
            var selectBikes = this.bikeContext.Bikes.ToList();
            //3. alle bikes entfernen, die in rentals sind
            for (int i = 0; i < selectRentals.Count; i++)
            {
                for (int y = 0; y < selectBikes.Count; y++)
                {
                    if (selectRentals[i].Bike.BikeId == selectBikes[y].BikeId)
                    {
                        Bike deleteBike = selectBikes[y];
                        selectBikes.Remove(deleteBike);
                    }
                }
            }


            //auch in einem schritt möglich
            var query = selectBikes.Where(b => b.BikeId > 0);  //todo query

            if (getAllAvailableBikesDTO.SortByPriceOfAdditionalHoursAsc)
            {
                query = query.OrderBy(b => b.PriceAdditionalHour);
            }
            if (getAllAvailableBikesDTO.SortByPriceOfFirstHourAsc)
            {
                query = query.OrderBy(b => b.PriceFirstHour);
            }
            if (getAllAvailableBikesDTO.SortByPurchaseDateDsc)
            {
                query = query.OrderByDescending(b => b.PurchaseDate);
            }


            return query.ToList();
        }

        public void CreateRental(Rental rental)
        {
            this.bikeContext.Add(rental);
            this.bikeContext.SaveChanges();
        }

        public List<Rental> GetAllEndedRentalsThatArePaid()
        {
            return this.bikeContext.Rentals.Where(r => r.Paid == false && r.RentalEnd != null).ToList();
        }



        //Delete
        public void DeleteBike(Bike bike)
        {
            List<Rental> rentals = this.bikeContext.Rentals.Include("Customer").Where(b => b.Bike.BikeId == bike.BikeId).ToList();
            List<Customer> customers = rentals.Select(c => c.Customer).ToList();

            this.bikeContext.Customers.RemoveRange(customers);
            this.bikeContext.Rentals.RemoveRange(rentals);

            this.bikeContext.Bikes.Remove(bike);
            this.bikeContext.SaveChanges();
        }

        public void UpdateRental(Rental oldRental, Rental rental)
        {
            this.bikeContext.Entry(oldRental).CurrentValues.SetValues(rental);
            this.bikeContext.SaveChanges();
        }

        //Update
        public void UpdateCustomer(Bike oldBike, BikePoco bike)
        {
            this.bikeContext.Entry(oldBike).CurrentValues.SetValues(bike);
            this.bikeContext.SaveChanges();
        }

        //Create
        public void CreateNewBike(Bike newBike)
        {
            this.bikeContext.Bikes.Add(newBike);
            this.bikeContext.SaveChanges();
        }
        public Bike GetBikeById(int id)
        {
            return this.bikeContext.Bikes.FirstOrDefault(i => i.BikeId == id);
        }
        public Rental GetRentalById(int id)
        {
            return this.bikeContext.Rentals.FirstOrDefault(i => i.RentalId == id);
        }
    }
}
