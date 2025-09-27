namespace Web.ResultMessages
{
    public static class Messages
    {
        public static class Article
        {
            public static string Add(string message)
            {
                return $"{message} başlıklı makale başarıyla yüklenmiştir.";
            }
            public static string Delete(string message)
            {
                return $"{message} başlıklı makale başarıyla silinmiştir.";
            }
            public static string UndoDelete(string message)
            {
                return $"{message} başlıklı makale başarıyla geri alınmıştır.";
            }
            public static string Update(string message)
            {
                return $"{message} başlıklı makale başarıyla güncellenmiştir."; 
            }

        }

        public static class Comment
        {
            public static string Add(string message)
            {
                return $"{message} kullanıcı yorumu başarıyla yüklenmiştir.";
            }
            public static string Delete(string message)
            {
                return $"{message} kullanıcı yorumu başarıyla silinmiştir.";
            }
            public static string UndoDelete(string message)
            {
                return $"{message} kullanıcı yorumu başarıyla geri alınmıştır.";
            }
            public static string Update(string message)
            {
                return $"{message} kullanıcı yorumu başarıyla güncellenmiştir.";
            }

            public static string Hard(string message)
            {
                return $"{message} kullanıcı yorumu veritabanından başarıyla silinmiştir.";
            }

        }


        public static class Category
        {
            public static string Add(string message)
            {
                return $"{message} başlıklı kategori başarıyla yüklenmiştir.";
            }
            public static string Delete(string message)
            {
                return $"{message} başlıklı kategori başarıyla silinmiştir.";
            }
            public static string Update(string message)
            {
                return $"{message} başlıklı kategori başarıyla güncellenmiştir.";
            }
            public static string UndoDelete(string message)
            {
                return $"{message} başlıklı kategori başarıyla geri alınmıştır.";
            }
        }
        public static class User
        {
            public static string Add(string message)
            {
                return $"{message} email hesabına sahip kullanıcı başarıyla yüklenmiştir.";
            }

            public static string Code(string message)
            {
                return $"{message} email hesabına kod başarıyla gönderildi.";
            }
            public static string Token()
            {
                return $"Opps Doğrulama Sırasında Bazı Şeyler Ters Gitti. Kayıt Ol Sayfasına Yönlendiriliyorsunuz.";
            }

            public static string UserToken()
            {
                return $"Opps Kişi eklenirken Bazı Sorunlar Oluştu. Kayıt Ol Sayfasına yönlendiriliyorsunuz.";
            }

            public static string Verify()
            {
                return $"E‑postası doğrulandı.Kişi Başarıyla Eklendi. Giriş yapabilirsiniz.";
            }
            public static string VerifyCodeNot()
            {
                return $"UserId Gecersiz.";
            }
            public static string ResendCode()
            {
                return $"Kod Tekrardan gönderildi.";
            }

            public static string VerifyCodeException()
            {
                return $"Kod gönderilirken hata oluştu.";
            }


            public static string Delete(string message)
            {
                return $"{message} email hesabına sahip kullanıcı başarıyla silinmiştir.";
            }
            public static string Update(string message)
            {
                return $"{message}  email hesabına sahip kullanıcı başarıyla güncellenmiştir.";
            }

        }

    }
}
