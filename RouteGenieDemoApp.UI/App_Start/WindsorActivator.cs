using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(RouteGenieDemoApp.UI.App_Start.WindsorActivator), "PreStart")]
[assembly: ApplicationShutdownMethodAttribute(typeof(RouteGenieDemoApp.UI.App_Start.WindsorActivator), "Shutdown")]
namespace RouteGenieDemoApp.UI.App_Start
{
    public static class WindsorActivator
    {
        static ContainerBootstrapper _Bootstrapper;

        public static void PreStart()
        {
            _Bootstrapper = ContainerBootstrapper.Bootstrap();
        }

        public static void Shutdown()
        {
            if (_Bootstrapper != null)
                _Bootstrapper.Dispose();
        }
    }
}