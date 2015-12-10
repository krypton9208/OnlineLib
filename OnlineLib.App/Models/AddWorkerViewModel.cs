using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
}
