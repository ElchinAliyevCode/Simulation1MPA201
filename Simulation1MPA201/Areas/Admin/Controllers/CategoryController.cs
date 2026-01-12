using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation1MPA201.Contexts;
using Simulation1MPA201.Models;
using Simulation1MPA201.ViewModels.Category;

namespace Simulation1MPA201.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly SimulationDbContext _context;

    public CategoryController(SimulationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.Select(x => new CategoryGetVM()
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();

        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Category category = new Category()
        {
            Name = vm.Name
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

         _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Update(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        CategoryUpdateVM vm = new CategoryUpdateVM()
        {
            Id=category.Id,
            Name=category.Name
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CategoryUpdateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == vm.Id);
        if (category == null)
        {
            return NotFound();
        }

        category.Name = vm.Name;

        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
}
