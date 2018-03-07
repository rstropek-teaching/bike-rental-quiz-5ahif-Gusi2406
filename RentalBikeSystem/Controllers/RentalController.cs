using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RentalBikeSystem.Model;
using RentalBikeSystem.BusinessLogic;
using RentalBikeSystem.Poco;
using RentalBikeSystem.DTO;

namespace RentalBikeSystem.Controllers
{
    [Route("api/rental")]
    public class RentalController : Controller
    {
        RentalBusinessLogic rentalBusinessLogic;

        public RentalController()
        {
            this.rentalBusinessLogic = new RentalBusinessLogic();
        }
        //************************************** Customer **************************************
        [HttpPost]
        [Route("GetAllCustomers")]
        public List<Customer> GetAllCustomers([FromBody]string lastnameFilter)
        {
            return this.rentalBusinessLogic.GetAllCustomersWithFilter(lastnameFilter);
        }

        [HttpPost]
        [Route("CreateNewCustomer")]
        public void CreateNewCustomer([FromBody]CustomerPoco newCustomer)
        {
            this.rentalBusinessLogic.CreateNewCustomer(newCustomer);
        }

        [HttpPost]
        [Route("UpdateCustomer")]
        public void UpdateCustomer([FromBody]CustomerPoco customer)
        {
            this.rentalBusinessLogic.UpdateCustomer(customer);
        }

        [HttpPost]
        [Route("DeleteCustomer")]
        public void DeleteCustomer([FromBody]int customerId)
        {
            this.rentalBusinessLogic.DeleteCustomer(customerId);
        }

        [HttpPost]
        [Route("GetAllRentalsByCustomerId")]
        public List<Rental> GetAllRentalsByCustomerId([FromBody]int customerId)
        {
            return this.rentalBusinessLogic.GetAllRentalsByCustomerId(customerId);
        }
        //************************************** Bike **************************************
        [HttpPost]
        [Route("GetAllAvailableBikes")]
        public List<Bike> GetAllAvailableBikes([FromBody] GetAllAvailableBikesDTO getAllAvailableBikesDTO)
        {
            return this.rentalBusinessLogic.GetAllAvailableBikes(getAllAvailableBikesDTO);
        }

        [HttpPost]
        [Route("CreateNewBike")]
        public void CreateNewBike([FromBody]BikePoco newBike)
        {
            this.rentalBusinessLogic.CreateNewBike(newBike);
        }
        [HttpPost]
        [Route("UpdateBike")]
        public void UpdateBike([FromBody]BikePoco updateBike)
        {
            this.rentalBusinessLogic.UpdateBike(updateBike);
        }
        [HttpPost]
        [Route("DeleteBike")]
        public void DeleteBike([FromBody]int bikeId)
        {
            this.rentalBusinessLogic.DeleteBike(bikeId);
        }
        //************************************** Rental **************************************
        [HttpPost]
        [Route("StartRental")]
        public void StartRental([FromBody]StartRentalDTO startRentalDTO)
        {
            this.rentalBusinessLogic.StartRental(startRentalDTO);
        }
        [HttpPut]
        [Route("EndRental")]
        public void EndRental([FromBody]int rentalId)
        {
            this.rentalBusinessLogic.EndRental(rentalId);
        }
        [HttpPut]
        [Route("MarkRentalAsPaid")]
        public void MarkRentalAsPaid([FromBody]int rentalId)
        {
            this.rentalBusinessLogic.MarkRentalAsPaid(rentalId);
        }
        [HttpPost]
        [Route("GetRentals")]
        public List<GetRentalsDTO> GetRentals()
        {
            return this.rentalBusinessLogic.GetRentals();
        }


    }
}
