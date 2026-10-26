using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace BudgetPeriodApp
{
    [DependsOn(typeof(BudgetPeriodAppCoreModule), typeof(AbpAutoMapperModule))]
    public class BudgetPeriodAppApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.AddMaps(typeof(BudgetPeriodAppApplicationModule).GetAssembly());
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BudgetPeriodAppApplicationModule).GetAssembly());
        }
    }
}
