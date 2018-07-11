using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectoApp.Data;
using ElectoApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElectoApp.Controllers
{
    public class ElectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ElectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Elections
        public async Task<IActionResult> Index()
        {
            var elect = await (from e in _context.ElectionTB orderby e.ElectionDate  select e).ToListAsync();
            return View(elect);
        }

        //Get upcoming presidential elections

            public async Task<IActionResult> GetPresidentElections()
        {
            

            var presi = await (from e in _context.ElectionTB orderby e.ElectionDate descending where e.ElectionType == Electoral.Presidential select e ).Take(5).ToListAsync();
            return PartialView("GetPresidentElections",presi);
        }

        //Get upcoming senatorial elections

        public async Task<IActionResult> GetSenateElections()
        {
           

           var senate = await (from e in _context.ElectionTB orderby e.ElectionDate descending where e.ElectionType == Electoral.Senatorial select e).Take(5).ToListAsync();
          
            return PartialView("_GetSenateElections", senate );
        }

        //Get upcoming Parliamentary elections

        public async Task<IActionResult> GetParliamentElections()
        {


            var parl = await (from e in _context.ElectionTB orderby e.ElectionDate  where e.ElectionType == Electoral.Parliamentary select e).Take(5).ToListAsync();

            return PartialView("GetParliamentElections", parl);
        }

        //Get upcoming Congress elections

        public async Task<IActionResult> GetCongressElections()
        {


            var cong = await (from e in _context.ElectionTB orderby e.ElectionDate descending where e.ElectionType == Electoral.Congressional select e).Take(5).ToListAsync();

            return PartialView("GetCongressElections", cong);
        }


        // GET: Elections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.ElectionTB
                .SingleOrDefaultAsync(m => m.ElectionId == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        // GET: Elections/Create
        [Authorize]
        public IActionResult Create()
        {

            var countryList = (from c in _context.CountryTB orderby c.CountryName select c).ToList();
            countryList.Insert(0, new Country { CountryId = 0, CountryName = "Select Country" });
            ViewBag.ListOfCountry = countryList;
            return View();
        }

        // POST: Elections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ElectionId,ElectionType,PreviousElectionDate,ElectionDate,CountryName, PublisherId")] Election election)
        {
            int CountriCode = Int32.Parse(election.CountryName);


            var newName = await _context.CountryTB.SingleOrDefaultAsync(m => m.CountryId == CountriCode);

            election.CountryName = newName.CountryName;


            election.CreatedDate = DateTime.Now;


            if (ModelState.IsValid)
            {
                _context.Add(election);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(election);
        }

        // GET: Elections/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.ElectionTB.SingleOrDefaultAsync(m => m.ElectionId == id);
            if (election == null)
            {
                return NotFound();
            }
            return View(election);
        }

        // POST: Elections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ElectionId,ElectionType,PreviousElectionDate,ElectionDate,CountryName,PublisherId")] Election election)
        {
            if (id != election.ElectionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(election);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionExists(election.ElectionId))
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
            return View(election);
        }

        // GET: Elections/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.ElectionTB
                .SingleOrDefaultAsync(m => m.ElectionId == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        // POST: Elections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var election = await _context.ElectionTB.SingleOrDefaultAsync(m => m.ElectionId == id);
            _context.ElectionTB.Remove(election);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectionExists(int id)
        {
            return _context.ElectionTB.Any(e => e.ElectionId == id);
        }
    }
}
