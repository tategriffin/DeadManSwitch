using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeadManSwitch.Service.WebApi
{
    public static class UserMapper
    {
        static UserMapper()
        {
            Mapper.AddProfile(new UserMapperProfile());
        }

        public static DeadManSwitch.Service.User ToServiceEntity(this DeadManSwitch.Service.WebApi.UserModel source)
        {
            return Mapper.Map<DeadManSwitch.Service.User>(source);
        }

        public static DeadManSwitch.Service.WebApi.UserModel ToWebApiEntity(this DeadManSwitch.Service.User source)
        {
            return Mapper.Map<DeadManSwitch.Service.WebApi.UserModel>(source);
        }

        public static DeadManSwitch.Service.User ToServiceEntity(this DeadManSwitch.Service.WebApi.UserRegistrationModel source)
        {
            return Mapper.Map<DeadManSwitch.Service.User>(source);
        }

        public static async Task<DeadManSwitch.Service.User> ToServiceEntityUser(this HttpResponseMessage source)
        {
            return await source.DeserializeResponseContentAsync<DeadManSwitch.Service.User>();
        }

        public static DeadManSwitch.Service.WebApi.UserRegistrationModel ToRegistrationModel(this DeadManSwitch.Service.User user, string password)
        {
            DeadManSwitch.Service.WebApi.UserModel userApiModel = user.ToWebApiEntity();

            DeadManSwitch.Service.WebApi.UserRegistrationModel registrationModel = new UserRegistrationModel();
            registrationModel.UserName = userApiModel.UserName;
            registrationModel.Email = userApiModel.Email;
            registrationModel.FirstName = userApiModel.FirstName;
            registrationModel.LastName = userApiModel.LastName;

            registrationModel.Password = password;

            return registrationModel;
        }

    }

    public class UserMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.WebApi.UserModel, DeadManSwitch.Service.User>();
            Mapper.CreateMap<DeadManSwitch.Service.User, DeadManSwitch.Service.WebApi.UserModel>();
//            Mapper.CreateMap<DeadManSwitch.Service.WebApi.UserRegistrationModel, DeadManSwitch.Service.User>();
        }
    }
}
