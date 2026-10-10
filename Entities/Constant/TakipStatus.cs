using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Constant
{
    public static class TakipStatus
    {
        public struct Nerede
        {
            public static string StokGiris = "STOK-GİRİŞ";
            public static string StokCikis = "STOK-ÇIKIŞ";
            public static string TeslimEdildi = "TESLİM EDİLDİ";
            public static string KaliteKontrol = "KALİTE KONTROL";
            public static string Liste = "LİSTE";
            public static string Islem = "İŞLEMLER";
        }
        public struct Durum
        {
            public static string Bekliyor = "BEKLİYOR";
            public static string ProseseHazir = "İŞLEME HAZIR";
            public static string ProsesYapiliyor = "İŞLEM YAPILIYOR";
            public static string ProsesBitti = "İŞLEM BİTTİ";
            public static string OnayBekliyor = "ONAY BEKLİYOR";
            public static string NakliyeBekliyor = "NAKLİYE BEKLİYOR";
            public static string Gitti = "GİTTİ";
        }
    }
}
