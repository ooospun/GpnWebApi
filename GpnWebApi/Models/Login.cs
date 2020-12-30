using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GpnWebApi.Models
{
    /// <summary>
    /// Информация для авторизации
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Электронная почта
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Хэш пароля пользователя
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
