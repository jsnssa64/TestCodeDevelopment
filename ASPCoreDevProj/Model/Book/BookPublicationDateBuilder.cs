using ASPCoreDevProj.Interface.Date;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreDevProj.Model.Book
{
    public interface IDateFormat { }

    public abstract class DateBuilder<TDateObject> where TDateObject : IDateFormat
    {
        public TDateObject DateObject { get; }
    }

    public class BookDate : IDateFormat
    {
        [Required]
        [Range(0, 40)]
        public int Day { get; set; }

        [Required]
        [Range(0, 40)]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }
    }

    public class BookDateBuilder : DateBuilder<BookDate>
    {
        public BookDateBuilder(int day, int month, int year)
        {
            DateObject.Day = day;
            DateObject.Month = month;
            DateObject.Year = year;
        }
    }
}
