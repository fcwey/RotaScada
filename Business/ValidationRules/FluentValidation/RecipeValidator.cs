using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class RecipeValidator : AbstractValidator<Recipe>
    {

        public RecipeValidator()
        {
            //Burada iş kodu kesinlikle yazılmaz.
            //Yaptıgınız sey nesneye girilen değerlerle alakalı olmalı
            //Burada veritabanına bağlanmaya çalışmayın!!


            RuleFor(p => p.RecipeType).NotEmpty();
            //    RuleFor(p => p.ProductName).Length(2, 30);
            //    RuleFor(p => p.UnitPrice).NotEmpty();
            //    RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(1);
            //    RuleFor(p => p.ProductName).Must(StartWithA);
        }

        //private bool StartWithA(string arg)
        //{
        //    return arg.StartsWith("A");
        //}
    }
}
