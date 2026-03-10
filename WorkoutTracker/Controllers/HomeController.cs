using Microsoft.AspNetCore.Authorization;
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

        public HomeController(WorkoutContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            var workouts = await _context.Workouts
                .Where(w => w.UserId == userId)
                .ToListAsync();
            return View(workouts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
