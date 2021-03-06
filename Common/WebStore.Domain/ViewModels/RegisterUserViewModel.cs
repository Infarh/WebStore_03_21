using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required, MaxLength(256)]
        [Display(Name = "Имя пользователя")]
        [Remote("InNameFree", "Account")]
        public string UserName { get; init; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; init; }
    }
}
