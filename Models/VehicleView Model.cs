using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class VehicleView_Model
    {
        public int ID { get; set; }
        public string CarBrand { get; set; }

        public string CarModel { get; set; }

        public string ProductionDate { get; set; }

        public double Price { get; set; }
        public IFormFile Image { get; set; }

    }
}
