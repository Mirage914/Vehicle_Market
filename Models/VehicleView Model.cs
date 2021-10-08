using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class VehicleView_Model
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Vehicle Brand field is required.")]
        public string CarBrand { get; set; }
        [Required(ErrorMessage = "Vehicle Model field is required.")]
        public string CarModel { get; set; }
        [Required(ErrorMessage = "Production Date field is required.")]
        public string ProductionDate { get; set; }
        [Required]
        public double Price { get; set; }

        public IFormFile Image { get; set; }
        public string ImageName { get; set; }

    }
}
