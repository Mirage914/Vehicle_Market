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
        
        public string CarBrand { get; set; }
       
        public string CarModel { get; set; }
       
        public string ProductionDate { get; set; }

        public double Price { get; set; }

        public string ImageName { get; set; }


    }
}
