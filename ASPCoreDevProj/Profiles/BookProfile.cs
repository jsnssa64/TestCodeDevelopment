using ASPCoreDevProj.Extension;
using ASPCoreDevProj.Model.BookDTO.Author;
using ASPCoreDevProj.Model.BookDTO.Book;
using ASPCoreDevProj.Model.BookDTO.Book.Interface;
using AutoMapper;
using System.Collections.Generic;
using Domain.Data.Model;
using System.Linq;
using ASPCoreDevProj.Model.BookDTO.Genre;
using System;
using ASPCoreDevProj.Model.BookDTO.Author.Interface;
using ASPCoreDevProj.Model.BookDTO.Genre.Interface;

namespace ASPCoreDevProj.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            //  Database Layer -> DTO
            CreateMap<Author, AuthorBasic>().ReverseMap();
            CreateMap<Genre, GenreBasic>().ReverseMap();
            CreateMap<Book, BookWithGenreAndAuthor>().ReverseMap();
            CreateMap<Book, BasicBook>().ReverseMap();
            CreateMap<Author, AuthorWithDOB>().ForMember(dest => dest.DOB, opt => opt.MapFrom(val => new DateTime(val.DOB.Year, val.DOB.Month, val.DOB.Day)));

            //  DTO -> Database Layer
        }
    }
}
