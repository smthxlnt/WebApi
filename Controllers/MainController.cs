using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using webapi_sidorova.Models;

namespace webapi_sidorova.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        SQL sql = new SQL();

        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return sql.GetThemes();
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return sql.GetMessage(id);
        }

        [HttpPost]
        public ActionResult<string> Post(FormData fd)
        {
            string result = "";
            try
            {
                bool success = true;
                string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
                Match isMatch = Regex.Match(fd.Mail, pattern, RegexOptions.IgnoreCase);
                success &= isMatch.Success;
                //pattern = "^\\+7\\s\\d{3}\\s\\d{3}\\s\\d{2}\\s\\d{2}$";
                //isMatch = Regex.Match(fd.Phone, pattern, RegexOptions.IgnoreCase);
                //success &= isMatch.Success;
                if (success)
                {
                    int contact = sql.GetContact(fd.Name, fd.Mail, fd.Phone);
                    if (contact > 0)
                    {
                        result = sql.NewMessage(contact, fd.ThemeId, fd.Message).ToString();
                    }
                    else
                    {
                        ModelState.AddModelError("FormData", "Не удалось внести сообщение в базу");
                    }
                }
                else
                {
                    ModelState.AddModelError("Mail", "Некорректный e-mail");
                }
                
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
