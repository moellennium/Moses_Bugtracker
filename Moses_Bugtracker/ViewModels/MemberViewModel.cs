using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Moses_Bugtracker.ViewModels
{
    public class MemberViewModel
    {
        public string Id { get; set; }

        [StringLength(40, ErrorMessage = "First Name must be between {2} and {1} characters long.", MinimumLength = 1)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress()]
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string PhoneNumber { get; set; }

        public HttpPostedFileBase Avatar { get; set; }
        
    }
}