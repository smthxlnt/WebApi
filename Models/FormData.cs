using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi_sidorova.Models
{
    public class FormData
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public int ThemeId { get; set; }
        public string Message { get; set; }
    }
}
