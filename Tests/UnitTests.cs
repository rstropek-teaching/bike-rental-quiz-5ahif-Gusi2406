using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalBikeSystem.Context;
using RentalBikeSystem.Controllers;
using RentalBikeSystem.Model;
using RentalBikeSystem.Poco;
using System;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        RentalController rentalController;

        public UnitTest1()
        {
            this.rentalController = new RentalController();
        }
        [TestMethod]
        public void TestCreateNewBike()
        {
            BikePoco bikePoco = new BikePoco
            {
                Brand = "KTM",
                Category = "Sport Bike",
                DateOfLastService = DateTime.Parse("2018/01/20"),
                PriceFirstHour = 3,
                PriceAdditionalHour = 5,
                PurchaseDate = DateTime.Now
            };
            this.rentalController.CreateNewBike(bikePoco);

            Bike bike = this.rentalController.GetAllAvailableBikes(new RentalBikeSystem.DTO.GetAllAvailableBikesDTO()).Find(b => b.Brand == bikePoco.Brand);

            if(bike == null)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void TestCreateRental()
        {
            BikePoco bikePoco = new BikePoco
            {
                Brand = "KTM",
                Category = "Sport Bike",
                DateOfLastService = DateTime.Parse("2018/01/20"),
                PriceFirstHour = 3,
                PriceAdditionalHour = 5,
                PurchaseDate = DateTime.Now
            };
            this.rentalController.CreateNewBike(bikePoco);

            CustomerPoco customerPoco = new CustomerPoco
            {
                Birthday = DateTime.Parse("2018/06/24"),
                Firstname = "Philipp",
                Lastname = "CreateCustomerTest",
                Gender = "männlich",
                Housenumber = 24,
                Street = "Marktplatz",
                ZipCode = 4310,
                Town = "Mauthausen"

            };
            this.rentalController.CreateNewCustomer(customerPoco);

            BikeContext context = new BikeContext();

            Bike bike = context.Bikes.Last();
            Customer customer = context.Customers.Last();
            Rental rental = new Rental
            {
                Bike = bike,
                Customer = customer,
                Paid = false,
                RentalBegin = DateTime.Parse("24.06.1999 10:00:00"),
                RentalEnd = DateTime.Parse("24.06.1999 11:00:00"),
                TotalPrice = 0
            };

            context.Add(rental);
            context.SaveChanges();

            Rental selectRental = this.rentalController.GetAllRentalsByCustomerId(customer.CustomerId).Find(r => r.Customer.CustomerId == customer.CustomerId);

            if (selectRental == null)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void TestCreateNewCustomer()
        {
            CustomerPoco customer = new CustomerPoco
            {
                Birthday = DateTime.Parse("2018/06/24"),
                Firstname = "Philipp",
                Lastname = "CreateCustomerTest",
                Gender = "männlich",
                Housenumber = 24,
                Street = "Marktplatz",
                ZipCode = 4310,
                Town = "Mauthausen"

            };
            this.rentalController.CreateNewCustomer(customer);

            Customer selectCustomer = this.rentalController.GetAllCustomers(customer.Lastname).Find(c => c.Lastname == customer.Lastname);

            if (selectCustomer == null)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void TestDeleteCustomer()
        {
            //Anlegen des Customers
            CustomerPoco customerPoco = new CustomerPoco
            {
                Birthday = DateTime.Parse("2018/06/24"),
                Firstname = "Philipp",
                Lastname = "Mustermanntest",
                Gender = "männlich",
                Housenumber = 24,
                Street = "Marktplatz",
                ZipCode = 4310,
                Town = "Mauthausen"

            };
            //Einfügen
            this.rentalController.CreateNewCustomer(customerPoco);

            BikeContext context = new BikeContext();
            Customer customer = context.Customers.Last();
            //Löschen
            this.rentalController.DeleteCustomer(customer.CustomerId);

            //Selektiern
            Customer selectCustomer = this.rentalController.GetAllCustomers(customerPoco.Lastname).Find(c => c.CustomerId == customer.CustomerId);
            
            //Nachschauen
            if(selectCustomer != null)
            {
                Assert.Fail();
            }
        }
    }
}
