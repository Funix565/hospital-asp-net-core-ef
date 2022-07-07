using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5AspNetCoreEfIndividual.Data;
using Lab5AspNetCoreEfIndividual.Models;
using Lab5AspNetCoreEfIndividual.Models.HospitalViewModels;

namespace Lab5AspNetCoreEfIndividual.Controllers
{
    public class TreatmentsController : Controller
    {
        private readonly HospitalContext _context;

        public TreatmentsController(HospitalContext context)
        {
            _context = context;
        }

        //// GET: Treatments
        //public async Task<IActionResult> Index()
        //{
        //    var hospitalContext = _context.Treatments.Include(t => t.Department);
        //    return View(await hospitalContext.ToListAsync());
        //}

        // The method accepts optional route data (id) and a query string parameter (courseID)
        // that provide the ID values of the selected doctor and selected treatment.
        // The parameters are provided by the Select hyperlinks on the page.
        public async Task<IActionResult> Index(int? id, int? doctorID)
        {
            // Eager loading for entities
            // Get TreatmentContraindication, TreatmentAssignments
            // Within the TreatmentAssignments property, Doctor property is loaded
            // And within that, the Consultations property are loaded
            // And within each Consultation the Patient is loaded
            // This is including multiple levels of related data
            var viewModel = new TreatmentIndexData();
            viewModel.Treatments = await _context.Treatments
                .Include(t => t.TreatmentContraindication)
                .Include(t => t.Department)
                .Include(t => t.TreatmentAssignments)
                    .ThenInclude(t => t.Doctor)
                        .ThenInclude(t => t.Consultations)
                            .ThenInclude(t => t.Patient)
                            // At that point in the code, another ThenInclude would be for navigation properties of Patient
                        // But calling Include starts over with Treatment properties
                //.Include(t => t.TreatmentAssignments)
                //.ThenInclude(t => t.Doctor)
                .AsNoTracking()
                .OrderBy(t => t.TreatmentTitle)
                .ToListAsync();

            // executes when a treatment was selected
            if (id != null)
            {
                ViewData["TreatmentID"] = id.Value;
                Treatment treatment = viewModel.Treatments.Where(
                    t => t.ID == id.Value).Single();
                viewModel.Doctors = treatment.TreatmentAssignments.Select(s => s.Doctor);
            }

            // executes when a doctor was selected
            if (doctorID != null)
            {
                ViewData["DoctorID"] = doctorID.Value;
                viewModel.Consultations = viewModel.Doctors.Where(
                    x => x.ID == doctorID).Single().Consultations;
            }

            return View(viewModel);
        }

        // GET: Treatments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments
                .Include(t => t.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        //// GET: Treatments/Create
        //public IActionResult Create()
        //{
        //    ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID");
        //    return View();
        //}

        public IActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: Treatments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,TreatmentTitle,Definition,RoomNumber,DepartmentID")] Treatment treatment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(treatment);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", treatment.DepartmentID);
        //    return View(treatment);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TreatmentTitle,Definition,RoomNumber,DepartmentID,TreatmentContraindication")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDepartmentsDropDownList(treatment.DepartmentID);
            return View(treatment);
        }

        //// GET: Treatments/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var treatment = await _context.Treatments.FindAsync(id);
        //    if (treatment == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", treatment.DepartmentID);
        //    return View(treatment);
        //}

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments
                .Include(t => t.TreatmentContraindication)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (treatment == null)
            {
                return NotFound();
            }

            PopulateDepartmentsDropDownList(treatment.DepartmentID);
            return View(treatment);
        }

        //// POST: Treatments/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,TreatmentTitle,Definition,RoomNumber,DepartmentID")] Treatment treatment)
        //{
        //    if (id != treatment.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(treatment);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TreatmentExists(treatment.ID))
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
        //    ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", treatment.DepartmentID);
        //    return View(treatment);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentToUpdate = await _context.Treatments
                .Include(t => t.TreatmentContraindication)
                .FirstOrDefaultAsync(t => t.ID == id);

            if (await TryUpdateModelAsync<Treatment>(
                treatmentToUpdate,
                "",
                t => t.TreatmentTitle, t => t.Definition, t => t.RoomNumber, t => t.DepartmentID,
                t => t.TreatmentContraindication))
            {
                // If the overview is blank, set the property to null.
                // The related row in the TreatmentContraindication will be deleted.
                if (String.IsNullOrWhiteSpace(treatmentToUpdate.TreatmentContraindication?.Overview))
                {
                    treatmentToUpdate.TreatmentContraindication = null;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */ )
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDepartmentsDropDownList(treatmentToUpdate.DepartmentID);
            return View(treatmentToUpdate);
        }


        // The method accepts the optional parameter that allows the calling code
        // to specify the item that will be selected when the drop-down list is rendered.
        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in _context.Departments
                                   orderby d.Name
                                   select d;
            
            ViewBag.DepartmentID = new SelectList(departmentsQuery.AsNoTracking(), "DepartmentID", "Name", selectedDepartment);
        }

        // GET: Treatments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments
                .Include(t => t.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        // POST: Treatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _context.Treatments.FindAsync(id);
            _context.Treatments.Remove(treatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentExists(int id)
        {
            return _context.Treatments.Any(e => e.ID == id);
        }
    }
}
