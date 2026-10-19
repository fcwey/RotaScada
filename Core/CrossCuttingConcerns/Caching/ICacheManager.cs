using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        object Get(string key);
        void Add(string key, object data, int duration);
        bool IsAdd(string key); //Cache tenmi getirelim yoksa veritabanındanmı
        void Remove(string key);    //Belli bir keyde ki Cache in ortadan kaldırılmasını sağlar.
        void RemoveByPattern(string pattern);//Örnegin get ile başlayan tüm Cache leri sil

    }
}
