using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsShared.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public string UniversityNumber { get; set; }
        public Gender Gender { get; set; }
        public UserType UserType { get; set; }
        public ICollection<UsersClass> Classes { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
    public enum UserType
    {
        Student,
        Teacher
    }
}
