using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;

namespace OnlineLib.App.ViewModels
{
    public class AddWorkerViewModel
    {
        [Required]
        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [Display(Name = "Name: ")]
        public string Name { get; set; }

        [Display(Name = "Surname: ")]
        public string Surname { get; set; }

    }

    public class ListWorkersViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Name and surname: ")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "City: ")]
        public string AddressCity { get; set; }

        [Required]
        [Display(Name = "Street and number")]
        public string AddressStreet { get; set; }

        [Required]
        [Display(Name = "Role: ")]
        public string RoleName { get; set; }
    }
}
