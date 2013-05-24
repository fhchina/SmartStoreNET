using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using SmartStore.Core.Data;
using SmartStore.Core.Infrastructure;
using SmartStore.Core.Infrastructure.DependencyManagement;
using SmartStore.Data;
using SmartStore.Plugin.Shipping.ByWeight.Data;
using SmartStore.Plugin.Shipping.ByWeight.Domain;
using SmartStore.Plugin.Shipping.ByWeight.Services;

namespace SmartStore.Plugin.Shipping.ByWeight
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ShippingByWeightService>().As<IShippingByWeightService>().InstancePerHttpRequest();

            //data layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                //register named context
                builder.Register<IDbContext>(c => new ShippingByWeightObjectContext(dataProviderSettings.DataConnectionString))
                    .Named<IDbContext>(ShippingByWeightObjectContext.ALIASKEY)
                    .InstancePerHttpRequest();

                builder.Register<ShippingByWeightObjectContext>(c => new ShippingByWeightObjectContext(dataProviderSettings.DataConnectionString))
                    .InstancePerHttpRequest();
            }
            else
            {
                //register named context
                builder.Register<IDbContext>(c => new ShippingByWeightObjectContext(c.Resolve<DataSettings>().DataConnectionString))
                    .Named<IDbContext>(ShippingByWeightObjectContext.ALIASKEY)
                    .InstancePerHttpRequest();

                builder.Register<ShippingByWeightObjectContext>(c => new ShippingByWeightObjectContext(c.Resolve<DataSettings>().DataConnectionString))
                    .InstancePerHttpRequest();
            }

            //override required repository with our custom context
            builder.RegisterType<EfRepository<ShippingByWeightRecord>>()
                .As<IRepository<ShippingByWeightRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ShippingByWeightObjectContext.ALIASKEY))
                .InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}