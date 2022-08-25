using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace webapi_sidorova.Models
{
    public class FormData
    {
        [Required(ErrorMessage = "Укажите имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите e-mail")]
        public string Mail { get; set; }
        [Phone(ErrorMessage = "Некорректный номер телефон")]
        [Required(ErrorMessage = "Введите номер телефона")]
        public string Phone { get; set; }
        public int ThemeId { get; set; }
        [Required(ErrorMessage = "Введите сообщение")]
        public string Message { get; set; }
    }
}
