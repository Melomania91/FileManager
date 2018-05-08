using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pivkina.FileManager.Web.Models
{
    public class LoginModel
    {
        [Display(Name = "Логин")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите логин")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}