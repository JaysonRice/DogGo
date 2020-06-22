using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doggo.Models
{
    public class Walker
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please add a neighborhood.")]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public Neighborhood Neighborhood { get; set; }
    }
}