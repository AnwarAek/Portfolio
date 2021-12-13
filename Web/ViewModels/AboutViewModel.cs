using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class AboutViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(20)]
        public string FirstResume { get; set; }

        [Required]
        [MinLength(20)]
        public string SecondResume { get; set; }
        public string PdfUrl { get; set; }

        [Required]
        public IFormFile PdfFile { get; set; }
    }
}
