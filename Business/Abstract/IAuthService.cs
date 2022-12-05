using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Ders33(1)
namespace Business.Abstract
{
    //Bu servis vasıtasıyla login yada register olacagız
    //Sisteme giriş yapacağız yada sisteme kayıt olacağız.
    public interface IAuthService
    {
        //Login olmak demek veritabanından kullanıcının varlığının kontrol edilmesi demek
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);//Idata result ile kayıt olan kullanıcı bilgisini alabiliriz
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult UserExist(string email); //Kullanıcı daha önce kayıt olmuşsa kızalım diye
        IDataResult<AccessToken> CreateAccessToken(User user);
    }
}
