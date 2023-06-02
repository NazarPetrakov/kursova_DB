using System.Runtime.CompilerServices;
using kursova.Data;
using kursova.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kursova.Controllers
{
    public class CitizenController : Controller
    {
        private readonly KursovaContext _kursovaContext;

        public CitizenController(KursovaContext kursovaContext)
        {
            _kursovaContext = kursovaContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var citizens = await _kursovaContext.Citizens.ToListAsync();
            return View(citizens);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCitizenViewModel addCitizenRequest)
        {
            var citizen = new Citizen()
            {
                FirstName = addCitizenRequest.FirstName,
                LastName = addCitizenRequest.LastName,
                PhoneNumber = addCitizenRequest.PhoneNumber
            };
            if (!ModelState.IsValid) return View();
            await _kursovaContext.Citizens.AddAsync(citizen);
            await _kursovaContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var citizen = await _kursovaContext.Citizens.FirstOrDefaultAsync(x => x.Id == id)!;

            if (citizen == null) return RedirectToAction("Index");

            var viewModel = new UpdateCitizenViewModel()
            {
                Id = citizen.Id,
                FirstName = citizen.FirstName,
                LastName = citizen.LastName,
                PhoneNumber = citizen.PhoneNumber
            };
            return await Task.Run(() => View("View", viewModel));

        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateCitizenViewModel updateCitizenViewModel)
        {
            var citizen = await _kursovaContext.Citizens.FindAsync(updateCitizenViewModel.Id);

            if (citizen == null) return RedirectToAction("Index");

            citizen.Id = updateCitizenViewModel.Id;
            citizen.FirstName = updateCitizenViewModel.FirstName;
            citizen.LastName = updateCitizenViewModel.LastName;
            citizen.PhoneNumber = updateCitizenViewModel.PhoneNumber;

            if (!ModelState.IsValid) return View();
            await _kursovaContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateCitizenViewModel updateCitizenViewModel)
        {
            var citizen = await _kursovaContext.Citizens.FindAsync(updateCitizenViewModel.Id);

            if (citizen == null) return RedirectToAction("Index");

            var applications = await _kursovaContext.Applications.ToListAsync();
            var applicationsWithCitizen = from a in applications
                 where a.CitizenId == citizen.Id
                 select a;

             if (applicationsWithCitizen.Any())
             {
                 ModelState.AddModelError("HasRelatedApplications", "Delete applications with this citizen first");
                 return View("View");
             }

             _kursovaContext.Citizens.Remove(citizen);
             await _kursovaContext.SaveChangesAsync();
             return RedirectToAction("Index");
        }
    }
}
