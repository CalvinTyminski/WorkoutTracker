using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly WorkoutContext _context;

        public ExercisesController(WorkoutContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var workoutContext = _context.Exercises.Include(e => e.Workout);
            return View(await workoutContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.Workout)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        [HttpGet]
        public IActionResult Create(int workoutId)
        {
            var exercise = new Exercise { WorkoutId = workoutId };
            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Workouts", new {id =exercise.WorkoutId});
            }
            return View(exercise);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            ViewData["WorkoutId"] = new SelectList(_context.Workouts, "Id", "Name", exercise.WorkoutId);
            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkoutId"] = new SelectList(_context.Workouts, "Id", "Name", exercise.WorkoutId);
            return View(exercise);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.Workout)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
