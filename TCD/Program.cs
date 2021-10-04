using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TestCodeDevelopment.Data;
using TestCodeDevelopment.Data.Models;

namespace TestCodeDevelopment
{
    class Program
    {
        public static Expression<Func<Student, bool>> isCalledJason
        {
            get
            {
                return value => value.FirstName == "Jason";
            }
        }

        public static Func<Student, bool> DoSomethingFunc
        {
            get
            {
                return value => value.FirstName == "Jason";
            }
        }

        public static string handleData(Student val)
        {
            return "Jason";
        }
        static void Main(string[] args)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                IQueryable<Student> T1 = from stud in context.Students
                                         select stud;
                List<Student> results = T1.Where(isCalledJason).ToList();


            }
            /*int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            IEnumerable<int> test = from num in numbers
                                    where num > 4
                                    select num;*/
        }

        public static Expression<Func<int, bool>> isLessThenTen
        {
            get
            {
                return (value) => value < 10;
            }
        }
    }
}
