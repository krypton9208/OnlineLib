using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLib.Repository.ViewModels
{
    public class ListWorkersViewModel
    {
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
