﻿using System.ComponentModel.DataAnnotations;

namespace Webprj.Models.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
