using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BankAccounts.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required]
        [MinLength(2, ErrorMessage = "must be atleast 2 characters")]
        [Display(Name = "First Name")]
        public string FirstName {get;set;}
        [Required]
        [MinLength(2, ErrorMessage = "must be atleast 2 characters")]
        [Display(Name = "Last Name")]
        public string LastName {get;set;}
        [EmailAddress]
        [Required]
        public string Email {get;set;}
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        // Will not be mapped to your users table
        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords need to match")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}

        public List<Transaction> AllTransactions {get;set;}
    }
}