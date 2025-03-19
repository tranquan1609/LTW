using Microsoft.EntityFrameworkCore;
using TranVuDienQuan_Buoi4.Models;

namespace TranVuDienQuan_Buoi4.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// ớp EFProductRepository là một repository sử dụng Entity Framework Core để thao tác với bảng Products trong cơ sở dữ liệu.
        ///GetAllAsync(): Lấy danh sách tất cả sản phẩm kèm theo thông tin danh mục (Category).
        ///GetByIdAsync(int id): Lấy một sản phẩm theo id, bao gồm cả thông tin danh mục.
        ///AddAsync(Product product): Thêm một sản phẩm mới vào cơ sở dữ liệu.
        ///UpdateAsync(Product product): Cập nhật thông tin sản phẩm trong cơ sở dữ liệu.
        ///DeleteAsync(int id): Xóa một sản phẩm theo id
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {

            // return await _context.Products.ToListAsync();
            return await _context.Products.Include(p => p.Category) // Include thông tin về category
                .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
