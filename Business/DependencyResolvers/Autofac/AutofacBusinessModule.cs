using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        //Şunun karşılığı sudur dediğimiz  bölgedir.
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductManager>().As<IProductService>();
            builder.RegisterType<EfProductDal>().As<IProductDal>();

            builder.RegisterType<CategoryManager>().As<ICategoryService>();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>();

            builder.RegisterType<ServisManager>().As<IServisService>();
            builder.RegisterType<EfServisDal>().As<IServisDal>();

            builder.RegisterType<BolumManager>().As<IBolumService>();
            builder.RegisterType<EfBolumDal>().As<IBolumDal>();

            builder.RegisterType<BolumIslemManager>().As<IBolumIslemService>();
            builder.RegisterType<EfBolumIslemDal>().As<IBolumIslemDal>();

            builder.RegisterType<MalzemeManager>().As<IMalzemeService>();
            builder.RegisterType<EfMalzemeDal>().As<IMalzemeDal>();


            builder.RegisterType<MalzemeGrupManager>().As<IMalzemeGrupService>();
            builder.RegisterType<EfMalzemeGrupDal>().As<IMalzemeGrupDal>();

            builder.RegisterType<IslemManager>().As<IIslemService>();
            builder.RegisterType<EfIslemDal>().As<IIslemDal>();

            builder.RegisterType<AktifIslemManager>().As<IAktifIslemService>();
            builder.RegisterType<EfAktifIslemDal>().As<IAktifIslemDal>();

            builder.RegisterType<FiyatManager>().As<IFiyatService>();
            builder.RegisterType<EfFiyatDal>().As<IFiyatDal>();

            builder.RegisterType<MusteriManager>().As<IMusteriService>();
            builder.RegisterType<EfMusteriDal>().As<IMusteriDal>();

            builder.RegisterType<MusteriFiyatManager>().As<IMusteriFiyatService>();
            builder.RegisterType<EfMusteriFiyatDal>().As<IMusteriFiyatDal>();

            builder.RegisterType<MalGirisKayitManager>().As<IMalGirisKayitService>();
            builder.RegisterType<EfMalGirisKayitDal>().As<IMalGirisKayitDal>();

            builder.RegisterType<MalGirisMalzemeManager>().As<IMalGirisMalzemeService>();
            builder.RegisterType<EfMalGirisMalzemeDal>().As<IMalGirisMalzemeDal>();

            builder.RegisterType<MalGirisResimManager>().As<IMalGirisResimService>();
            builder.RegisterType<EfMalGirisResimDal>().As<IMalGirisResimDal>();

            builder.RegisterType<MalGirisIslemManager>().As<IMalGirisIslemService>();
            builder.RegisterType<EfMalGirisIslemDal>().As<IMalGirisIslemDal>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
            //Katogory içinde user içinde buraları dolduruyor olacağız
        }
    }
}
//not eklendi