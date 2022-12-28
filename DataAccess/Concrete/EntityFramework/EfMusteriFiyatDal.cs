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
    public class EfMusteriFiyatDal : EfEntityRepositoryBase<MusteriFiyat, Contexts.NorthwindContext>, IMusteriFiyatDal
    {
        public List<MusteriFiyatDto> GetListMusteriFiyatDtos(int musteriId)
        {
            //müşteri id sine göre olmalı, yada musteri çağıracağız? bu tamamen müşterilerde olabilir
            using (var context = new NorthwindContext())
            {
                var result = (from fiyat in context.Fiyatlar
                              from mfiyat in context.musteriFiyats.Where(b => b.FiyatId == fiyat.Id).DefaultIfEmpty()
                                                                  .Where(b => b.MusteriId == musteriId).DefaultIfEmpty()
                              join bolumislem in context.BolumIslemler on fiyat.BolumIslemId equals bolumislem.Id
                              join bolum in context.Bolumler on bolumislem.BolumId equals bolum.Id
                              join mgrup in context.MalzemeGrups on fiyat.MGrupId equals mgrup.Id
                              orderby fiyat.MGrupId
                              select new MusteriFiyatDto()
                              {
                                  Id = fiyat.Id,
                                  BolumIslemName = bolumislem.BolumIslemi,
                                  BolumName = bolum.Name,
                                  MGrupName = mgrup.GrupName,
                                  BirimFiyati = fiyat.Fiyati,
                                  MusteriFiyati = mfiyat.MusFiyat

                              }).ToList();
                return result;


            }
        }
    }
}
//var result = (from fiyat in context.Fiyatlar
//              join musteriFiyat in context.musteriFiyats on fiyat.Id equals musteriFiyat.FiyatId
//              join bolumislem in context.BolumIslemler on fiyat.BolumIslemId equals bolumislem.Id
//              join bolum in context.Bolumler on bolumislem.BolumId equals bolum.Id
//              join mgrup in context.MalzemeGrups on fiyat.MGrupId equals mgrup.Id
//              orderby fiyat.MGrupId
//              select new MusteriFiyatDto()
//              {
//                  Id = fiyat.Id,
//                  BolumIslemName = bolumislem.BolumIslemi,
//                  BolumName = bolum.Name,
//                  MGrupName = mgrup.GrupName,
//                  BirimFiyati = fiyat.Fiyati,
//                  MusteriFiyati = musteriFiyat.MusFiyat

//              }).ToList();
//return result;