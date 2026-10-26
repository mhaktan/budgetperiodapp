using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BudgetPeriodApp.EntityFrameworkCore;

namespace BudgetPeriodApp.Web.Host
{
    [DependsOn(typeof(BudgetPeriodAppApplicationModule), typeof(BudgetPeriodAppEntityFrameworkCoreModule), typeof(AbpAspNetCoreModule))]
    public class BudgetPeriodAppWebHostModule : AbpModule
    {
        public override void PreInitialize()
        {
            // Expose all AppServices as dynamic API controllers
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(BudgetPeriodAppApplicationModule).GetAssembly(),
                    moduleName: "app",
                    useConventionalHttpVerbs: true
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BudgetPeriodAppWebHostModule).GetAssembly());
        }
    }
}
