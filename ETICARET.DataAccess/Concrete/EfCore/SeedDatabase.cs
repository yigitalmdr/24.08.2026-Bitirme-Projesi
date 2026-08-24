using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed(DataContext context)
        {
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.AddRange(Categories);
                }
                if (context.Products.Count() == 0)
                {
                    context.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
                context.SaveChanges();
            }

        }
        private static Category[] Categories =
        {
            new Category(){Name="Telefon"},//0
            new Category(){Name="Bilgisayar"},//1
            new Category(){Name="Elektronik"},//2
            new Category(){Name="Ev Gereçleri"},//3
            new Category(){Name="Beyaz Eşya"},//4
            new Category(){Name="Moda"},//5
            new Category(){Name="Kitap Müzik Film Hobi"}//6
        };
        private static Product[] Products =
        {
            //0
            new Product(){Name="IPhone 15 128",Price=50000,Images={
                    new Image(){ImageUrl="iphone 15.jpg"},
                    new Image(){ImageUrl="iphone 15-1.jpg"},
                    new Image(){ImageUrl="iphone 15-2.jpg"},
                    new Image(){ImageUrl="iphone 15-3.jpg"}},
                Description="<p>IPhone 15 128 gb Telefon</p>"
                },
            //1
          new Product(){ Name = "iPhone 14 128 GB" , Price = 50000, Images =
                {   new Image() {ImageUrl = "iphone14.jpg" },
                    new Image() {ImageUrl = "iphone15.jpg" },
                    new Image() {ImageUrl = "iphone14mini.jpg" },
                    new Image() {ImageUrl = "iphone12.jpg" } },
                Description ="<p>elma</p>" },
            //2
            new Product(){Name="MSI MEG VISION X Gaming PC",Price=409684,Images={
                    new Image(){ImageUrl="msi-meg-vision.jpg"},
                    new Image(){ImageUrl="msi-meg-vision-1.jpg"},
                    new Image(){ImageUrl="msi-meg-vision-2.jpg"},
                    new Image(){ImageUrl="msi-meg-vision-3.jpg"} },
                Description="<p>İşletim Sistemi: Windows 11 Home\r\nİşlemci: Intel Core Ultra 9 processor 285K\r\nDepolama Alanı: 2TB SSD\r\nSistem Belleği: 64GB DDR5\r\nEkran Kartı: GeForce RTX 5090 32G VENTUS 3X\r\nRenk: Siyah</p>"
            },
            //3
            new Product(){Name="MSI MPG INFINITE Z3 Gaming PC",Price=145473,Images={
                    new Image(){ImageUrl="msi-mpg-infinite.jpg"},
                    new Image(){ImageUrl="msi-mpg-infinite-2.jpg"},
                    new Image(){ImageUrl="msi-mpg-infinite-3.jpg"},
                    new Image(){ImageUrl="msi-mpg-infinite-4.jpg"} },
                Description="<p>İşletim Sistemi: Windows 11 Home\r\nİşlemci: AMD Ryzen 7 9700X\r\nDepolama Alanı: 1TB*1 SSD\r\nSistem Belleği: 32GB DDR5\r\nEkran Kartı: GeForce RTX 5070 12G SHADOW 2X\r\nRenk: Siyah</p>"
            },
            //4
            new Product(){Name="MSI MAG INFINITE S3 Gaming PC",Price=122235,Images={
                    new Image(){ImageUrl="msi-mag-infinite.jpg"},
                    new Image(){ImageUrl="msi-mag-infinite-1.jpg"},
                    new Image(){ImageUrl="msi-mag-infinite-2.jpg"},
                    new Image(){ImageUrl="msi-mag-infinite-3.jpg"} },
                Description="<p>İşletim Sistemi: Windows 11 Home\r\nİşlemci: Intel Core i7-14700F\r\nDepolama Alanı:  1TB SSD\r\nSistem Belleği: 32GB DDR5\r\nEkran Kartı: GeForce RTX 5070 SHADOW 2X 12G\r\nRenk: Siyah</p>"
            },
            //5
            new Product(){Name="MSI MPG INFINITE X3 Masaüstü Gaming PC",Price=180270,Images={
                    new Image(){ImageUrl="msi-mpg-x3.jpg"},
                    new Image(){ImageUrl="msi-mpg-x3-1.jpg"},
                    new Image(){ImageUrl="msi-mpg-x3-2.jpg"},
                    new Image(){ImageUrl="msi-mpg-x3-3.jpg"} },
                Description="<p>İşletim Sistemi: Windows 11\r\nİşlemci: Intel ULTRA 7 265K\r\nDepolama Alanı: 1000GB SSD\r\nSistem Belleği: 32GB DDR5\r\nEkran Kartı: GeForce RTX 5070 Ti VENTUS 3X 16G\r\nRenk: Siyah</p>"
            },
            //6
            new Product(){Name="MSI MPG TRIDENT AS  Gaming PC",Price=115000,Images={
                    new Image(){ImageUrl="msi-mpg-trident.jpg"},
                    new Image(){ImageUrl="msi-mpg-trident-1.jpg"},
                    new Image(){ImageUrl="msi-mpg-trident-2.jpg"},
                    new Image(){ImageUrl="msi-mpg-trident-3.jpg"} },
                Description="<p>İşletim Sistemi: Windows 11 Home\r\nİşlemci: Intel Core Ultra 7 processor 265F\r\nDepolama Alanı: 1TB*1 SSD\r\nSistem Belleği: 32GB DDR5\r\nEkran Kartı: GeForce RTX 5060 Ti 8G SHADOW 2X PLUS\r\nRenk: Siyah</p>"
            },
            //7
            new Product(){Name="Ütü Masası",Price=500,Images={
                    new Image(){ImageUrl="utu-masasi.jpg"},
                    new Image(){ImageUrl="utu-masasi-1.jpg"},
                    new Image(){ImageUrl="utu-masasi-2.jpg"},
                    new Image(){ImageUrl="utu-masasi-3.jpg"} },
                Description="<p>Cok iyi ütü masası</p>"
            },
            //8
            new Product(){Name="Buzdolabı",Price=15000,Images={
                    new Image(){ImageUrl="buzdolabi.jpg"},
                    new Image(){ImageUrl="buzdolabi-1.jpg"},
                    new Image(){ImageUrl="buzdolabi-2.jpg"},
                    new Image(){ImageUrl="buzdolabi-3.jpg"} },
                Description="<p>Cok iyi buzdolabı</p>"
            },
            //9
            new Product(){Name="Çamaşır Makinesi",Price=10000,Images={
                    new Image(){ImageUrl="camasir-makinesi.jpg"},
                    new Image(){ImageUrl="camasir-makinesi-1.jpg"},
                    new Image(){ImageUrl="camasir-makinesi-2.jpg"},
                    new Image(){ImageUrl="camasir-makinesi-3.jpg"} },
                Description="<p>Cok iyi çamaşır makinesi</p>"
            },
            //10
            new Product(){Name="Bulaşık Makinesi",Price=8000,Images={
                    new Image(){ImageUrl="bulasik-makinesi.jpg"},
                    new Image(){ImageUrl="bulasik-makinesi-1.jpg"},
                    new Image(){ImageUrl="bulasik-makinesi-2.jpg"},
                    new Image(){ImageUrl="bulasik-makinesi-3.jpg"} },
                Description="<p>Cok iyi bulaşık makinesi</p>"
            },
            //11
            new Product(){Name="Televizyon",Price=20000,Images={
                    new Image(){ImageUrl="televizyon.jpg"},
                    new Image(){ImageUrl="televizyon-1.jpg"},
                    new Image(){ImageUrl="televizyon-2.jpg"},
                    new Image(){ImageUrl="televizyon-3.jpg"} },
                Description="<p>Cok iyi televizyon</p>"
          },
            //12
              new Product(){Name="IPhone 17 256",Price=80000,Images={
                    new Image(){ImageUrl="iphone 17.jpg"},
                    new Image(){ImageUrl="iphone 17-1.jpg"},
                    new Image(){ImageUrl="iphone 17-2.jpg"},
                    new Image(){ImageUrl="iphone 17-3.jpg"}},
                Description="<p>IPhone 17 256 gb Beyaz Telefon</p>"
                },
            //13
            new Product(){Name="IPhone 14 128",Price=40000,Images={
                    new Image(){ImageUrl="iphone14.jpg"},
                    new Image(){ImageUrl="iphone14-1.jpg"},
                    new Image(){ImageUrl="iphone14-2.jpg"},
                    new Image(){ImageUrl="iphone14-3.jpg"}},
                Description="<p>IPhone 14 128 gb Blue Telefon</p>"
                },
            //14
            new Product(){Name="PHILIPS EP5547/90 Tam Otomatik Espresso Makinesi Krom Siyah",Price=30000,Images={
                    new Image(){ImageUrl="PHILIPS.jpg"},
                    new Image(){ImageUrl="PHILIPS-1.jpg"},
                    new Image(){ImageUrl="PHILIPS-2.jpg"},
                    new Image(){ImageUrl="PHILIPS-3.jpg"}},
                Description="<p>Tam Otomatik Espresso Makinesi</p>"
                },
            //15
            new Product(){Name="DYSON V12 Detect Slim Absolute Kablosuz Şarjlı Dikey Süpürge Sarı Nikel",Price=40000,Images={
                    new Image(){ImageUrl="DYSON.jpg"},
                    new Image(){ImageUrl="DYSON-1.jpg"},
                    new Image(){ImageUrl="DYSON-2.jpg"},
                    new Image(){ImageUrl="DYSON-3.jpg"}},
                Description="<p>Kablosuz Şarjlı Dikey Süpürge</p>"
                },
            //16
            new Product(){Name="TEFAL SuperGrill 3in1 Tost Makinesi Inox",Price=6000,Images={
                    new Image(){ImageUrl="TEFAL.jpg"},
                    new Image(){ImageUrl="TEFAL-1.jpg"},
                    new Image(){ImageUrl="TEFAL-2.jpg"},
                    new Image(){ImageUrl="TEFAL-3.jpg"}},
                Description="<p>Ekstra ızgara seçeneği</p>"
                },
            //17
            new Product(){Name="ARZUM AR6000 Steampro Plus Buharlı Ütü Siyah",Price=3000,Images={
                    new Image(){ImageUrl="ARZUM.jpg"},
                    new Image(){ImageUrl="ARZUM-1.jpg"},
                    new Image(){ImageUrl="ARZUM-2.jpg"},
                    new Image(){ImageUrl="ARZUM-3.jpg"}},
                Description="<p>Buharlı Ütü Siyah</p>"
                },
            //18
            new Product(){Name="GRUNDIG KMP 4470 G Mutfak Şefi",Price=10000,Images={
                    new Image(){ImageUrl="GRUNDIG.jpg"},
                    new Image(){ImageUrl="GRUNDIG-1.jpg"},
                    new Image(){ImageUrl="GRUNDIG-2.jpg"},
                    new Image(){ImageUrl="GRUNDIG-3.jpg"}},
                Description="<p>Hamur Yoğurma Aparatı, Balon Çırpıcı Aparat</p>"
                },
            //19
               new Product(){ Name = "İphone 13 128 GB" , Price = 36000, Images = { new Image() {ImageUrl = "iphone5.jpg" },  new Image() {ImageUrl = "iphone6.jpg" }, new Image() {ImageUrl = "iphone15.jpg" }, new Image() {ImageUrl = "iphone13.jpg" } },Description ="<p>3 kameralı elma</p>" },
            //20
            new Product(){ Name = "İphone 12 128 GB" , Price = 36000, Images = { new Image() {ImageUrl = "iphone5.jpg" },  new Image() {ImageUrl = "iphone6.jpg" }, new Image() {ImageUrl = "iphone15.jpg" }, new Image() {ImageUrl = "iphone13.jpg" } },Description ="<p>hocanın telefonundan</p>" },
            //21
            new Product(){ Name = "İphone 8 128 GB" , Price = 15000, Images = { new Image() {ImageUrl = "iphone11.jpg" },  new Image() {ImageUrl = "iphone5.jpg" }, new Image() {ImageUrl = "iphone15.jpg" }, new Image() {ImageUrl = "iphone.jpg" } },Description ="<p>Emektar Elma</p>" },
            //22
            new Product(){ Name = "İphone 15 128 GB" , Price = 90000, Images = { new Image() {ImageUrl = "iphone13.jpg" },  new Image() {ImageUrl = "iphone15.jpg" }, new Image() {ImageUrl = "iphone14mini.jpg" }, new Image() {ImageUrl = "iphone.jpg" } },Description ="<p>Amasya Elması</p>" },
            //23
            new Product(){ Name = "samsung monitör" , Price = 50000, Images = { new Image() {ImageUrl = "samsungmonitor3.jpg" },  new Image() {ImageUrl = "samsungmonitor1.jpg" }, new Image() {ImageUrl = "samsungmonitor2.jpg" }, new Image() {ImageUrl = "samsungmonitor4.jpg" } },Description ="<p>Evrim teorisi</p>" },
            //24
            new Product(){ Name = "Ipad" , Price = 7000, Images = { new Image() {ImageUrl = "ipad3.jpg" },  new Image() {ImageUrl = "ipad6.jpg" }, new Image() {ImageUrl = "ipad5.jpg" }, new Image() {ImageUrl = "ipad3.jpg" } },Description ="<p>Tablet</p>" },
            //25
            new Product(){ Name = "Ipad3" , Price = 7000, Images = { new Image() {ImageUrl = "ipad3.jpg" },  new Image() {ImageUrl = "ipad6.jpg" }, new Image() {ImageUrl = "ipad5.jpg" }, new Image() {ImageUrl = "ipad3.jpg" } },Description ="<p>Bunda pubg ne oynanır</p>" },
            //26
            new Product(){ Name = "Tablet Casper" , Price = 3000, Images = { new Image() {ImageUrl = "tablet1.jpg" },  new Image() {ImageUrl = "tablet2.jpg" }, new Image() {ImageUrl = "tablet.jpg" }, new Image() {ImageUrl = "tablet2.jpg" } },Description ="<p>8 gigotyt ram var</p>" },
            //27
            new Product(){ Name = "Tablet" , Price = 4000, Images = { new Image() {ImageUrl = "tablet1.jpg" },  new Image() {ImageUrl = "tablet2.jpg" }, new Image() {ImageUrl = "tablet.jpg" }, new Image() {ImageUrl = "tablet2.jpg" } },Description ="<p>tablet</p>" },
            //28
            new Product(){ Name = "Monster notebook" , Price = 22000, Images = { new Image() {ImageUrl = "monster2.jpg" },  new Image() {ImageUrl = "monster.jpg" }, new Image() {ImageUrl = "monster2.jpg" }, new Image() {ImageUrl = "monster.jpg" } },Description ="<p>Güzel laptop</p>" },
            //29
            new Product(){ Name = "Excalibur notebook" , Price = 40000, Images = { new Image() {ImageUrl = "excalibur.jpg" },  new Image() {ImageUrl = "macpro.jpg" }, new Image() {ImageUrl = "monster.jpg" }, new Image() {ImageUrl = "slimeasus.jpg" } },Description ="<p>Güzel laptop</p>" },
            //30
            new Product(){ Name = "Lenovo IdeaPad" , Price = 8000, Images = { new Image() {ImageUrl = "slimeasus.jpg" },  new Image() {ImageUrl = "asus5.jpg" }, new Image() {ImageUrl = "asus7.jpg" }, new Image() {ImageUrl = "asus2.jpg" } },Description ="<p>Asus alınır ya</p>" },
            //31
            new Product(){ Name = "HP 2W6K4EA Pavilion" , Price = 12000, Images = { new Image() {ImageUrl = "asus7.jpg" },  new Image() {ImageUrl = "slimeasus.jpg" }, new Image() {ImageUrl = "asus2.jpg" }, new Image() {ImageUrl = "asus5.jpg" } },Description ="<p>Al bu kirazdan kalmaz birazdan</p>" },
            //32
            new Product(){ Name = "Asus X515JF Core i7" , Price = 17000, Images = { new Image() {ImageUrl = "asuslptp.jpg" },  new Image() {ImageUrl = "slimeasus.jpg" }, new Image() {ImageUrl = "asuslptp.jpg" }, new Image() {ImageUrl = "monster.jpg" } },Description ="<p>Asus X515JF Core i7</p>" },
            //33
            new Product(){ Name = "Dyson Süpürge" , Price = 35000, Images = { new Image() {ImageUrl = "dayson2.jpg" },  new Image() {ImageUrl = "dayson3.jpg" }, new Image() {ImageUrl = "dyson3.jpg" }, new Image() {ImageUrl = "dyson.jpg" } },Description ="<p>havanızı değiştirin</p>" },
            //34
            new Product(){ Name = "Ütü" , Price = 3000, Images = { new Image() {ImageUrl = "utu.jpg" },  new Image() {ImageUrl = "utu.jpg" }, new Image() {ImageUrl = "utu.jpg" }, new Image() {ImageUrl = "utu.jpg" } },Description ="<p>Güzel Laptop</p>" },
            //35
            new Product(){ Name = "Philips airfryer" , Price = 15000, Images = { new Image() {ImageUrl = "air.jpg" },  new Image() {ImageUrl = "air.jpg" }, new Image() {ImageUrl = "air.jpg" }, new Image() {ImageUrl = "air.jpg" } },Description ="<p>Yeni gelin Çeyizi</p>" },
            //36
            new Product(){ Name = "Karaca " , Price = 5000, Images = { new Image() {ImageUrl = "karaca.jpg" },  new Image() {ImageUrl = "karaca2.jpg" }, new Image() {ImageUrl = "karaca.jpg" }, new Image() {ImageUrl = "karaca2.jpg" } },Description ="<p>Karaca</p>" },
            //37
            new Product(){ Name = "Rowta Saç Düzleştirici" , Price =1000, Images = { new Image() {ImageUrl = "sacduz2.jpg" },  new Image() {ImageUrl = "sackurut2.jpg" }, new Image() {ImageUrl = "sackurut2.jpg" }, new Image() {ImageUrl = "sacduz2.jpg" } },Description ="<p>Kaliteli</p>" },
            //38
            new Product(){ Name = "Kendwood KATLE " , Price = 3000, Images = { new Image() {ImageUrl = "katle.jpg" },  new Image() {ImageUrl = "katle.jpg" }, new Image() {ImageUrl = "katle.jpg" }, new Image() {ImageUrl = "katle.jpg" } },Description ="<p>Kaliteli Katle</p>" },
            //39
            new Product(){ Name = "Tencere " , Price = 3000, Images = { new Image() {ImageUrl = "TencereSeti1.jpg" },  new Image() {ImageUrl = "TencereSeti2.jpg" }, new Image() {ImageUrl = "TencereSeti3.jpg" }, new Image() {ImageUrl = "TencereSeti4.jpg" } },Description ="<p>Tencere Seti</p>" },
            //40
            new Product(){ Name = "Logitech " , Price = 3500, Images = { new Image() {ImageUrl = "Logitech3.jpg" },  new Image() {ImageUrl = "Logitech2.jpg" }, new Image() {ImageUrl = "Logitech1.jpg" }, new Image() {ImageUrl = "Logitech.jpg" } },Description ="<p>Hocanın Mousu</p>" },
            //41
            new Product(){ Name = "Dell Monitör " , Price = 22000, Images = { new Image() {ImageUrl = "Dell.jpg" },  new Image() {ImageUrl = "Dell1.jpg" }, new Image() {ImageUrl = "Dell2.jpg" }, new Image() {ImageUrl = "Dell3.jpg" } },Description ="<p>Efsane Monitör</p>" },
            //42
            new Product(){ Name ="İphone 17 256 Lavanta" , Price = 88499, Images ={ new Image() { ImageUrl = "apple17.jpg" },new Image () { ImageUrl = "apple.17.jpg" }, new Image() {ImageUrl = "apple.17.1.jpg"},new Image() {ImageUrl ="apple..17.jpg" } },Description = "<p>İphone 17 Lavanta Rengi </p>"},
            //43
            new Product(){ Name ="İphone 15 256 Pembe" , Price = 46990, Images ={ new Image() { ImageUrl = "apple15.jpg" },new Image () { ImageUrl = "apple15.jpg" }, new Image() {ImageUrl = "apple.15.jpg"},new Image() {ImageUrl ="apple..15.jpg" } },Description = " <p>İphone 15 </p>"},
            //44
            new Product(){ Name ="İphone 15 256 Pembe" , Price = 46990, Images ={ new Image() { ImageUrl = "apple15.jpg" },new Image () { ImageUrl = "apple15.jpg" }, new Image() {ImageUrl = "apple.15.jpg"},new Image() {ImageUrl ="apple..15.jpg" } },Description = " <p>İphone 15 </p>"},
            //45
            new Product(){ Name ="İphone 16 Pro Max Çöl Titanyumu" , Price = 1099999, Images ={ new Image() { ImageUrl = "apple16max.jpg" },new Image () { ImageUrl = "apple16.max.jpg" }, new Image() {ImageUrl = "apple.16max.jpg"},new Image() {ImageUrl ="apple.16.max.jpg" } },Description = " <p>İphone 16 Pro Max 512GB Çöl Titanyumu </p>"},
            //46
             new Product(){ Name ="İphone 16" , Price = 59999, Images ={ new Image() { ImageUrl = "apple16.jpg" },new Image () { ImageUrl = "apple16..jpg" }, new Image() {ImageUrl = "apple.16.jpg"},new Image() {ImageUrl ="apple16...jpg" } },Description = " <p>İphone 16 128GB Deniz Mavisi </p>"},
            //47
             new Product(){ Name ="İphone 17 Pro Max Abis" , Price = 123999, Images ={ new Image() { ImageUrl = "apple17pro.jpg" },new Image () { ImageUrl = "apple17.pro.jpg" }, new Image() {ImageUrl = "apple.17..jpg"},new Image() {ImageUrl ="apple17pro...jpg" } },Description = " <p>İphone 17 256GB Abis </p>"},
            //48
             new Product(){ Name ="Dell Inspiron Bilgisayar" , Price = 23999, Images ={ new Image() { ImageUrl = "dell.jpg" },new Image () { ImageUrl = "dell.1.jpg" }, new Image() {ImageUrl = "dell..jpg"},new Image() {ImageUrl ="dell1.jpg" } },Description = " <p>Dell Inspiron 15 3530 13.Nesil Core i5 1334U-16GB Ssd-15-6inc-W11 </p>"},
            //49
             new Product(){ Name ="MacBook Air" , Price = 74999, Images ={ new Image() { ImageUrl = "macbook.mc7.jpg" },new Image () { ImageUrl = "macbook.mc.7.jpg" }, new Image() {ImageUrl = "macbook.mc7.jpg"},new Image() {ImageUrl ="macbookmc.jpg" } },Description = " <p>MacBook Air MC7D4TU/A M4 24GB-512GB Ssd-Liquid Retina -15-3 inc-GökMavisi </p>"},
            //50
             new Product(){ Name ="Lenova LOQ Bilgisayar " , Price = 84999, Images ={ new Image() { ImageUrl = "lenova.1.jpg" },new Image () { ImageUrl = "Lenova.1...jpg" }, new Image() {ImageUrl = "lenova.1..jpg"},new Image() {ImageUrl ="Lenova1.jpg" } },Description = " <p>Lenova LOQ 13.Nesil Core i7 13650HX-RTX5070 8GB-24GB-1TB Ssd-15-6-W11 </p>"},
            //51
             new Product(){ Name =" Msi Thin 15 13.Nesil" , Price = 43079, Images ={ new Image() { ImageUrl = "msı.jpg" },new Image () { ImageUrl = "msı1.jpg" }, new Image() {ImageUrl = "msı1..jpg"},new Image() {ImageUrl ="msı..jpg" } },Description = " <p>Msı Thin 15 13.Nesil Core i5 13420H-RTX4050 6Gb-512 Gb Ssd-15.6inc-W11 </p>"},
            //52
              new Product(){ Name ="Huawei Matebook" , Price = 24999, Images ={ new Image() { ImageUrl = "huawei.jpg" },new Image () { ImageUrl = "huawei1.jpg" }, new Image() {ImageUrl = "huawei..jpg"},new Image() {ImageUrl ="huawei.1.jpg" } },Description = " <p>Bilgisayar </p>"},
            //53
             new Product(){ Name ="Philips LatteGo" , Price = 24999, Images ={ new Image() { ImageUrl = "kahve.jpg" },new Image () { ImageUrl = "kahve1.jpg" }, new Image() {ImageUrl = "kahve.1.jpg"},new Image() {ImageUrl ="kahve..jpg" } },Description = " <p>Tam Otomatik Espresso Makinesi </p>"},
            //54
             new Product(){ Name ="Arzum Çay Sefası" , Price = 4199, Images ={ new Image() { ImageUrl = "çaycı.jpg" },new Image () { ImageUrl = "çaycı..jpg" }, new Image() {ImageUrl = "çaycıı.jpg"},new Image() {ImageUrl ="Çaycı.1.jpg" } },Description = " <p>Çaycı </p>"},
            //55
              new Product(){ Name ="Roborock" , Price = 23999, Images ={ new Image() { ImageUrl = "robot.sü.jpg" },new Image () { ImageUrl = "robot.süp.jpg" }, new Image() {ImageUrl = "robot.süpü.jpg"},new Image() {ImageUrl ="robo.jpg" } },Description = " <p>İphone 17 256GB Abis </p>"},
            //56
             new Product(){ Name ="Philips Ütü" , Price = 6499, Images ={ new Image() { ImageUrl = "philips.jpg" },new Image () { ImageUrl = "philipps.jpg" }, new Image() {ImageUrl = "philipss.jpg"},new Image() { ImageUrl ="philips"} },Description = " <p>İphone 17 256GB Abis </p>"},
            //57
             new Product(){ Name ="Canon Fotograf Makinesi" , Price = 42699, Images ={ new Image() { ImageUrl = "canon.jpg" },new Image () { ImageUrl = "canon...jpg" }, new Image() {ImageUrl = "canon.1.jpg"},new Image() {ImageUrl ="Canoon.jpg" } },Description = " <p> Güzel Fotoğraf Makinesi </p>"},
            //58
              new Product(){ Name ="İphone 17 Pro Max Abis" , Price = 123999, Images ={ new Image() { ImageUrl = "apple17pro.jpg" },new Image () { ImageUrl = "apple17.pro.jpg" }, new Image() {ImageUrl = "apple.17..jpg"},new Image() {ImageUrl ="apple17pro...jpg" } },Description = " <p>İphone 17 256GB Abis </p>"},
            //59
              new Product(){ Name ="Tefal İkili Tava" , Price = 1899, Images ={ new Image() { ImageUrl = "tefaal.jpg" },new Image () { ImageUrl = "tefaall.jpg" }, new Image() {ImageUrl = "tefal.jpg"},new Image() {ImageUrl ="tefall.jpg" } },Description = " <p>Çeyizlerin Gözdesi </p>"},
            //60
              new Product(){ Name ="Stanley Termos 0.47L" , Price = 1382, Images ={ new Image() { ImageUrl = "stanley.jpg" },new Image () { ImageUrl = "stannley.jpg" }, new Image() {ImageUrl = "stanleey.jpg"},new Image() {ImageUrl ="ter.jpg" } },Description = " <p>Termoslar Şahı</p>"},

        };
        private static ProductCategory[] ProductCategories =
        {
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[2],Category=Categories[1]},
            new ProductCategory(){Product=Products[3],Category=Categories[1]},
            new ProductCategory(){Product=Products[4],Category=Categories[1]},
            new ProductCategory(){Product=Products[5],Category=Categories[1]},
            new ProductCategory(){Product=Products[6],Category=Categories[1]},
            new ProductCategory(){Product=Products[7],Category=Categories[2]},
            new ProductCategory(){Product=Products[8],Category=Categories[4]},
            new ProductCategory(){Product=Products[9],Category=Categories[4]},
            new ProductCategory(){Product=Products[10],Category=Categories[4]},
            new ProductCategory(){Product=Products[11],Category=Categories[2]},
            new ProductCategory(){Product=Products[12],Category=Categories[0]},
            new ProductCategory(){Product=Products[13],Category=Categories[0]},
            new ProductCategory(){Product=Products[14],Category=Categories[2]},
            new ProductCategory(){Product=Products[15],Category=Categories[2]},
            new ProductCategory(){Product=Products[16],Category=Categories[2]},
            new ProductCategory(){Product=Products[17],Category=Categories[2]},
            new ProductCategory(){Product=Products[18],Category=Categories[2]},
            new ProductCategory(){Product=Products[19],Category=Categories[0]},
            new ProductCategory(){Product=Products[20],Category=Categories[0]},
            new ProductCategory(){Product=Products[21],Category=Categories[0]},
            new ProductCategory(){Product=Products[22],Category=Categories[0]},
            new ProductCategory(){Product=Products[24],Category=Categories[2]},
            new ProductCategory(){Product=Products[25],Category=Categories[2]},
            new ProductCategory(){Product=Products[26],Category=Categories[2]},
            new ProductCategory(){Product=Products[27],Category=Categories[2]},
            new ProductCategory(){Product=Products[28],Category=Categories[1]},
            new ProductCategory(){Product=Products[29],Category=Categories[1]},
            new ProductCategory(){Product=Products[30],Category=Categories[1]},
            new ProductCategory(){Product=Products[31],Category=Categories[1]},
            new ProductCategory(){Product=Products[32],Category=Categories[1]},
            new ProductCategory(){Product=Products[33],Category=Categories[2]},
            new ProductCategory(){Product=Products[34],Category=Categories[2]},
            new ProductCategory(){Product=Products[35],Category=Categories[2]},
            new ProductCategory(){Product=Products[36],Category=Categories[3]},
            new ProductCategory(){Product=Products[37],Category=Categories[5]},
            new ProductCategory(){Product=Products[38],Category=Categories[2]},
            new ProductCategory(){Product=Products[39],Category=Categories[3]},
            new ProductCategory(){Product=Products[40],Category=Categories[1]},
            new ProductCategory(){Product=Products[41],Category=Categories[1]},
            new ProductCategory(){Product=Products[23],Category=Categories[1]},
            new ProductCategory(){Product=Products[42],Category=Categories[0]},
            new ProductCategory(){Product=Products[43],Category=Categories[0]},
            new ProductCategory(){Product=Products[44],Category=Categories[0]},
            new ProductCategory(){Product=Products[45],Category=Categories[0]},
            new ProductCategory(){Product=Products[46],Category=Categories[0]},
            new ProductCategory(){Product=Products[47],Category=Categories[0]},
            new ProductCategory(){Product=Products[48],Category=Categories[1]},
            new ProductCategory(){Product=Products[49],Category=Categories[1]},
            new ProductCategory(){Product=Products[50],Category=Categories[1]},
            new ProductCategory(){Product=Products[51],Category=Categories[1]},
            new ProductCategory(){Product=Products[52],Category=Categories[1]},
            new ProductCategory(){Product=Products[53],Category=Categories[2]},
            new ProductCategory(){Product=Products[54],Category=Categories[2]},
            new ProductCategory(){Product=Products[55],Category=Categories[2]},
            new ProductCategory(){Product=Products[56],Category=Categories[2]},
            new ProductCategory(){Product=Products[57],Category=Categories[2]},
            new ProductCategory(){Product=Products[58],Category=Categories[0]},
            new ProductCategory(){Product=Products[59],Category=Categories[3]},
            new ProductCategory(){Product=Products[60],Category=Categories[6]}
        };


    }
}
