using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models
{
    public class LoginUser
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "must be provided")]
        [EmailAddress]
        public string loginEmail {get;set;}
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "must be provided")]
        [Display(Name = "Password")]
        public string loginPassword {get;set;}
    }
}