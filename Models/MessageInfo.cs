using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi_sidorova.Models
{
    public class MessageInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Theme { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Addition { get; set; }
    }
}
