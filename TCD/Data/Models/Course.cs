using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCodeDevelopment.Data.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<StudentCourseEnrollment> StudentCourseEnrollment { get; set; }
    }
}
