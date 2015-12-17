using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;

namespace OnlineLib.Repository.ViewModels
{
    public class ListWorkersViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Name and surname: ")]
        public string Name { get; set; }

      

        [Required]
        [Display(Name = "Role: ")]
        public string RoleName { get; set; }
    }

    public class ProfiEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Name: ")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Surname: ")]
        public string Surname { get; set; }


    }
}
