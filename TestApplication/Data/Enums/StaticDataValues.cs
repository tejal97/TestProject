using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApplication.Domain.StudentModels;

namespace TestApplication.Data.Enums
{
    public static class StaticDataValues
    {
        public const int FullMarks = 100;
        public const int PassMarks = 40;

        
    }
    public static class StaticDataFunctions
    {
        public static float CalculatePercentage(Subject subject)
        {
            var subjectTotal = (float)(subject.Maths + subject.Nepali + subject.Social + subject.Science + subject.English);
            var multiplier = 100 / ((float)StaticDataValues.FullMarks * 5);
            return subjectTotal * multiplier;
        }
        public static float CalculateTotal(Subject subject)
        {
            return (subject.Maths + subject.Nepali + subject.Social + subject.Science + subject.English);
        }
    }
}
