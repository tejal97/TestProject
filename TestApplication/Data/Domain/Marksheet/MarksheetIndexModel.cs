using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using TestApplication.Data.Enums;

namespace TestApplication.Domain.Marksheet
{
    public class MarksheetIndexModel
    {
        public MarksheetIndexModel()
        {
            GradeList = new List<SelectListItem>();
            foreach (GradeEnum eVal in Enum.GetValues(typeof(GradeEnum)))
            {
                GradeList.Add(new SelectListItem { Text = Enum.GetName(typeof(GradeEnum), eVal), Value = eVal.ToString() });
            }
        }
        public string Message { get; set; }
        public string Grade { get; set; }
        public string Student { get; set; }
        public List<SelectListItem> GradeList { get; set; }
    }
}
