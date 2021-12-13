using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class About : EntityBase
    {
        public string FirstResume { get; set; }
        public string SecondResume { get; set; }
        public string PdfUrl { get; set; }
    }
}
