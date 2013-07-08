using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using Dto;
using Persistence.DO;

namespace UI
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //playlists route /playlist/3
            routes.MapRoute("PlaylistsEditIds",
                            "Playlist/{playlistId}",
                            new {controller = "Playlist", action="Edit"},
                            new{playlistId=@"\d+"} // constraint apenas numeros
                );


            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterAutoMapper();
            RegisterRoles();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private static void RegisterRoles()
        {
            if (!Roles.RoleExists("admin"))
                Roles.CreateRole("admin");
            if (!Roles.RoleExists("user"))
                Roles.CreateRole("user");
            if (Membership.GetUser("administrador") == null)
                Membership.CreateUser("administrador", "123456");
            if (!Roles.GetRolesForUser("administrador").Contains("admin"))
                Roles.AddUserToRole("administrador", "admin");
            
        }

        private static void RegisterAutoMapper()
        {
            //UserProfiles
            Mapper.CreateMap<UserProfileDto, UserProfile>();
            Mapper.CreateMap<UserProfile, UserProfileDto>()
                  .ForMember(dto => dto.TotalPlaylists, e => e.MapFrom(u => u.Playlists.Count))
                  .ForMember(dto =>dto.Email, e=>e.Ignore());
            
            //Playlist
            Mapper.CreateMap<PlaylistDto,Playlist>();
            Mapper.CreateMap<Playlist, PlaylistDto>()
                .ForMember(dto => dto.TotalTracks, e => e.MapFrom(playlist => playlist.Tracks.Count))
                .ForMember(dto => dto.Tracks, e => e.Ignore())
                .ForMember(dto=> dto.Contributor, e=> e.Ignore());

            //Tracks
            Mapper.CreateMap<Track, TrackDto>();

            //SharedPlaylist
            Mapper.CreateMap<SharedPlaylist, SharedPlaylistDto>();
            Mapper.CreateMap<SharedPlaylistDto, SharedPlaylist>();

        }

    }
}