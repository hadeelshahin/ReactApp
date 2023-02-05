using ModelsShared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsShared.ViewModels
{
    public class RegisterVM
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public UserType UserType { get; set; }
    }
}
