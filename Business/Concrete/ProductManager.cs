using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {

        //iş katmanını kullanabilmek için veri erişim katmanına ihtiyacımız var.(Ders 10)


        //Dependency Injection kalıbı oluşturduk
        //bağımlılıklardan kurtulduk
        private IProductDal _productDal;
        private ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService) //Constroctar kalıp
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [ValidationAspect(typeof(ProductValidator), Priority = 1)]
        [CacheRemoveAspect("IProductService.Get")]
        //[CacheRemoveAspect("ICategoryService.Get")] Arka arkaya yazılabilir duruma göre güncellensin diye. Servis ismi ve get geçen yerler
        public IResult Add(Product product)
        {
            //Business codes
            //Herhangi bir iş kodumuz varsa buraya gelecek
            //öengin daha önce eklenen ürün isminin bir daha eklenmemesi gibi işlemler, validasyon kodları vb.

            //  ValidationTool.Validate(new ProductValidator(), product);

            IResult result = BusinessRules.Run(CheckIfProuductNameExists(product.ProductName));

            if (result != null)
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);

            //Eger basarısız msajıda gönderilecekse try catch yapısı kullanılır 
        }


        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        [PerformanceAspect(5)] //5 snyi geçerse outputa yazacak.
        public IDataResult<List<Product>> GetList()
        {
            // Thread.Sleep(5000);
            return new SuccessDataResult<List<Product>>(_productDal.GetList().ToList());
        }
        //[SecuredOperation("Product.List,Admin")]
        [CacheAspect(duration: 10)]
        [LogAspect(typeof(FileLogger))]
        public IDataResult<List<Product>> GetListByCotegory(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.CategoryId == categoryId).ToList());
        }

        [TransactionScopeAspect]
        public IResult TransactionalOperation(Product product)
        {
            //Burada yazılan operasyonlaın tamamı gerçekleşemezse, Hiç çalışmaz yada gerçekleşen geri alınır.
            //Transactionlar çoklu operasyonların aynı anda çalışmasını garanti altına almak için kullanılır.
            _productDal.Update(product);
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }


        //ADD iş Kuralları
        private IResult CheckIfProuductNameExists(string productName)
        {
            var result = _productDal.GetList(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCategoryIsEnabled()
        {
            var result = _categoryService.GetList();
            if (result.Data.Count < 10)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
