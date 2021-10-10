using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Project1.Controllers
{
    public class VehicleController : Controller
    {
        private readonly Project1Context _context;

        public VehicleController(Project1Context context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(string sortOrder, string searchString,string MaxPrice,string MinPrice)
        {
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            ViewData["Max"] = MaxPrice;
            ViewData["Min"] = MinPrice;
            var vehicles = from v in _context.Vehicle
                           select v;

            if (MinPrice != "0" && Convert.ToInt32(MaxPrice) > Convert.ToInt32(MinPrice))
            {
                vehicles = vehicles.Where(v => v.Price >= Convert.ToInt32(MinPrice) && v.Price < Convert.ToInt32(MaxPrice));
            }
            else if (MinPrice != "0")
            {
                vehicles = vehicles.Where(v => v.Price >= Convert.ToInt32(MinPrice));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(v => v.CarBrand.Contains(searchString)
                  || v.CarModel.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    vehicles = vehicles.OrderByDescending(v => v.CarBrand);
                    break;
                default:
                    vehicles = vehicles.OrderBy(v => v.CarBrand);
                    break;
            }
            return View(await vehicles.AsNoTracking().ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id, VehicleView_Model _vehicle)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Vehicle
               .FirstOrDefaultAsync(m => m.ID == id);
            _vehicle.CarBrand = cars.CarBrand;
            _vehicle.CarModel = cars.CarModel;
            _vehicle.Price = cars.Price;
            _vehicle.ProductionDate = cars.ProductionDate;
            _vehicle.ImageName = cars.ImageName;
            if (cars == null)
            {
                return NotFound();
            }

            return View(_vehicle);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {

            return View();



        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle, VehicleView_Model _vehicle)
        {
            vehicle.CarModel = _vehicle.CarModel;
            vehicle.CarBrand = _vehicle.CarBrand;
            vehicle.ProductionDate = _vehicle.ProductionDate;
            vehicle.Price = _vehicle.Price;
            var file = _vehicle.Image;
            if (file == null || file.Length == 0)
            {
                ViewBag.ErrorCreate = "You not select file.";
                return View();
            }
            vehicle.ImageName = file.FileName;
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/Images",
                         file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(_vehicle);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            VehicleView_Model _vehicle = new VehicleView_Model();
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicle.FindAsync(id);
            _vehicle.CarBrand = vehicle.CarBrand;
            _vehicle.CarModel = vehicle.CarModel;
            _vehicle.Price = vehicle.Price;
            _vehicle.ProductionDate = vehicle.ProductionDate;
            _vehicle.ImageName = vehicle.ImageName;
            if (_vehicle == null)
            {
                return NotFound();
            }

            return View(_vehicle);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle, VehicleView_Model _vehicle)
        {
            vehicle.ID = _vehicle.ID;
            vehicle.CarModel = _vehicle.CarModel;
            vehicle.CarBrand = _vehicle.CarBrand;
            vehicle.ProductionDate = _vehicle.ProductionDate;
            vehicle.Price = _vehicle.Price;
            vehicle.ImageName = _vehicle.ImageName;
            if (_vehicle.Image!=null)
            {
                var file = _vehicle.Image;
                if (file == null || file.Length == 0)
                {
                    return View(_vehicle);
                }
                vehicle.ImageName = file.FileName;
                var path = Path.Combine(
                       Directory.GetCurrentDirectory(), "wwwroot/Images",
                        file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }                    
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(_vehicle);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var cars = await _context.Vehicle.FindAsync(id);
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/Images",
                         cars.ImageName);
            System.IO.File.Delete(path);
            _context.Vehicle.Remove(cars);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.ID == id);
        }
    }
}
