using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "* Name is required")]
        [StringLength(100, ErrorMessage = "* Name must not exceed 100 characters")]
        [Display(Name="Your Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "* Subject is required")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "* Email is required")]
        [EmailAddress(ErrorMessage = "* Must be a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "* Message is required")]
        [UIHint("MultilineText")]
        public string Message { get; set; }

    }
}