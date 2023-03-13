using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ReviewTime { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey(nameof(Reviewer))]
        public int ReviewerId { get; set; }
        public Reviewer Reviewer { get; set; }

    }
}
