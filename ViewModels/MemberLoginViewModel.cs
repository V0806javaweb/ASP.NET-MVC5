﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemberSystem.ViewModels
{
    public class MemberLoginViewModel
    {
        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "輸入帳號")]
        public string Account { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "輸入密碼")]
        public string Password { get; set; }
    }
}