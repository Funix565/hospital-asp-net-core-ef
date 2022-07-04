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
    public class DepartmentsController : Controller
    {
        private readonly HospitalContext _context;

        public DepartmentsController(HospitalContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var hospitalContext = _context.Departments.Include(d => d.Chief);
            return View(await hospitalContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Chief)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            //ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID");
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // //public async Task<IActionResult> Create([Bind("DepartmentID,Name,Budget,EstablishmentDate,DoctorID,RowVersion")] Department department)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,Name,Budget,EstablishmentDate,DoctorID,xmin")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", department.DoctorID);
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorName", department.DoctorID);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var department = await _context.Departments.FindAsync(id);
            var department = await _context.Departments
                .Include(i => i.Chief)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }
            //ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", department.DoctorID);
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorName", department.DoctorID);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("DepartmentID,Name,Budget,EstablishmentDate,DoctorID,RowVersion")] Department department)
        //{
        //    if (id != department.DepartmentID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(department);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DepartmentExists(department.DepartmentID))
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
        //    //ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", department.DoctorID);
        //    ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorName", department.DoctorID);
        //    return View(department);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int? id, byte[] rowVersion)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    // Try to read the department to be updated.
        //    var departmentToUpdate = await _context.Departments
        //        .Include(i => i.Chief)
        //        .FirstOrDefaultAsync(m => m.DepartmentID == id);

        //    // Case when department was deleted by another user
        //    if (departmentToUpdate == null)
        //    {
        //        // Here we create a Department in order to redisplay it with an error message
        //        Department deletedDepartment = new Department();
        //        await TryUpdateModelAsync(deletedDepartment);
        //        ModelState.AddModelError(string.Empty,
        //            "Unable to save changes. The department was deleted by another user.");
        //        ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorName", deletedDepartment.DoctorID);
        //        return View(deletedDepartment);
        //    }

        //    _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = rowVersion;

        //    if (await TryUpdateModelAsync<Department>(
        //        departmentToUpdate,
        //        "",
        //        s => s.Name, s => s.EstablishmentDate, s => s.Budget, s => s.DoctorID))
        //    {
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateConcurrencyException ex)
        //        {
        //            var exceptionEntry = ex.Entries.Single();
        //            var clientValues = (Department)exceptionEntry.Entity;
        //            var databaseEntry = exceptionEntry.GetDatabaseValues();
        //            if (databaseEntry == null)
        //            {
        //                ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
        //            }
        //            else
        //            {
        //                var databaseValues = (Department)databaseEntry.ToObject();

        //                if (databaseValues.Name != clientValues.Name)
        //                {
        //                    ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
        //                }
        //                if (databaseValues.Budget != clientValues.Budget)
        //                {
        //                    ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}");
        //                }
        //                if (databaseValues.EstablishmentDate != clientValues.EstablishmentDate)
        //                {
        //                    ModelState.AddModelError("EstablishmentDate", $"Current value: {databaseValues.EstablishmentDate:d}");
        //                }
        //                if (databaseValues.DoctorID != clientValues.DoctorID)
        //                {
        //                    Doctor databaseDoctor = await _context.Doctors.FirstOrDefaultAsync(i => i.DoctorID == databaseValues.DoctorID);
        //                    ModelState.AddModelError("DoctorID", $"Current value: {databaseDoctor?.DoctorName}");
        //                }

        //                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
        //                    + "was modified by another user after you got the original value. The "
        //                    + "edit operation was canceled and the current values in the database "
        //                    + "have been displayed. If you still want to edit this record, click "
        //                    + "the Save button again. Otherwise click the Back to List hyperlink.");

        //                // The code sets the RowVersion value of the departmentToUpdate to the new value
        //                // retrieved from the database. This new RowVersion value will be stored
        //                // in the hidden field when the Edit page is redisplayed, and the next time
        //                // the user clicks Save, only concurrency errors that happen since
        //                // the redisplay of the Edit page will be caught.
        //                departmentToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
        //                ModelState.Remove("RowVersion");
        //            }
        //        }
        //    }
        //    ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorName", departmentToUpdate.DoctorID);
        //    return View(departmentToUpdate);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, uint xMin)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Try to read the department to be updated.
            var departmentToUpdate = await _context.Departments
                .Include(i => i.Chief)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            // Case when department was deleted by another user
            if (departmentToUpdate == null)
            {
                // Here we create a Department in order to redisplay it with an error message
                Department deletedDepartment = new Department();
                await TryUpdateModelAsync(deletedDepartment);
                ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
                ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorName", deletedDepartment.DoctorID);
                return View(deletedDepartment);
            }

            _context.Entry(departmentToUpdate).Property("xmin").OriginalValue = xMin;

            if (await TryUpdateModelAsync<Department>(
                departmentToUpdate,
                "",
                s => s.Name, s => s.EstablishmentDate, s => s.Budget, s => s.DoctorID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Department)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                        }
                        if (databaseValues.Budget != clientValues.Budget)
                        {
                            ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}");
                        }
                        if (databaseValues.EstablishmentDate != clientValues.EstablishmentDate)
                        {
                            ModelState.AddModelError("EstablishmentDate", $"Current value: {databaseValues.EstablishmentDate:d}");
                        }
                        if (databaseValues.DoctorID != clientValues.DoctorID)
                        {
                            Doctor databaseDoctor = await _context.Doctors.FirstOrDefaultAsync(i => i.DoctorID == databaseValues.DoctorID);
                            ModelState.AddModelError("DoctorID", $"Current value: {databaseDoctor?.DoctorName}");
                        }

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");

                        // The code sets the XMIN value of the departmentToUpdate to the new value
                        // retrieved from the database. This new XMIN value will be stored
                        // in the hidden field when the Edit page is redisplayed, and the next time
                        // the user clicks Save, only concurrency errors that happen since
                        // the redisplay of the Edit page will be caught.
                        departmentToUpdate.xmin = (uint)databaseValues.xmin;
                        ModelState.Remove("xmin");
                    }
                }
            }

            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorName", departmentToUpdate.DoctorID);
            return View(departmentToUpdate);

        }

        // GET: Departments/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var department = await _context.Departments
        //        .Include(d => d.Chief)
        //        .FirstOrDefaultAsync(m => m.DepartmentID == id);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(department);
        //}

        // The method accepts an optional parameter that indicates whether the page is being redisplayed after a concurrency error.
        // If this flag is true and the department specified no longer exists, it was deleted by another user.
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Chief)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            
            // The entity was deleted by another user
            // The code redirects to the Index page
            if (department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }

            // The department does exist, it was changed by another user
            // The code sends an error message to the view using ViewData
            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var department = await _context.Departments.FindAsync(id);
        //    _context.Departments.Remove(department);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department)
        {
            try
            {
                // If the department is already deleted, the AnyAsync method returns false
                // and the application just goes back to the Index method.
                if (await _context.Departments.AnyAsync(m => m.DepartmentID == department.DepartmentID))
                {
                    _context.Departments.Remove(department);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            // If a concurrency error is caught, the code redisplays the Delete confirmation page
            // and provides a flag that indicates it should display a concurrency error message.
            catch (DbUpdateConcurrencyException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = department.DepartmentID });
            }
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }
    }
}
