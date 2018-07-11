using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectoApp.Data;
using ElectoApp.Models;

namespace ElectoApp.Controllers
{
    public class CapitalCitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CapitalCitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CapitalCities
        public async Task<IActionResult> Index()
        {
            return View(await _context.CapitalCityTB.ToListAsync());
        }

        // GET: CapitalCities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capitalCity = await _context.CapitalCityTB
                .SingleOrDefaultAsync(m => m.CapitalCityId == id);
            if (capitalCity == null)
            {
                return NotFound();
            }

            return View(capitalCity);
        }

        // GET: CapitalCities/Create
        public IActionResult Create()
        {

            var countryList = (from c in _context.CountryTB orderby c.CountryName select c).ToList();
            countryList.Insert(0, new Country { CountryId = 0, CountryName = "Select Country" });
            ViewBag.ListOfCountry = countryList;
            return View();
        }

        // POST: CapitalCities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CapitalCityId,CapitalCityName,CapCountry")] CapitalCity capitalCity)
        {
            int CountryCode = Int32.Parse(capitalCity.CapCountry);


            var newName = await _context.CountryTB.SingleOrDefaultAsync(m => m.CountryId == CountryCode);

            capitalCity.CapCountry = newName.CountryName;

            if (ModelState.IsValid)
            {
                _context.Add(capitalCity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(capitalCity);
        }

        // GET: CapitalCities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capitalCity = await _context.CapitalCityTB.SingleOrDefaultAsync(m => m.CapitalCityId == id);
            if (capitalCity == null)
            {
                return NotFound();
            }
            return View(capitalCity);
        }

        // POST: CapitalCities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CapitalCityId,CapitalCityName,CapCountry")] CapitalCity capitalCity)
        {
            if (id != capitalCity.CapitalCityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(capitalCity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapitalCityExists(capitalCity.CapitalCityId))
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
            return View(capitalCity);
        }

        // GET: CapitalCities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capitalCity = await _context.CapitalCityTB
                .SingleOrDefaultAsync(m => m.CapitalCityId == id);
            if (capitalCity == null)
            {
                return NotFound();
            }

            return View(capitalCity);
        }

        // POST: CapitalCities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var capitalCity = await _context.CapitalCityTB.SingleOrDefaultAsync(m => m.CapitalCityId == id);
            _context.CapitalCityTB.Remove(capitalCity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CapitalCityExists(int id)
        {
            return _context.CapitalCityTB.Any(e => e.CapitalCityId == id);
        }
    }
}
