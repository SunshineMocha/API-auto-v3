using API_Auto_v3._0.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
//using System.Web.Http.Controllers;

namespace API_Auto_v3._0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarDealerController : ControllerBase
    {
        private static List<CarDealer> carDealers = new List<CarDealer>();

        // GET ALL Car Dealers
        // GET api/CarDealer
        [HttpGet]
        public ActionResult<IEnumerable<CarDealer>> Get()
        {
            return carDealers;
        }


        // GET Car Dealers by ID
        // GET api/CarDealer/5
        [HttpGet("{id}")]
        public ActionResult<CarDealer> Get(int id)
        {
            var carDealer = carDealers.FirstOrDefault(d => d.DealerID == id);
            if (carDealer == null)
            {
                return NotFound();
            }
            return carDealer;
        }

        // POST Car Dealer with 1 (or more) cars
        // POST api/CarDealer
        [HttpPost]
        public ActionResult<CarDealer> Post(CarDealer carDealer)
        {
            carDealers.Add(carDealer);
            return CreatedAtAction(nameof(Get), new { id = carDealer.DealerID }, carDealer);
        }

        // POST Car to an existing Car Dealer
        // POST api/CarDealer/ID
        [HttpPost]
        [Route("api/CarDealer/{id}")]
        public string CreateCar(int id, [FromBody] Car car)
        {
            // Assuming you have a Car Dealer list called "carDealerList"
            var carDealer = carDealers.FirstOrDefault(c => c.DealerID == id);
            if (carDealer == null)
            {
                return "Not Found!";
            }

            carDealer.CarList.Add(car);

            return "Car Added!";
        }


        // PUT Updates Car Dealer ID and Name
        // PUT api/CarDealer/5
        [HttpPut]
        public ActionResult<CarDealer> Put(int id, CarDealer updatedCarDealer)
        {
            var carDealer = carDealers.FirstOrDefault(d => d.DealerID == id);
            if (carDealer == null)
            {
                return NotFound();
            }

            carDealer.DealerID = updatedCarDealer.DealerID;
            carDealer.DealerName = updatedCarDealer.DealerName;

            return carDealer;
        }

        // PUT Updates Car Attributes given CarPlate
        // PUT api/CarDealer/5/AA000AA
        [HttpPut("{carPlate}")]
        public IActionResult EditCar(string carPlate, [FromBody] Car updatedCar)
        {
            foreach (var carDealer in carDealers)
            {
                // Find the car with the given car plate in the car dealer's car list
                var carToUpdate = carDealer.CarList.FirstOrDefault(car => car.CarPlate == carPlate);

                if (carToUpdate != null)
                {
                    carToUpdate.CarPlate = updatedCar.CarPlate;
                    carToUpdate.Model = updatedCar.Model;
                    carToUpdate.MaxSpeed = updatedCar.MaxSpeed;
                    carToUpdate.Displacement = updatedCar.Displacement;
                    return Ok("Car attributes updated successfully.");
                }
            }
            return NotFound("Car not found.");
        }

        // DELETE Car from Car Dealer based on CarPlate
        // DELETE api/CarDealer/5
        [HttpDelete("{plate}")]
        public ActionResult Delete(string plate)
        {
            foreach (var carDealer in carDealers)
            {
                // Find the car with the given car plate in the car dealer's car list
                var carToRemove = carDealer.CarList.FirstOrDefault(car => car.CarPlate == plate);

                if (carToRemove != null)
                {
                    // Remove the car from the car list
                    carDealer.CarList.Remove(carToRemove);
                    return NoContent();
                }
            }
            return NotFound();
        }
    }
}