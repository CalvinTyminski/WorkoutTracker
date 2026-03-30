using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly WorkoutContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(WorkoutContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            var workouts = await _context.Workouts
                .Where(w => w.UserId == userId)
                .Include(w => w.Exercises)
                .ToListAsync();

            var volumeData = workouts
                .GroupBy(w => w.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Volume = g.SelectMany(w => w.Exercises)
                .Sum(e => e.Sets * e.Reps * e.Weight)
                })
                .OrderBy(d => d.Date)
                .ToList();

            var durationData = workouts
                .GroupBy(w => w.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Duration = g.Sum(w => w.DurationMinutes)
                })
                .OrderBy(d => d.Date)
                .ToList();

            var personalRecords = workouts
                .SelectMany(w => w.Exercises)
                .GroupBy(e => e.Name)
                .Select(g => new
                {
                    Exercise = g.Key,
                    MaxWeight = g.Max(w => w.Weight)
                })
                .ToList();

            ViewBag.Workouts = workouts;

            ViewBag.Dates = volumeData.Select(x => x.Date.ToString("yyyy-MM-dd")).ToList();
            ViewBag.Volume = volumeData.Select(x => x.Volume).ToList();
            ViewBag.DurationDates = durationData.Select(x => x.Date.ToString("yyyy-MM-dd")).ToList();
            ViewBag.Duration = durationData.Select(x => x.Duration).ToList();
            ViewBag.PersonalRecords = personalRecords;


            return View(workouts);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
