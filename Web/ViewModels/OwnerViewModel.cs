using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class OwnerViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Profile { get; set; }
        public string Imageurl { get; set; }
        public IFormFile File { get; set; }
    }
}
