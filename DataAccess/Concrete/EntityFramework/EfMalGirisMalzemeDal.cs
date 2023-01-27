using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMalGirisMalzemeDal : EfEntityRepositoryBase<MalGirisMalzeme, Contexts.NorthwindContext>, IMalGirisMalzemeDal
    {
        public void DeleteListByMGirisMgKayitId(int MgirisKayitId)
        {
            using (var context = new NorthwindContext())
            {
                context.MalGirisMalzemeler.RemoveRange(context.MalGirisMalzemeler.Where(x => x.MalGirisId== MgirisKayitId));

                context.SaveChanges();


            }
        }

        public List<MalGirisMalzemeDto> GetListByBolumDtos(int BolumId, int MgirisId)
        {
            using (var context = new NorthwindContext())
            {
                var result = (from mgMalzemeler in context.MalGirisMalzemeler
                              join malzemeler in context.Malzemeler on mgMalzemeler.MalzemeId equals malzemeler.Id
                              join mGrup in context.MalzemeGrups on malzemeler.MGrupId equals mGrup.Id
                              where mgMalzemeler.BolumId == BolumId
                              where mgMalzemeler.MalGirisId== MgirisId
                              select new MalGirisMalzemeDto()
                              {
                                  Id= mgMalzemeler.Id,
                                  Adet=mgMalzemeler.Adet,
                                  Agirlik=mgMalzemeler.Agirlik,
                                  Sertlik = mgMalzemeler.Sertlik,
                                  MalzemeGrupName= mGrup.GrupName,
                                  MalzemeStandartNo= malzemeler.MalzemeName


                              }).ToList();
                return result;


            }
        }

        public List<MalGirisMalzeme> GetListByMgKayitId(int MgirisId)
        {
            using (var context = new NorthwindContext())
            {
                var result = (from mgMalzemeler in context.MalGirisMalzemeler
                              where mgMalzemeler.MalGirisId == MgirisId
                              select new MalGirisMalzeme()
                              {
                                  Id = mgMalzemeler.Id,
                                  Adet = mgMalzemeler.Adet,
                                  Agirlik = mgMalzemeler.Agirlik,
                                  Sertlik = mgMalzemeler.Sertlik,
                                  BolumId= mgMalzemeler.BolumId,
                                  BulunanSertlik= mgMalzemeler.BulunanSertlik,
                                  FormNo= mgMalzemeler.FormNo,
                                  MalGirisId= mgMalzemeler.MalGirisId,
                                  MalzemeId= mgMalzemeler.MalzemeId,
                                  Secilen= mgMalzemeler.Secilen,
                                  SertlikDerinlik= mgMalzemeler.SertlikDerinlik,
                                  SertlikType = mgMalzemeler.SertlikType,
                                  SilenKullanici = mgMalzemeler.SilenKullanici,
                                  Silindi = mgMalzemeler.Silindi,
                                  SilindiAciklama = mgMalzemeler.SilindiAciklama,
                                  Toplam= mgMalzemeler.Toplam
                              }).ToList();
                return result;


            }
        }
    }
}
