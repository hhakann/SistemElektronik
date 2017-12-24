using SistemElektronik.WebUI.App_Classes;
using SistemElektronik.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemElektronik.WebUI.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Urunler()
        {
            //Bağlantı içerisindeki ürünleri listeye çevir 
            return View(Context.Baglanti.Urun.ToList());
        }
        public ActionResult UrunEkle()
        {
            //Baglatın içerisinden kategorileri ve markaları listeye çevirir ki ürün ekleme işlemleri gerçekleşsin
            ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
            ViewBag.Markalar = Context.Baglanti.Marka.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult UrunEkle(Urun urn)
        {

            urn.EklenmeTarihi = DateTime.Now;
            Context.Baglanti.Urun.Add(urn);
            Context.Baglanti.SaveChanges();


            return RedirectToAction("Urunler");
        }
        public ActionResult UrunSil(int id)
        {
            Urun urn = Context.Baglanti.Urun.FirstOrDefault(x => x.Id == id);
            Context.Baglanti.Urun.Remove(urn);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        }
        public ActionResult Markalar()
        {
            return View(Context.Baglanti.Marka.ToList());
        }
        public ActionResult MarkaEkle()
        {

            return View();
        }
        [HttpPost]
        public ActionResult MarkaEkle(Marka mrk, HttpPostedFileBase fileUpload)
        {
            int resimId = -1;
            if (fileUpload != null)

            {// REsmi fileuploaddan çektik
                Image img = Image.FromStream(fileUpload.InputStream);
                //Yükseklik ve genişlikleri Confic dosyasında settingsten çektik
                int widht = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaWidth"].ToString());
                int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaHeight"].ToString());
                //gelen image isimlerinin benzerliklerini engellemek için guid ile karakte oluşturup dosya yoluna yeni isim oluşturduk
                string name = "/Content/MarkaResim/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);
                //resmin ismiyle beraber kaydedilmesi
                Bitmap bm = new Bitmap(img, widht, height);
                bm.Save(Server.MapPath(name));

                //Tabloya resmi ekle
                Resim rsm = new Resim();
                rsm.OrtaYol = name;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                //Eger SAvechange te id atamaz ise resme oluşan id yi lokal resimId değişkeninde tutup Markanın mrk nesnesine resim Id si attılır.
                if (rsm.Id != null)
                {
                    resimId = rsm.Id;
                }

            }
            if (resimId != -1)
            {
                mrk.ResimId = resimId;
            }
            Context.Baglanti.Marka.Add(mrk);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");
        }
        public ActionResult MarkaSil(int id)
        {
            Marka mrk = Context.Baglanti.Marka.FirstOrDefault(x => x.Id == id);
            Context.Baglanti.Marka.Remove(mrk);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");
        }
        public ActionResult Kategoriler()
        {
            return View(Context.Baglanti.Kategori.ToList());
        }
        public ActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KategoriEkle(Kategori ktg)
        {
            Context.Baglanti.Kategori.Add(ktg);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Kategoriler");
        }

        public ActionResult KategoriSil(int id)
        {
            Kategori ktg = Context.Baglanti.Kategori.FirstOrDefault(x => x.Id == id);
            Context.Baglanti.Kategori.Remove(ktg);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Kategoriler");
        }
        public ActionResult OzellikTipEkle()
        {
            return View(Context.Baglanti.Kategori.ToList());
        }
        [HttpPost]
        public ActionResult OzellikTipEkle(OzellikTip ot)
        {
            Context.Baglanti.OzellikTip.Add(ot);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikTipleri");
        }
        public ActionResult OzellikTipSil(int id)
        {
            OzellikTip ot = Context.Baglanti.OzellikTip.FirstOrDefault(x => x.Id == id);
            Context.Baglanti.OzellikTip.Remove(ot);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikTipleri");
        }
        public ActionResult OzellikDegerleri()
        {
            return View(Context.Baglanti.OzellikDeger.ToList());
        }

        public ActionResult OzellikDegerEkle()
        {
            return View(Context.Baglanti.OzellikTip.ToList());
        }
        [HttpPost]
        public ActionResult OzellikDegerEkle(OzellikDeger od)
        {
            Context.Baglanti.OzellikDeger.Add(od);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikDegerleri");
        }

        public ActionResult OzellikDegerSil(int id)
        {
            OzellikDeger od = Context.Baglanti.OzellikDeger.FirstOrDefault(x => x.Id == id);
            Context.Baglanti.OzellikDeger.Remove(od);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikDegerleri");
        }
        public ActionResult UrunOzellikleri()
        {
            return View(Context.Baglanti.UrunOzellik.ToList());
        }

        public ActionResult UrunOzellikSil(int urunID, int tipID, int degerID)
        {
            UrunOzellik uo = Context.Baglanti.UrunOzellik.FirstOrDefault(x => x.UrunID == urunID && x.OzellikTipID == tipID && x.OzellikDegerID == degerID);
            Context.Baglanti.UrunOzellik.Remove(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        }
        public ActionResult UrunOzellikEkle()
        {
            return View(Context.Baglanti.Urun.ToList());
        }

        public PartialViewResult UrunOzellikTipWidget(int? katId)
        {
            if (katId != null)
            {
                var data = Context.Baglanti.OzellikTip.Where(x => x.KategoriID == katId).ToList();
                return PartialView(data);
            }
            else
            {
                var data = Context.Baglanti.OzellikTip.ToList();
                return PartialView(data);
            }
        }

        public PartialViewResult UrunOzellikDegerWidget(int? tipId)
        {
            if (tipId != null)
            {
                var data = Context.Baglanti.OzellikDeger.Where(x => x.OzellikTipID == tipId).ToList();
                return PartialView(data);
            }
            else
            {
                var data = Context.Baglanti.OzellikDeger.ToList();
                return PartialView(data);
            }
        }
        [HttpPost]
        public ActionResult UrunOzellikEkle(UrunOzellik uo)
        {
            Context.Baglanti.UrunOzellik.Add(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        }

        public ActionResult UrunResimEkle(int id)
        {
            return View(id);
        }

        [HttpPost]
        public ActionResult UrunResimEkle(int uId, HttpPostedFileBase fileupload)
        {
            if (fileupload != null)
            {
                Image img = Image.FromStream(fileupload.InputStream);

                Bitmap ortaResim = new Bitmap(img, Settings.UrunOrtaBoyut);
                Bitmap buyukResim = new Bitmap(img, Settings.UrunBuyukBoyut);

                string ortaYol = "/Content/UrunResim/Orta/" + Guid.NewGuid() + Path.GetExtension(fileupload.FileName);
                string buyukYol = "/Content/UrunResim/Buyuk/" + Guid.NewGuid() + Path.GetExtension(fileupload.FileName);

                ortaResim.Save(Server.MapPath(ortaYol));
                buyukResim.Save(Server.MapPath(buyukYol));

                Resim rsm = new Resim();
                rsm.BuyukYol = buyukYol;
                rsm.OrtaYol = ortaYol;
                rsm.UrunID = uId;

                if (Context.Baglanti.Resim.FirstOrDefault(x => x.UrunID == uId && x.Varsayilan == false) != null)
                    rsm.Varsayilan = true;
                else
                    rsm.Varsayilan = false;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                return View(uId);
            }
            return View(uId);
        }

        public ActionResult SliderResimleri()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SliderResimEkle(HttpPostedFileBase fileupload)
        {
            if (fileupload != null)
            {
                Image img = Image.FromStream(fileupload.InputStream);

                Bitmap bmp = new Bitmap(img, Settings.SliderResimBoyut);

                string yol = "/Content/SliderResim/" + Guid.NewGuid() + Path.GetExtension(fileupload.FileName);
                bmp.Save(Server.MapPath(yol));

                Resim rsm = new Resim();
                rsm.BuyukYol = yol;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
            }
            return RedirectToAction("SliderResimleri");
        }

    }

}