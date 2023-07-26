using API_Auto_v3._0.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace API_Auto_v3._0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarDealerController : ControllerBase
    {
        private static List<CarDealer> carDealers = new List<CarDealer>();

        private static string filePath = "data.txt";

        private static void ReadRecords()
        {
            if (System.IO.File.Exists(filePath))
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] recordData = line.Split(";");
                    int dealerId = Convert.ToInt32(recordData[0]);
                    string dealerName = recordData[1];
                    List<Car> carList = new List<Car>();

                    for (int i = 2; i < recordData.Length; i += 4)
                    {
                        string carPlate = recordData[i];
                        string model = recordData[i + 1];
                        int maxSpeed = Convert.ToInt32(recordData[i + 2]);
                        double displacement = Convert.ToDouble(recordData[i + 3]);

                        Car car = new Car
                        {
                            CarPlate = carPlate,
                            Model = model,
                            MaxSpeed = maxSpeed,
                            Displacement = displacement
                        };
                        carList.Add(car);
                    }

                    CarDealer carDealer = new CarDealer
                    {
                        DealerID = dealerId,
                        DealerName = dealerName,
                        CarList = carList
                    };

                    carDealers.Add(carDealer);
                }
            }
        }

        private static void WriteRecords()
        {
            List<string> lines = new List<string>();
            foreach (CarDealer carDealer in carDealers)
            {
                string recordData = $"{carDealer.DealerID};{carDealer.DealerName}";
                foreach (Car car in carDealer.CarList)
                {
                    recordData += $";{car.CarPlate};{car.Model};{car.MaxSpeed};{car.Displacement}";
                }

                lines.Add(recordData);
            }
            System.IO.File.WriteAllLines(filePath, lines);
        }

        // Getting ALL the car dealers
        // GET api/CarDealer

        [HttpGet]
        public ActionResult<IEnumerable<CarDealer>> Get()
        {
            carDealers = new List<CarDealer>();
            //CODICE PER READ FILE
            ReadRecords();
            //FINE CODICE READ FILE
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

        // Creating a car dealer
        // POST api/CarDealer
        [HttpPost]
        public ActionResult<CarDealer> Post(CarDealer carDealer)
        {
            carDealers.Add(carDealer);
            //CODICE PER WRITE FILE
            WriteRecords();
            //FINE CODICE WRITE FILE
            return CreatedAtAction(nameof(Get), new { id = carDealer.DealerID }, carDealer);
        }

        // Creating a new car record to an existing car dealer
        // POST api/CarDealer/ID
        [HttpPost]
        [Route("{id}")]
        public IActionResult CreateCar(int id, [FromBody] Car car)
        {
            var carDealer = carDealers.FirstOrDefault(c => c.DealerID == id);
            if (carDealer == null)
            {
                return BadRequest();
            }

            carDealer.CarList.Add(car);
            //CODICE PER WRITE FILE
            WriteRecords();
            //FINE CODICE WRITE FILE
            return Ok();
        }

        // Update car dealer name
        // PUT api/CarDealer/5
        [HttpPut]
        public IActionResult EditDealer (int id, [FromBody] CarDealer updatedCarDealer)
        {
            var cardealerToUpdate = carDealers.FirstOrDefault(d => d.DealerID == id);

            if (cardealerToUpdate == null) throw new Exception("Il concessionario non esiste");

            cardealerToUpdate.DealerName = updatedCarDealer.DealerName;

            //CODICE PER WRITE FILE
            WriteRecords();
            //FINE CODICE WRITE FILE
            return Ok();
        }

        // Update of a single car object
        // PUT api/CarDealer/5/AA000AA

        [HttpPut("{carPlate}")]
        public IActionResult EditCar(string carPlate, [FromBody] Car updatedCar)
        {
            bool carFound = false; // flag to indicate if the car is found and updated

            foreach (var carDealer in carDealers)
            {
                var carToUpdate = carDealer.CarList.FirstOrDefault(car => car.CarPlate == carPlate);

                if (carToUpdate != null)
                {
                    carToUpdate.CarPlate = updatedCar.CarPlate;
                    carToUpdate.Model = updatedCar.Model;
                    carToUpdate.MaxSpeed = updatedCar.MaxSpeed;
                    carToUpdate.Displacement = updatedCar.Displacement;

                    carFound = true; // Set the flag to true if the car is found and updated
                    break; // Exit the loop after updating the car details
                }
            }

            if (!carFound)
            {
                throw new Exception("La targa non esiste"); // Throw an exception if the car is not found
            }

            //CODICE PER WRITE FILE
            WriteRecords();
            //FINE CODICE WRITE FILE

            return Ok();
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
                    //CODICE PER WRITE FILE
                    WriteRecords();
                    //FINE CODICE WRITE FILE
                    return NoContent();
                }
            }
            return Ok();
        }
    }
}