using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5AspNetCoreEfIndividual.Data;
using Lab5AspNetCoreEfIndividual.Models;

namespace Lab5AspNetCoreEfIndividual.Controllers
{
    // Asynchronous programming is the default mode for ASP.NET Core and EF Core.
    // By default the Entity Framework implicitly implements transactions
    public class PatientsController : Controller
    {
        private readonly HospitalContext _context;

        public PatientsController(HospitalContext context)
        {
            _context = context;
        }

        //// GET: Patients
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Patients.ToListAsync());
        //}

        // Add sorting functionality
        // Add filtering functionality
        //public async Task<IActionResult> Index(
        //    string sortOrder,
        //    string currentFilter,
        //    string searchString,
        //    int? pageNumber)
        //{
        //    // this must be included in the paging links in order to keep the sort order the same while paging.
        //    ViewData["CurrentSort"] = sortOrder;
        //    ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewData["InsuranceSortParm"] = sortOrder == "Insurance" ? "ins_desc" : "Insurance";

        //    // If the search string is changed during paging, the page has to be reset to 1,
        //    // because the new filter can result in different data to display.
        //    if (searchString != null)
        //    {
        //        pageNumber = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    // This value must be included in the paging links in order to maintain the filter settings during paging,
        //    // and it must be restored to the text box when the page is redisplayed.
        //    ViewData["CurrentFilter"] = searchString;

        //    var patients = from p in _context.Patients
        //                   select p;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        patients = patients.Where(p => p.Name.Contains(searchString));
        //    }

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            patients = patients.OrderByDescending(p => p.Name);
        //            break;
        //        case "Insurance":
        //            patients = patients.OrderBy(p => p.InsuranceId);
        //            break;
        //        case "ins_desc":
        //            patients = patients.OrderByDescending(p => p.InsuranceId);
        //            break;
        //        default:
        //            patients = patients.OrderBy(p => p.Name);
        //            break;
        //    }

        //    int pageSize = 3;
        //    // The two question marks represent the null-coalescing operator.
        //    // The null-coalescing operator defines a default value for a nullable type;
        //    // the expression (pageNumber ?? 1) means return the value of pageNumber if it has a value,
        //    // or return 1 if pageNumber is null.
        //    return View(await PaginatedList<Patient>.CreateAsync(patients.AsNoTracking(), pageNumber ?? 1, pageSize));

        //    //return View(await patients.AsNoTracking().ToListAsync());
        //}

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "LastName_desc" : "";
            ViewData["InsuranceSortParm"] = sortOrder == "Insurance" ? "Insurance_desc" : "Insurance";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var patients = from p in _context.Patients select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.Name.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "Name";
            }

            bool descending = false;
            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                descending = true;
            }

            if (descending)
            {
                // We use OrderBy with property as string
                // Earlier we set the property manually
                // ```case "name_desc":
                //      patients = patients.OrderByDescending(p => p.Name);
                //      break; ```
                patients = patients.OrderByDescending(e => EF.Property<object>(e, sortOrder));
            }
            else
            {
                patients = patients.OrderBy(e => EF.Property<object>(e, sortOrder));
            }

            int pageSize = 3;
            return View(await PaginatedList<Patient>.CreateAsync(patients.AsNoTracking(),
                pageNumber ?? 1, pageSize));
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // The Include and ThenInclude methods cause the context to load the Patient.Consultations nav prop
            // and within each consultation the Consultation.Doctor nav prop.
            // The AsNoTracking improves performance in scenarios where the entities returned won't be updated in the current context's lifetime
            // In the tutorial SingleOrDefaultAsync selects up to 2 rows:
            // - If the query would return multiple rows, the method returns null.
            // - o determine whether the query would return multiple rows, EF has to check if it returns at least 2.
            var patient = await _context.Patients
                .Include(s => s.Consultations)
                    .ThenInclude(e => e.Doctor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id); 
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // The ValidateAntiForgeryToken attribute helps prevent cross-site request forgery (CSRF) attacks.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,InsuranceId,Diagnosis")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //
                ModelState.AddModelError("", "Unable to save changes.");
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        //// POST: Patients/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,PatientName,InsuranceId,Diagnosis")] Patient patient)
        //{
        //    if (id != patient.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(patient);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PatientExists(patient.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(patient);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientToUpdate = await _context.Patients.FirstOrDefaultAsync(p => p.ID == id);
            if (await TryUpdateModelAsync<Patient>(
                patientToUpdate,
                "",
                p => p.Name, p => p.InsuranceId, p => p.Diagnosis))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(patientToUpdate);
        }

        // GET: Patients/Delete/5
        // This code accepts an optional parameter that indicates whether the method was called after a failure to save changes.
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again.";
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // This code retrieves the selected entity
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // then calls the Remove method to set the entity's status to Deleted
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }
    }
}
