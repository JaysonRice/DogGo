using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doggo.Models
{
    public class Dog
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(35)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Owner")]
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }

        [Required]
        public string Breed { get; set; }
        public string Notes { get; set; }

        [Url]
        public string ImageUrl { get; set; }
    }
}
