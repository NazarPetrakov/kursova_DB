using kursova.Data;
using kursova.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Logging;

namespace kursova.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly KursovaContext _kursovaContext;
        public ApplicationController(KursovaContext kursovaContext)
        {
            _kursovaContext = kursovaContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var applications = await _kursovaContext.Applications.ToListAsync();
            return View(applications);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var citizens = await _kursovaContext.Citizens.ToListAsync();
            var citizenOptions = citizens.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Id} {c.FirstName} {c.LastName}"
            });
            ViewBag.CitizenOptions = citizenOptions;

            var specialists = await _kursovaContext.Specialists.ToListAsync();
            var specialistOptions = specialists.Select(s => new SelectListItem

            {
                Value = s.Id.ToString(),
                Text = $"{s.Id} {s.FirstName} {s.LastName} ({s.Specialty})"
            });
            ViewBag.SpecialistOptions = specialistOptions;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddApplicationViewModel addApplicationViewModel)
        {
            
            var application = new Application()
            {
                Date = addApplicationViewModel.Date,
                Content = addApplicationViewModel.Content,
                Status = "Not accepted",
                CitizenId = addApplicationViewModel.CitizenId,
                SpecialistId = addApplicationViewModel.SpecialistId,
                Citizen = await _kursovaContext.Citizens.FirstOrDefaultAsync(x => x.Id == addApplicationViewModel.CitizenId),
                Specialist = await _kursovaContext.Specialists.FirstOrDefaultAsync(x => x.Id == addApplicationViewModel.SpecialistId)
            };
            
            if (!ModelState.IsValid) return View();

            var citizen =
                (await _kursovaContext.Citizens.FirstOrDefaultAsync(x => x.Id == addApplicationViewModel.CitizenId))!;
            citizen.Applications.Add(application);

            var specialist =
                (await _kursovaContext.Specialists.FirstOrDefaultAsync(x => x.Id == addApplicationViewModel.SpecialistId))!;
            specialist.Applications.Add(application);


            await _kursovaContext.Applications.AddAsync(application);
            await _kursovaContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var application = await _kursovaContext.Applications.FirstOrDefaultAsync(x => x.Id == id)!;

            var citizens = await _kursovaContext.Citizens.ToListAsync();
            var citizenOptions = citizens.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Id} {c.FirstName} {c.LastName}"
            });
            ViewBag.CitizenOptions = citizenOptions;

            var specialists = await _kursovaContext.Specialists.ToListAsync();
            var specialistOptions = specialists.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"{s.Id} {s.FirstName} {s.LastName} ({s.Specialty})"
            });
            ViewBag.SpecialistOptions = specialistOptions;

            if (application == null) return RedirectToAction("Index");

            var viewModel = new UpdateApplicationViewModel()
            {
                Id = application.Id,
                Date = application.Date,
                Content = application.Content,
                Status = application.Status,
                CitizenId = application.CitizenId,
                SpecialistId = application.SpecialistId
            };
            return await Task.Run(() => View("View", viewModel));

        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateApplicationViewModel updateApplicationViewModel)
        {
            var application = await _kursovaContext.Applications.FindAsync(updateApplicationViewModel.Id);

            if (application == null) return RedirectToAction("Index");
            if (!ModelState.IsValid) return View();
            application.FillApplication(updateApplicationViewModel, _kursovaContext);
            if (application.Status == "Accepted")
            {
                var number = from a in _kursovaContext.Applications.ToList()
                             where a.Date!.Value.Date == application.Date!.Value.Date && a.SpecialistId == application.SpecialistId
                             select a.QueueNum;
                if (number.All(x => x == null))
                {
                    application.QueueNum = 1;
                }
                else
                {
                    application.QueueNum = (short?)(number.Max() + 1);
                }
            }
            
            await _kursovaContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateApplicationViewModel updateApplicationViewModel)
        {
            var application = await _kursovaContext.Applications.FindAsync(updateApplicationViewModel.Id);

            if (application == null) return RedirectToAction("Index");

            var citizen =
                (await _kursovaContext.Citizens.FirstOrDefaultAsync(x => x.Id == application!.CitizenId))!;
            citizen.Applications.Remove(application);

            var specialist =
                (await _kursovaContext.Specialists.FirstOrDefaultAsync(x => x.Id == application.SpecialistId))!;
            specialist.Applications.Remove(application);

            _kursovaContext.Applications.Remove(application);
            await _kursovaContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
