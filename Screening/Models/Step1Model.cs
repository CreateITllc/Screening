using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Screening.Models
{
    public partial class Step1Model
    {
        public Step1Model()
        {
        }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter User Name")]
        [StringLength(15, MinimumLength = 6)]
        [System.Web.Mvc.Remote("IsUserNameExists", "Registration", ErrorMessage = "User Name already exists")]
        public string UserName { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9_\.\-]{2,4})+$", ErrorMessage = "Please Enter Valid Email Address")]

        [System.Web.Mvc.Remote("IsEmailExists", "Registration", ErrorMessage = "Email already exists")]
        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; }
        public int UserTypeID { get; set; }
        [Required(ErrorMessage = "New Password Required")]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6)]
        public string Password { get; set; }
        [Required(ErrorMessage = " Confirm Password Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The New Password and Confirm Password do not match.")]
        [StringLength(15, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
        public virtual ICollection<QuestionAnswerVm> UserQuestionAnswerVM { get; set; }
        [Required(ErrorMessage = "Please Select Question")]
        public int Question1 { get; set; }
        [Required(ErrorMessage = "Please Select Question")]
        public int Question2 { get; set; }
        [Required(ErrorMessage = "Please Select Question")]
        public int Question3 { get; set; }
        [Required(ErrorMessage = "Please Enter Answer")]
        public string Answer1 { get; set; }
        [Required(ErrorMessage = "Please Enter Answer")]
        public string Answer2 { get; set; }
        [Required(ErrorMessage = "Please Enter Answer")]
        public string Answer3 { get; set; }
        public string Captcha { get; set; }
        public bool IsActive { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CreatorID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdaterID { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}