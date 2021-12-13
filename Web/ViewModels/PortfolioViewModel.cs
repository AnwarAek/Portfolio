using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class PortfolioViewModel
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100 , MinimumLength = 5)]
        public string ProgectName { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
