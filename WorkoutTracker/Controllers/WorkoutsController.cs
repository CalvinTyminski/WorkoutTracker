using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WorkoutTracker.Controllers
{
    [Authorize]
    public class WorkoutsController : Controller
    {
        private readonly WorkoutContext _context;

        public WorkoutsController(WorkoutContext context)
        {
            _context = context;
        }

 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var workout = await _context.Workouts
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

       
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Date,DurationMinutes")] Workout workout)
        //{
        //    workout.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(workout);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View(workout);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Workout workout)
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Content("User is not logged in");
            }

            workout.UserId = userId;

            if (ModelState.IsValid)
            {
                _context.Add(workout);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(workout);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }
            return View(workout);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Workout workout)
        {
            if (id != workout.Id)
            {
                return NotFound();
            }

            var existingWorkout = await _context.Workouts.FindAsync(id);

            if (existingWorkout == null)
            {
                return NotFound();
            }

            existingWorkout.Name = workout.Name;
            existingWorkout.Date = workout.Date;
            existingWorkout.DurationMinutes = workout.DurationMinutes;

            await _context.SaveChangesAsync();
                   
            return RedirectToAction("Index", "Home");
        }

        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(e => e.Id == id);
        }
    }
}
