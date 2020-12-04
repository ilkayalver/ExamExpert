using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExamExpert.Common.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Kullanıcı Adı zorunludur")]
        [Display(Name = "Kullanıcı Adınız :")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [Display(Name = "Şifreniz :")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla?")]
        public bool RememberMe { get; set; }
    }
}
