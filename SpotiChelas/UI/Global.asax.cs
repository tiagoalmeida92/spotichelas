using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            //playlists route /playlist/3/edit
            routes.MapRoute("PlaylistsRouteIds",
                            "Playlist/{action}/{playlistId}",
                            new {controller = "Playlist"}
                );

             //playlists route /playlist/index
            routes.MapRoute("PlaylistsRoute",
                            "Playlist/{action}",
                            new {controller = "Playlist", action = "Index"}
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

            // Use LocalDB for Entity Framework by default
            //Database.DefaultConnectionFactory =
            //    new SqlConnectionFactory(
            //        @"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");
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

    }
}