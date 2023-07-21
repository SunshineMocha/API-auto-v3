using System;
using System.Reflection.Metadata.Ecma335;

namespace API_Auto_v3._0.Models
{
    public class CarDealer
    {
        public int DealerID { get; set; }
        public string DealerName { get; set; }
        public List<Car> CarList { get; set; }
    }

    public class Car
    {
        public string CarPlate { get; set; }
        public string Model { get; set; }
        public int MaxSpeed { get; set; }
        public double Displacement { get; set; }
    }

}