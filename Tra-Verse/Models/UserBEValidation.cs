using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tra_Verse.Models
{
    public class UserBEValidation
    {
        [Required] // attributes 
        [RegularExpression(@"^([A-z0-9\-.]{5,})@([A-z0-9\-.]+)\.([A-z]{2,5})$")]
        public string LoginEmail { get; set; }

        [Required]
        [RegularExpression(@"^[A-z0-9]{6,}$")]
        public string LoginPassword { set; get; }

        [Required]
        [RegularExpression(@"^([A-z0-9\-.]{5,})@([A-z0-9\-.]+)\.([A-z]{2,5})$")]
        public string RegEmail { set; get; }

        [Required]
        [RegularExpression(@"^[A-z0-9]{6,}$")]
        public string RegPassword { set; get; }

        [Required]
        [RegularExpression(@"^[A-z0-9]{2,}$")]
        public string ConfPass { set; get; }



        public UserBEValidation()

        {

            LoginEmail = "";

            LoginPassword = "";

            RegEmail = "";

            RegPassword = "";

            ConfPass = "";

        }

        public UserBEValidation(string le, string lp, string re, string rp, string cp)

        {

            LoginEmail = le;

            LoginPassword = lp;

            RegEmail = re;

            RegPassword = rp;

            ConfPass = cp;



        }
    }
}
