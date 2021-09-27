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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicle.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cars == null)
            {
                return NotFound();
            }

            return View(cars);
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
        public async Task<IActionResult> Create([Bind("CarBrand,CarModel,ProductionDate,Price,ImageName")] Vehicle vehicle,VehicleView_Model _vehicle)
        {
            _vehicle.CarBrand = vehicle.CarBrand;
            _vehicle.CarModel = vehicle.CarModel;
            _vehicle.ProductionDate = vehicle.ProductionDate;
            _vehicle.Price = vehicle.Price;
            var file = _vehicle.Image;
            if (file == null || file.Length== 0)
                return Content("file not selected");

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
            return View(vehicle);
        }
     
        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Vehicle.FindAsync(id);
            if (cars == null)
            {
                return NotFound();
            }
            return View(cars);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CarBrand,CarModel,ProductionDate,Price")] Vehicle vehicle)
        {
            if (id != vehicle.ID)
            {
                return NotFound();
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
            return View(vehicle);
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
