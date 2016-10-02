using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CarRental.Data.App_Data;
using CarRental.Web.Models.ViewModel;

namespace CarRental.Web.Structure.Mapper
{
    public class MapperTranslator : Profile
    {
        public override string ProfileName
        {
            get { return "MapperTranslator"; }
        }

        public MapperTranslator()
        {
            CreateMap<Car, CarViewModel>();
        }
    }
}