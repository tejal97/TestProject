using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestApplication.Data.Enums;

namespace TestApplication.Domain.StudentModels
{
    public class Student
    {
        [Key]
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Address { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Grade { get; set; }
    }
    public class Subject
    {
        [Key]
        public string SubjectId { get; set; }
        public string StudentId { get; set; }
        public string Grade { get; set; }

        [Range(0, StaticDataValues.FullMarks)]
        public int Maths { get; set; }

        [Range(0, StaticDataValues.FullMarks)]
        public int Science { get; set; }

        [Range(0, StaticDataValues.FullMarks)]
        public int English { get; set; }

        [Range(0, StaticDataValues.FullMarks)]
        public int Nepali { get; set; }

        [Range(0, StaticDataValues.FullMarks)]
        public int Social { get; set; }

        [Range(0, 100)]
        public float Percentage { get; set; }
        public float TotalMarks { get; set; }
    }
    public class StudentViewModel
    {
        public StudentViewModel()
        {
            Student = new Student();
            Subject = new Subject();
            GradeList = new List<SelectListItem>();
            foreach (GradeEnum eVal in Enum.GetValues(typeof(GradeEnum)))
            {
                GradeList.Add(new SelectListItem { Text = Enum.GetName(typeof(GradeEnum), eVal), Value = eVal.ToString() });
            }
        }
        public Student Student { get; set; }
        public Subject Subject { get; set; }
        public List<SelectListItem> GradeList { get; set; }
    } 
    public class StudentRankedListModel
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Percentage { get; set; }
        public string TotalMarks { get; set; }
    }
    public class FailedStudentModel
    {
        public string StudentId { get; set; }

        public string StudentName { get; set; }
        public string Percentage { get; set; }
    }
}
