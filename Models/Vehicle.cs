using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace Project1.Models
{
    public class Vehicle
    {
        public int ID { get; set; }
        [Required]
        public string CarBrand { get; set; }
        [Required]
        public string CarModel { get; set; }
        [Required]
        public string ProductionDate { get; set; }
        [Required]
        public double Price { get; set; }

    }
}
