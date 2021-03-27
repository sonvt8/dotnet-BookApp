using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    [Table("Books")]
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Description { get; set; }
        public AppUsers AppUsers { get; set; }
        public int AppUsersId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
