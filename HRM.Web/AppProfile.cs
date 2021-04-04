using AutoMapper;
using HRM.Data.Models;
using HRM.InputModel.Rooms;
using HRM.InputModel.RoomTypes;
using HRM.Services.Rooms.Model;
using HRM.Services.Users.Model;
using HRM.ViewModel.Rooms;
using HRM.ViewModel.RoomTypes;
using HRM.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Web
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            #region RoomTypes
            this.CreateMap<RoomType,RoomTypesInfoViewModel>();
            this.CreateMap<RoomType, RoomTypesEditInputModel>();
            #endregion

            #region
            this.CreateMap<User, UsersInfoViewModel>();
            this.CreateMap<User, UserServiceModel>().ReverseMap();
            this.CreateMap<User, User>();
            this.CreateMap<UsersInfoViewModel, UserServiceModel>().ReverseMap();

            #endregion
            #region
            this.CreateMap<Room, RoomsInfoViewModel>().ReverseMap(); 
            this.CreateMap<Room, Room>(); 
            this.CreateMap<RoomsInfoViewModel,RoomsCreateInputViewModel>();
            this.CreateMap<RoomServiceModel, Room>(); 

            #endregion
        }
    }
}
