using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        //Buradaki yapıyı cok dil destekli sistemlerlede yapılandırabilirsiniz.
        //Örn bir proprty oluşturup Get bloğunda seçili dile göre karşılığını veri tabanından getirebilirsiniz.
        public static string ProductAdded = "Ürün Başarıyla eklendi";
        public static string ProductDeleted = "Ürün Başarıyla silindi";
        public static string ProductUpdated = "Ürün Başarıyla güncellendi";
        public static string CategoryAdded = "Kategori Başarıyla eklendi";
        public static string CategoryDeleted = "Kategori Başarıyla silindi";
        public static string CategoryUpdated = "Kategori Başarıyla güncellendi";

        public static string UserNotFound = "Kullanıcı Bulunamadı";
        public static string PasswordError = "Şifre Hatalı";
        public static string SuccessfulLogin = "Sisteme Giriş Başarılı";
        public static string UserAlreadyExists = "Bu kullanıcı zaten mevcut";
        public static string UserRegistered = "Kullanıcı Başarıyla Kaydedildi";
        public static string AccessTokenCreated = "Access Token Başarıyla Oluşturuldu";

        public static string AuthorizationDenied = "Yetkiniz yok";
        public static string ProductNameAlreadyExists = "Ürün zaten mevcut";

        public static string ServisUpdated = "Servis aracı Başarıyla güncellendi";
        public static string ServisDeleted= "Servis aracı Başarıyla silindi";
        public static string ServisAdded = "Servis aracı Başarıyla eklendi";
    }
}
