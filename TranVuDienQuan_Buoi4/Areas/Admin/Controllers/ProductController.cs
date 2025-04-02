using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TranVuDienQuan_Buoi4.Models;
using TranVuDienQuan_Buoi4.Repositories;
using System.IO;
using System.Threading.Tasks;

namespace TranVuDienQuan_Buoi4.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // Bảo vệ toàn bộ controller
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        // Xem chi tiết sản phẩm
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee + "," + SD.Role_Customer)]
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // Thêm sản phẩm - GET
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // Thêm sản phẩm - POST
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    var imageSavePath = await SaveImage(imageUrl);
                    if (string.IsNullOrEmpty(imageSavePath))
                    {
                        ModelState.AddModelError("ImageUrl", "Lỗi khi tải ảnh lên.");
                        ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
                        return View(product);
                    }
                    product.ImageUrl = imageSavePath;
                }
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View(product);
        }

        // Lưu ảnh vào wwwroot
        private async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                var filePath = Path.Combine("wwwroot/images", image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                return "/images/" + image.FileName;
            }
            catch
            {
                return null; // Trả về null nếu có lỗi khi lưu ảnh
            }
        }

        // Cập nhật sản phẩm - GET
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // Cập nhật sản phẩm - POST
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        {
            if (id != product.Id) return NotFound();

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return NotFound();

            // Loại bỏ validation ImageUrl khi không có ảnh mới
            ModelState.Remove("ImageUrl");

            if (await TryUpdateModelAsync(existingProduct))
            {
                if (imageUrl != null)
                {
                    var imageSavePath = await SaveImage(imageUrl);
                    if (string.IsNullOrEmpty(imageSavePath))
                    {
                        ModelState.AddModelError("ImageUrl", "Lỗi khi tải ảnh lên.");
                        ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
                        return View(product);
                    }
                    existingProduct.ImageUrl = imageSavePath;
                }

                await _productRepository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View(product);
        }

        // Xóa sản phẩm - GET
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // Xóa sản phẩm - POST
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
