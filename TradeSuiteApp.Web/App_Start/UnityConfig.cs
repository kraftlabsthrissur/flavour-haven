using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using DataAccessLayer;
using PresentationContractLayer;
using BusinessLayer;
using TradeSuiteApp.Web.Controllers;
using Unity.Injection;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using TradeSuiteApp.Web.Models;
using Unity.Lifetime;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web;

namespace TradeSuiteApp.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IDropdownContract, DropDownRepository>();
            container.RegisterType<IPayment, PaymentRepository>();
            container.RegisterType<IPurchaseReturn, PurchaseReturnRepository>();
            container.RegisterType<IRolePermissionContract, RolePermissionRepository>();
            container.RegisterType<IProductionIssue, ProductionIssueRepository>();
            container.RegisterType<IPreprocessIssue, PreprocessIssueRepository>();
            container.RegisterType<IPreprocessIssueReceipt, PreprocessIssueReceiptRepository>();
            container.RegisterType<IProductionSchedule, ProductionScheduleRepository>();

            

            //User Login
            container.RegisterType<IUserStore<ApplicationUser,int>, UserStore<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>>();
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));


            //container.RegisterType<AccountController>(new InjectionConstructor());
            ////container.RegisterType<RolesAdminController>(new InjectionConstructor());
            //container.RegisterType<ManageController>(new InjectionConstructor());
            ////container.RegisterType<UsersAdminController>(new InjectionConstructor());

            //container.RegisterType<UserStore<ApplicationUser,Role,int,UserLogin,UserRole,UserClaim>>();
            //container.RegisterType<ApplicationSignInManager>();
            //container.RegisterType<ApplicationUserManager>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
             
        }
    }
}