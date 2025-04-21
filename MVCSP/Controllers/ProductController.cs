using Microsoft.AspNetCore.Mvc;
using MVCSP.Interface;
using MVCSP.Models;

namespace MVCSP.Controllers
{
    public class ProductController : Controller
    {
        public readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (data, totalCount) = await _productService.GetPaginatedData(1, 10, "");
            var model = new ProductListModel
            {
                Products = data.ToList(),
                TotalCount = totalCount,
                CurrentPage = 1,
                PageSize = 10,
                Search = ""
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedPartial(int page = 1, int pageSize = 10, string search = "")
        {
            var (data, totalCount) = await _productService.GetPaginatedData(page, pageSize, search);

            var model = new ProductListModel
            {
                Products = data.ToList(),
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                Search = search
            };

            return PartialView("_ProductList", model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            await _productService.AddAsync(data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _productService.GetByIdAsync(id);
            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            await _productService.UpdateAsync(data);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }

    }
}
