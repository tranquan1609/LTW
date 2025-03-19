using Microsoft.EntityFrameworkCore;
using TranVuDienQuan_Buoi4.Models;

namespace TranVuDienQuan_Buoi4.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Lớp EFCategoryRepository là một repository sử dụng Entity Framework Core để thao tác với bảng Categories trong cơ sở dữ liệu.
        /*
        GetAllAsync() : Lấy danh sách tất cả danh mục.
        GetByIdAsync(int id): Lấy một danh mục theo id.
        AddAsync(Category category): Thêm một danh mục mới vào cơ sở dữ liệu.
        UpdateAsync(Category category): Cập nhật thông tin danh mục trong cơ sở dữ liệu.
        DeleteAsync(int id): Xóa một danh mục theo id.
        */
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var cate = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(cate);
            await _context.SaveChangesAsync();
        }
    }
}
