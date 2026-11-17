using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductInventory.Entities.Entities
{
    public class Categories
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int parentCategoryId { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
