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
    public class DoctorsController : Controller
    {
        private readonly HospitalContext _context;

        public DoctorsController(HospitalContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Doctors.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            // Probably, we make new Doctor because ViewData["Treatments"] should be assigned
            var doctor = new Doctor();
            doctor.TreatmentAssignments = new List<TreatmentAssignment>();
            PopulateAssignedTreatmentData(doctor);
            return View();
        }

        //// POST: Doctors/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("DoctorID,DoctorName,JobTitle")] Doctor doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(doctor);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(doctor);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,JobTitle")] Doctor doctor, string[] selectedTreatments)
        {
            if (selectedTreatments != null)
            {
                // Treatments are added even if there are model errors so that when there are model errors,
                // and the page is redisplayed with an error message,
                // any course selections that were made are automatically restored.
                doctor.TreatmentAssignments = new List<TreatmentAssignment>();
                foreach (var treatment in selectedTreatments)
                {
                    var treatmentToAdd = new TreatmentAssignment { DoctorID = doctor.ID, TreatmentID = int.Parse(treatment) };
                    doctor.TreatmentAssignments.Add(treatmentToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedTreatmentData(doctor);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var doctor = await _context.Doctors.FindAsync(id);
            // TODO: What about Consultations?
            // TODO: Test removes. Errors, orphans?
            var doctor = await _context.Doctors
                .Include(d => d.TreatmentAssignments).ThenInclude(d => d.Treatment)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (doctor == null)
            {
                return NotFound();
            }
            PopulateAssignedTreatmentData(doctor);
            return View(doctor);
        }

        // Provides information for the checkbox array
        private void PopulateAssignedTreatmentData(Doctor doctor)
        {
            var allTreatments = _context.Treatments;
            var doctorTreatments = new HashSet<int>(doctor.TreatmentAssignments.Select(t => t.TreatmentID));
            var viewModel = new List<AssignedTreatmentData>();
            foreach (var treatment in allTreatments)
            {
                viewModel.Add(new AssignedTreatmentData
                {
                    TreatmentID = treatment.ID,
                    Title = treatment.TreatmentTitle,
                    // The view will use this property to determine which checkboxes must be displayed as selected
                    Assigned = doctorTreatments.Contains(treatment.ID)
                });
            }
            ViewData["Treatments"] = viewModel;
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("DoctorID,DoctorName,JobTitle")] Doctor doctor)
        //{
        //    if (id != doctor.DoctorID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(doctor);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DoctorExists(doctor.DoctorID))
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
        //    return View(doctor);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedTreatments)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorToUpdate = await _context.Doctors
                .Include(d => d.TreatmentAssignments)
                    .ThenInclude(d => d.Treatment)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Doctor>(
                doctorToUpdate,
                "",
                d => d.Name, d => d.JobTitle))
            {
                UpdateDoctorTreatments(selectedTreatments, doctorToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateDoctorTreatments(selectedTreatments, doctorToUpdate);
            PopulateAssignedTreatmentData(doctorToUpdate);
            return View(doctorToUpdate);
        }

        private void UpdateDoctorTreatments(string[] selectedTreatments, Doctor doctorToUpdate)
        {
            // If no checkboxes were selected, the code initializes
            // the navigation property with an empty collection and returns.
            if (selectedTreatments == null)
            {
                doctorToUpdate.TreatmentAssignments = new List<TreatmentAssignment>();
                return;
            }

            // The code then loops through all courses in the database
            // and checks each course against the ones currently assigned to the instructor
            // versus the ones that were selected in the view.
            var selectedTreatmentsHS = new HashSet<string>(selectedTreatments);
            var doctorTreatments = new HashSet<int>
                (doctorToUpdate.TreatmentAssignments.Select(t => t.Treatment.ID));
            foreach (var treatment in _context.Treatments)
            {
                // If the checkbox for a treatment was selected
                // but the treatment isn't in the navigation property,
                // the treatment is added to the collection in the navigation property.
                if (selectedTreatmentsHS.Contains(treatment.ID.ToString()))
                {
                    if (!doctorTreatments.Contains(treatment.ID))
                    {
                        doctorToUpdate.TreatmentAssignments.Add(new TreatmentAssignment { DoctorID = doctorToUpdate.ID, TreatmentID = treatment.ID });
                    }
                }
                else
                {
                    // If the checkbox for a treatment wasn't selected,
                    // but the course is in the navigation property,
                    // the treatment is removed from the navigation property.
                    if (doctorTreatments.Contains(treatment.ID))
                    {
                        TreatmentAssignment treatmentToRemove = doctorToUpdate.TreatmentAssignments.FirstOrDefault(t => t.TreatmentID == treatment.ID);
                        _context.Remove(treatmentToRemove);
                    }
                }
            }
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        //// POST: Doctors/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var doctor = await _context.Doctors.FindAsync(id);
        //    _context.Doctors.Remove(doctor);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // We have to include TreatmentAssignments
            // or EF won't know about them and won't delete them.
            // Could be configured with cascade delete in the database.
            Doctor doctor = await _context.Doctors
                .Include(d => d.TreatmentAssignments)
                .SingleAsync(d => d.ID == id);

            // Remove the doctor from departments
            var departments = await _context.Departments
                .Where(d => d.DoctorID == id)
                .ToListAsync();
            departments.ForEach(d => d.DoctorID = null);

            _context.Doctors.Remove(doctor);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.ID == id);
        }
    }
}
