using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExamExpert.Common.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Kullanıcı Adı zorunludur")]
        [StringLength(100)]
        [Display(Name = "Kullanıcı Adınız")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }        

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(6, ErrorMessage = "Şifreniz 6 karakter içermek zorundadır. 6 karakterden az veya fazla karakter girişi yapamazsınız. Karakter olarak rakam ve harf kullanabilirsiniz.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler aynı olmalıdır.")]
        public string ConfirmPassword { get; set; }
    }
}
