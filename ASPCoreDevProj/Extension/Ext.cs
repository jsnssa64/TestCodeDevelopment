using ASPCoreDevProj.Model.BookDTO.Author;
using ASPCoreDevProj.Model.BookDTO.Author.Interface;
using ASPCoreDevProj.Model.BookDTO.Genre;
using ASPCoreDevProj.Model.BookDTO.Genre.Interface;
using Domain.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASPCoreDevProj.Extension
{

    public static class Reflection
    {
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }

    public static class ListLinqEditor
    {
        public static bool FindItemInListByValue<T>(this T Item, IList<T> SearchList, string Key) => SearchList.Any(SLItem => Reflection.GetPropValue(SLItem, Key) == Reflection.GetPropValue(Item, Key));
    }
}
