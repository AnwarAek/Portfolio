using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Core.Entities
{
    public class Contact : EntityBase
    {
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string  Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string  Email { get; set; }
        public string  Number { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string  Message { get; set; }
    }
}
