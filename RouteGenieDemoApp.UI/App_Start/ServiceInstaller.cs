using Castle.MicroKernel.Registration;
using RouteGenieDemoApp.Business.Services;
using RouteGenieDemoApp.Business.Services.Interfaces;


namespace RouteGenieDemoApp.UI.App_Start
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container,
      Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            /*container.Register(
               Component
                   .For<IUserService>()
                   .ImplementedBy<UserService>()
                   .LifestyleTransient());*/




        }
    }
}