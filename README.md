# MarsRoverGame
Case Study

## Proje Detayları
Proje 3 ana yapıdan oluşmaktadır.
- Houston -> Web Interface
- Satellite -> Ana İşlem Yürütücü
- Rover -> Ana İşlem Yürütücüye bağlı nesneler

## Çalışma Çapısı
Satellite ve Houston modülleri paralel şekilde çalıştırılır. Houston modülü üzerinden, keşfedilecek alan boyutu maksimum 50x50 olacak şekilde girilir. Girilen alan boyutuna göre minimum 1 maksimum 10 adet olacak şekilde Rover modül örneklemi(instance) ayağa kaldırılır. Satellite modülü, her bir Rover Intance ile bağlantı kurar. Houston modülünden, oluşan her bir Rover için hareket komutu alınır. Hareket komutu Satellite modülünde işlenir ve uygunsa ilgili Roverı hareket ettirir.

## Bilgiler
- Proje .net 6 ve VS2022 ile geliştirildi. Test etmek için .net 6 SDK kurulu olmalıdır.
- Houston projesi kolay anlaşılabilir olması için tüm işlemler Pages/Index.cshtml sayfasında yapılmıştır. Projede her hangi bir js modülü yada frameworkü kullanılmadı.
- Satellite projesi bilgiler EntityFramework MemoryDB üzerinde tutulmuştur. Dependency Injection örneğine Services/RoverService.cs ve Services/SatelliteService.cs ulaşabilirsiniz. Yapılan diğer tüm işlemler Controllers/OrbitController.cs altından ulaşabilirsiniz.
- Rover projesinde işlevsel bir kod bloğu bulunmamaktadır. Sadece her bir rover için instance oluşturularak iletişim kurması örneklenmiştir.


> Projeyi Windows ortamda <Launch.bat> üzerinden çalıştırabilirsiniz

> Çalıştırma öncesi sertifika hatası alırsanız <TrustCerts.bat> dosyasını çalıştırdıktan sonra deneyebilirsiniz.

![This is an image](https://imgur.com/2G8awvs)

