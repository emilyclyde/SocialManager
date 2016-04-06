using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace socialmanager.Models
{
    public class FacebookProfileViewModel
    {
        [Required]
        [Display(Name = "First Name")]

        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]

        public string LastName { get; set; }

        public string Fullname { get; set; }

        public string ImageURL { get; set; }
        
        public string LinkURL { get; set; }

        public string Locale { get; set; }

        public string email { get; set; }

        public DateTime birthdate { get; set; }

        public string Location { get; set; }

        public string gender { get; set; }

        //public Facebook.JsonObject age_range { get; set; }

        public string Bio { get; set; }
    }
}