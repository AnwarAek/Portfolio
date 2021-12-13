using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class Authontification : EntityBase
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string PassPhrase { get; set; }
    }
}
