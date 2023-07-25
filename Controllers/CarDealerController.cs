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

        // Getting ALL the car dealers
        // GET api/CarDealer
        [HttpGet]
        public ActionResult<IEnumerable<CarDealer>> Get()
        {
            return carDealers;
        }

        // Getting the car dealer by ID
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

        // Creating a car dealer with one (or more) cars
        // POST api/CarDealer
        [HttpPost]
        public ActionResult<CarDealer> Post(CarDealer carDealer)
        {
            carDealers.Add(carDealer);
            return CreatedAtAction(nameof(Get), new { id = carDealer.DealerID }, carDealer);
        }

        // Creating a new car record to an existing car dealer
        // POST api/CarDealer/ID
        [HttpPost]
        [Route("{id}")]
        public string CreateCar(int id, [FromBody] Car car)
        {
            var carDealer = carDealers.FirstOrDefault(c => c.DealerID == id);
            if (carDealer == null)
            {
                return "Not Found!";
            }

            carDealer.CarList.Add(car);

            return "Car Added!";
        }

        // Updating car dealer ID and name
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

        // Update of a single car object
        // PUT api/CarDealer/5/AA000AA
        [HttpPut("{carPlate}")]
        public IActionResult EditCar(string carPlate, [FromBody] Car updatedCar)
        {
            foreach (var carDealer in carDealers)
            {
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

        // Deleting a car by its plate
        // DELETE api/CarDealer/5
        [HttpDelete("{plate}")]
        public ActionResult Delete(string plate)
        {
            foreach (var carDealer in carDealers)
            {
                var carToRemove = carDealer.CarList.FirstOrDefault(car => car.CarPlate == plate);

                if (carToRemove != null)
                {
                    carDealer.CarList.Remove(carToRemove);
                    return NoContent();
                }
            }
            return Ok();
        }
    }
}