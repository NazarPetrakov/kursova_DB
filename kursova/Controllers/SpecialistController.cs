using kursova.Data;
using kursova.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kursova.Controllers
{
    public class SpecialistController : Controller
    {
        private readonly KursovaContext _kursovaContext;

        public SpecialistController(KursovaContext kursovaContext)
        {
            _kursovaContext = kursovaContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var specialists = await _kursovaContext.Specialists.ToListAsync();
            return View(specialists);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSpecialistViewModel addSpecialist)
        {

            var specialist = new Specialist()
            {
                FirstName = addSpecialist.FirstName,
                LastName = addSpecialist.LastName,
                Specialty = addSpecialist.Specialty
            };
            if (!ModelState.IsValid) return View();
            await _kursovaContext.Specialists.AddAsync(specialist);
            await _kursovaContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var specialist = await _kursovaContext.Specialists.FirstOrDefaultAsync(x => x.Id == id);

            if (specialist == null) return RedirectToAction("Index");

            var viewModel = new UpdateSpecialistViewModel()
            {
                Id = specialist.Id,
                FirstName = specialist.FirstName,
                LastName = specialist.LastName,
                Specialty = specialist.Specialty
            };
            return await Task.Run(() => View("View", viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateSpecialistViewModel updateSpecialistViewModel)
        {
            var specialist = await _kursovaContext.Specialists.FindAsync(updateSpecialistViewModel.Id);

            if (specialist == null) return RedirectToAction("Index");

            specialist.Id = updateSpecialistViewModel.Id;
            specialist.FirstName = updateSpecialistViewModel.FirstName;
            specialist.LastName = updateSpecialistViewModel.LastName;
            specialist.Specialty = updateSpecialistViewModel.Specialty;


            if (!ModelState.IsValid) return View();
            await _kursovaContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateSpecialistViewModel updateSpecialistViewModel)
        {
            var specialist = await _kursovaContext.Specialists.FindAsync(updateSpecialistViewModel.Id);

            if (specialist == null) return RedirectToAction("Index");

            var applications = await _kursovaContext.Applications.ToListAsync();
            var applicationsWithSpecialist = from a in applications
                where a.SpecialistId == specialist.Id
                select a;

            if (applicationsWithSpecialist.Any())
            {
                ModelState.AddModelError("HasRelatedApplications", "Delete applications with this specialist first");
                return View("View");
            }

            _kursovaContext.Specialists.Remove(specialist);
            await _kursovaContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
