using AutoMapper;
using System;

namespace Tatisoft.UsersAdmin.Core.Model.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());

        void AfterMapping() => Console.WriteLine("hola!");
    }
}