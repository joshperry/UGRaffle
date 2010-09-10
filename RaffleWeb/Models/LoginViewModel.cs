using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RaffleWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage="Please specify your email.")]
        public string Email { get; set; }
        [Required(ErrorMessage="Password cannot be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}