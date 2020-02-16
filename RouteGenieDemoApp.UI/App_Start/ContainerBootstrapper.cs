using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
namespace RouteGenieDemoApp.UI.App_Start
{
    public class ContainerBootstrapper : IContainerAccessor, IDisposable
    {
        readonly IWindsorContainer _Container;

        ContainerBootstrapper(IWindsorContainer container)
        {
            this._Container = container;
        }

        public IWindsorContainer Container
        {
            get { return _Container; }
        }

        public static ContainerBootstrapper Bootstrap()
        {
            var container = new WindsorContainer().
                Install(FromAssembly.This());
            return new ContainerBootstrapper(container);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}