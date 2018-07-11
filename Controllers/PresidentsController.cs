using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectoApp.Data;
using ElectoApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace ElectoApp.Controllers
{
    public class PresidentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PresidentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Presidents
        public async Task<IActionResult> Index()
        {
            return View(await _context.PresidentTB.ToListAsync());
        }

        //Get longest serving president
        public async Task<IActionResult> GetLongestPresidents()
        {


            var longest = await (from e in _context.PresidentTB orderby e.AssumedOffice  select e).Take(3).ToListAsync();
            return PartialView("GetLongestPresidents", longest);
        }

        //Get recent elected president
        public async Task<IActionResult> GetRecentPresidents()
        {


            var recent = await (from e in _context.PresidentTB orderby e.AssumedOffice descending  select e).Take(4).ToListAsync();
            return PartialView("GetRecentPresidents", recent);
        }




        // GET: Presidents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var president = await _context.PresidentTB
                .SingleOrDefaultAsync(m => m.PresidentId == id);
            if (president == null)
            {
                return NotFound();
            }

            return View(president);
        }

        // GET: Presidents/Create
        [Authorize]
        public IActionResult Create()
        {
           

            var countryList = (from c in _context.CountryTB orderby c.CountryName select c).ToList();
            countryList.Insert(0, new Country { CountryId = 0, CountryName = "Select Country" });
            ViewBag.ListOfCountry = countryList;

            var capitalList = (from c in _context.CapitalCityTB orderby c.CapitalCityName select c).ToList();
            capitalList.Insert(0, new CapitalCity { CapitalCityId = 0, CapitalCityName = "Select Capital" });
            ViewBag.ListOfCapital = capitalList;

            var continentList = (from c in _context.ContinentTB orderby c.ContinentName select c).ToList();
            continentList.Insert(0, new Continent { ContinentId = 0, ContinentName = "Select Continent" });
            ViewBag.ListOfContinent = continentList;



            return View();
        }

        // POST: Presidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(President president, IFormFile Photo)
        {
            if (Photo != null && Photo.Length > 0)
            {
                Stream stream = Photo.OpenReadStream();
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    president.Photo = memoryStream.ToArray();
                }

            }

            int CountriCode = Int32.Parse(president.PresCountry);
            


            var newContri = await _context.CountryTB.SingleOrDefaultAsync(m => m.CountryId == CountriCode);
            var newCapi = await _context.CapitalCityTB.SingleOrDefaultAsync(m => m.CapCountry == newContri.CountryName);



            president.PresCountry = newContri.CountryName;
            president.PresContinent = newContri.CounContinent;
            president.PresCapital = newCapi.CapitalCityName;


            president.CreatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(president);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(president);
        }

        // GET: Presidents/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var president = await _context.PresidentTB.SingleOrDefaultAsync(m => m.PresidentId == id);
            if (president == null)
            {
                return NotFound();
            }
            return View(president);
        }

        // POST: Presidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("PresidentId,FullName,AssumedOffice,OfficeTerms,PrecededBy,Photo,PresCountry,PresCapital,PresContinent,PoliticalParty,Publisher")] President president)
        {
            if (id != president.PresidentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(president);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresidentExists(president.PresidentId))
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
            return View(president);
        }

        // GET: Presidents/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var president = await _context.PresidentTB
                .SingleOrDefaultAsync(m => m.PresidentId == id);
            if (president == null)
            {
                return NotFound();
            }

            return View(president);
        }

        // POST: Presidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var president = await _context.PresidentTB.SingleOrDefaultAsync(m => m.PresidentId == id);
            _context.PresidentTB.Remove(president);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PresidentExists(int id)
        {
            return _context.PresidentTB.Any(e => e.PresidentId == id);
        }


        [Authorize]
        public JsonResult GetContinent(int CountryId)
        {

          var  continentList = (from c in _context.ContinentTB join 
                                k in _context.CountryTB on c.ContinentName equals k.CounContinent
                                where k.CountryId==CountryId
                                select c).ToList();

            continentList.Insert(0, new Continent { ContinentId = 0, ContinentName = "Select Continent" });
            return Json(new SelectList(continentList, "ContinentId", "ContinentName"));
        }

        [Authorize]
        public JsonResult GetCapital(int CountryId)
        {

            var Countri = _context.CountryTB.First(e => e.CountryId == CountryId);
            var newName = Countri.CountryName;
            var capitalList = (from c in _context.CapitalCityTB
                               
                                 where c.CapCountry == newName
                                 select c).ToList();

            capitalList.Insert(0, new CapitalCity { CapitalCityId = 0, CapitalCityName = "Select Capital City" });
            return Json(new SelectList(capitalList, "CapitalCityId", "CapitalCityName"));
        }

    }
}
