using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace ETICARET.WebUI.Extensions
{
    public static class TempDataExtensions
    {
        //JsonConvert.SerializeObject methodu ile nesneyi JSON formatına çeviriyoruz ve Tempdataya string olarak kaydediyoruz
        //Bu extension methodlar sayesinde karmaşık nesneleri json formatında saklayabiliriz.
        //Yani tempdataya karmaşın nesneleri json formatında aktar diyoruz.
        
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }
        public static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            tempData.TryGetValue(key, out object? value);
            return value == null ? null : JsonConvert.DeserializeObject<T>((string)value);
            //value null ise eğer null dönder değilse json formatındaki stringi tekrar nesneye dönüştür.
        }
    }
}

/*
 * TEMPDATA HAKKINDA:
 * 
 * TempData, istekler arası veri taşımak için kullanılır.
 * Session'dan farkı, veri bir kez okunduktan sonra silinir.
 * 
 * KULLANIM ÖRNEĞİ:
 * 
 * // Veri kaydetme (Controller'da):
 * TempData.Put("message", new { Type = "success", Text = "İşlem başarılı!" });
 * 
 * // Veri okuma (View'da veya başka Controller'da):
 * var message = TempData.Get<MessageModel>("message");
 * 
 * NEDEN JSON?
 * - TempData sadece primitive tipler ve string saklar
 * - Karmaşık nesneleri saklamak için JSON'a çevirmemiz gerekir
 * - JSON, nesneyi string olarak saklar ve geri dönüştürür
 */