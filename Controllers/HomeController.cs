﻿using Lab5AspNetCoreEfIndividual.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Lab5AspNetCoreEfIndividual.Data;
using Lab5AspNetCoreEfIndividual.Models.HospitalViewModels;

namespace Lab5AspNetCoreEfIndividual.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HospitalContext _context;

        public HomeController(ILogger<HomeController> logger, HospitalContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Group patients by Diagnosis
        public async Task<ActionResult> About()
        {
            // The LINQ statement groups the patient entities by diagnosis,
            // calculates the number of entities in each group,
            // and stores the results in a collection of PatientDiagnosisGroup view model objects.
            IQueryable<PatientDiagnosisGroup> data =
                from patient in _context.Patients
                group patient by patient.Diagnosis into diagnosisGroup
                select new PatientDiagnosisGroup()
                {
                    Diagnosis = diagnosisGroup.Key,
                    PatientCount = diagnosisGroup.Count()
                };
            return View(await data.AsNoTracking().ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
