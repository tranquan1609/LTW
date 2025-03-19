using System.ComponentModel.DataAnnotations;

namespace TranVuDienQuan_Buoi4.Models
{
    public class Category
    {
        //Khai báo các thuộc tính
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }

        //Danh sách sản phẩm
        public List<Product>? Products { get; set; }
    }

}
