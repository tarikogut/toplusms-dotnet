using Microsoft.EntityFrameworkCore;
using Toplusms.Models;

namespace Toplusms.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await SeedBtkReferencesAsync(db);

        if (await db.Tenants.AnyAsync()) return;

        // Roles
        var adminRole = new Role { Name = "admin" };
        var bayiRole = new Role { Name = "bayi" };
        var musteriRole = new Role { Name = "musteri" };
        db.Roles.AddRange(adminRole, bayiRole, musteriRole);
        await db.SaveChangesAsync();

        // Host tenant
        var hostTenant = new Tenant
        {
            Domain = "toplusms.link",
            Name = "Toplusms",
            Code = "HOST",
            Slug = "toplusms",
            Type = "host",
            Status = "active",
            TaxNumber = "1234567890",
            TaxOffice = "Kadıköy VD",
            CustomerType = "T",
            Phone = "08501234567",
            Email = "info@toplusms.link"
        };
        db.Tenants.Add(hostTenant);
        await db.SaveChangesAsync();

        // Admin user
        var adminUser = new User
        {
            TenantId = hostTenant.Id,
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Name = "Admin",
            Surname = "User",
            Email = "admin@toplusms.link",
            RoleId = adminRole.Id,
            Status = "active"
        };
        db.Users.Add(adminUser);
        await db.SaveChangesAsync();

        // Demo musteri user
        var musteriUser = new User
        {
            TenantId = hostTenant.Id,
            Username = "musteri",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Name = "Müşteri",
            Surname = "User",
            Email = "musteri@toplusms.link",
            RoleId = musteriRole.Id,
            Status = "active"
        };
        db.Users.Add(musteriUser);
        await db.SaveChangesAsync();

        // Demo headers
        db.SMSHeaders.AddRange(
            new SMSHeader { TenantId = hostTenant.Id, Name = "TOPLUSMS" },
            new SMSHeader { TenantId = hostTenant.Id, Name = "SMS PANEL" }
        );
        await db.SaveChangesAsync();

        // Demo bank accounts
        db.BankAccounts.Add(new BankAccount
        {
            TenantId = hostTenant.Id,
            BankName = "Ziraat Bankası",
            BranchCode = "1234",
            AccountNumber = "1234567890",
            Iban = "TR12 3456 7890 1234 5678 9012 34",
            HolderName = "Toplusms"
        });
        await db.SaveChangesAsync();

        // Demo bayi tenant
        var bayiTenant = new Tenant
        {
            Domain = "bayi01.toplusms.link",
            Name = "Test Bayi",
            Code = "BAYI01",
            Slug = "test-bayi",
            Type = "bayi",
            Status = "active",
            ParentTenantId = hostTenant.Id,
            TaxNumber = "9876543210",
            TaxOffice = "Üsküdar VD",
            CustomerType = "T",
            Phone = "05321112233",
            Email = "bayi@test.com"
        };
        db.Tenants.Add(bayiTenant);
        await db.SaveChangesAsync();

        // Demo musteri tenant (under bayit)
        var musteriTenant = new Tenant
        {
            Domain = "mus01.toplusms.link",
            Name = "Test Müşteri",
            Code = "MUS01",
            Slug = "test-musteri",
            Type = "musteri",
            Status = "active",
            ParentTenantId = bayiTenant.Id,
            CustomerType = "B",
            Phone = "05441112233",
            Email = "musteri@test.com"
        };
        db.Tenants.Add(musteriTenant);
        await db.SaveChangesAsync();

        // Bayi kullanıcısı
        db.Users.Add(new User
        {
            TenantId = bayiTenant.Id,
            Username = "bayi",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Name = "Bayi",
            Surname = "Kullanıcı",
            Email = "bayi@test.com",
            RoleId = bayiRole.Id,
            Status = "active"
        });
        await db.SaveChangesAsync();

        // Müşteri kullanıcısı (alt tenant)
        db.Users.Add(new User
        {
            TenantId = musteriTenant.Id,
            Username = "musteri",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Name = "Müşteri",
            Surname = "Kullanıcı",
            Email = "musteri@test.com",
            RoleId = musteriRole.Id,
            Status = "active"
        });
        await db.SaveChangesAsync();
    }

    private static async Task SeedBtkReferencesAsync(AppDbContext db)
    {
        if (await db.Occupations.AnyAsync()) return;

        // EK-5 Meslek Kodlari (ISCO 08)
        db.Occupations.AddRange(
            new Occupation { Code = 11, Name = "Kanun yapıcılar ve üst düzey yöneticiler" },
            new Occupation { Code = 12, Name = "Genel müdürler ve başkanlar (özel sektör)" },
            new Occupation { Code = 13, Name = "İş hizmetleri ve idaresi ile ilgili müdürler" },
            new Occupation { Code = 14, Name = "Satış, pazarlama ve iş geliştirme ile ilgili müdürler" },
            new Occupation { Code = 21, Name = "Fizik ve yer bilimleri ile ilgili profesyonel meslek mensupları" },
            new Occupation { Code = 22, Name = "Matematikçiler, istatistikçiler ve aktüerler" },
            new Occupation { Code = 23, Name = "Yaşam bilimleri ile ilgili profesyonel meslek mensupları" },
            new Occupation { Code = 24, Name = "Mühendislik ile ilgili profesyonel meslek mensupları" },
            new Occupation { Code = 25, Name = "Elektroteknoloji mühendisleri" },
            new Occupation { Code = 26, Name = "Mimarlar, planlamacılar, harita mühendisleri ve tasarımcılar" },
            new Occupation { Code = 31, Name = "Fizik ve mühendislik bilimleri teknisyenleri" },
            new Occupation { Code = 32, Name = "Maden, imalat ve inşaat süpervizörleri" },
            new Occupation { Code = 33, Name = "İşlem kontrol teknisyenleri" },
            new Occupation { Code = 34, Name = "Yaşam bilimleri teknisyenleri ve ilgili yardımcı profesyonel meslek mensupları" },
            new Occupation { Code = 35, Name = "Gemi ve hava taşıtı kontrolörleri ve teknisyenleri" },
            new Occupation { Code = 41, Name = "Genel büro elemanları" },
            new Occupation { Code = 42, Name = "Sekreterler (genel)" },
            new Occupation { Code = 43, Name = "Klavye kullanan operatörler" },
            new Occupation { Code = 51, Name = "Seyahatlerde hizmet veren elemanlar, kondüktörler ve rehberler" },
            new Occupation { Code = 52, Name = "Aşçılar" },
            new Occupation { Code = 53, Name = "Garsonlar ve barmenler" },
            new Occupation { Code = 61, Name = "Bahçıvanlar ve bitkisel ürün yetiştiricileri" },
            new Occupation { Code = 62, Name = "Hayvan yetiştiricileri" },
            new Occupation { Code = 71, Name = "Kaba inşaat ve ilgili işlerde çalışan sanatkarlar" },
            new Occupation { Code = 72, Name = "İnşaatı tamamlayıcı işlerde çalışan sanatkarlar" },
            new Occupation { Code = 73, Name = "Badana, boya ve bina dış yüzey temizliği işlerinde çalışan sanatkarlar" },
            new Occupation { Code = 74, Name = "Metal levha ve inşaat malzemesi, kaynakçılar ve ilgili işlerde çalışanlar" },
            new Occupation { Code = 75, Name = "Demirciler, alet yapımcıları ve ilgili işlerde çalışanlar" },
            new Occupation { Code = 81, Name = "Madencilik ve mineral işleme tesisi operatörleri" },
            new Occupation { Code = 82, Name = "Metal işleme ve perdahlama tesisi operatörleri" }
        );

        // EK-4 Kimlik Tipleri
        db.IdentityTypes.AddRange(
            new IdentityType { Code = "TCKK", Name = "TC Çipli Kimlik Kartı", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCNC", Name = "TC Nüfus Cüzdanı", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCYK", Name = "TC Yabancı Kimlik Belgesi", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "YKN" },
            new IdentityType { Code = "TCPC", Name = "TC Pasaportu (Çipli e-Pasaport)", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCPL", Name = "TC Pasaportu (Eski Tip Lacivert)", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCPY", Name = "TC Pasaportu (Eski Tip Yeşil)", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCPG", Name = "TC Pasaportu (Eski Tip Gri)", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCPK", Name = "TC Pasaportu (Eski Tip Kırmızı)", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCGP", Name = "TC Pasaportu (Geçici)", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "YP", Name = "Yabancı Pasaport", HasSerialNo = true, IdentityNoField = "OPTIONAL" },
            new IdentityType { Code = "AC", Name = "Uçuş mürettebatı belgesi", HasSerialNo = true, IdentityNoField = "OPTIONAL" },
            new IdentityType { Code = "GC", Name = "Gemi adamı cüzdanı", HasSerialNo = true, IdentityNoField = "OPTIONAL" },
            new IdentityType { Code = "NE", Name = "NATO Emri belgesi", HasSerialNo = true, IdentityNoField = "OPTIONAL" },
            new IdentityType { Code = "SB", Name = "Seyahat Belgesi", HasSerialNo = true, IdentityNoField = "OPTIONAL" },
            new IdentityType { Code = "HB", Name = "Hudut Geçiş Belgesi", HasSerialNo = true, IdentityNoField = "OPTIONAL" },
            new IdentityType { Code = "GK", Name = "Gemi Komutanı Onaylı Personel Listesi", HasSerialNo = true, IdentityNoField = "OPTIONAL" },
            new IdentityType { Code = "TCSC", Name = "TC Sürücü Belgesi", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCHS", Name = "TC Hakim/Savcı Mesleki Kimlik Kartı", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCSV", Name = "TC Avukatlık Belgesi", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCGK", Name = "TC Geçici Kimlik Belgesi", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCMA", Name = "TC Mavi Kart", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCEV", Name = "TC Uluslararası aile cüzdanı", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TCNT", Name = "Noter Kimlik Kartı", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TBMM", Name = "TBMM Milletvekili Kimlik Kartı", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "TSK", Name = "TSK Kimlik Kartı", CountryCode = "TUR", HasSerialNo = true, IdentityNoField = "TCKN" },
            new IdentityType { Code = "KKTC", Name = "KKTC Kimlik Kartı", CountryCode = "KKT", HasSerialNo = true, IdentityNoField = "OPTIONAL" }
        );

        // EK-3 Hizmet Tipleri
        db.ServiceTypes.AddRange(
            new ServiceType { Code = "POSTPAID_GSM", Name = "Postpaid GSM", OperatorType = "MOBIL", Category = "MOBIL Hizmeti" },
            new ServiceType { Code = "PREPAID_GSM", Name = "Prepaid GSM", OperatorType = "MOBIL", Category = "MOBIL Hizmeti" },
            new ServiceType { Code = "PREPAID_INTERNET", Name = "Prepaid Internet", OperatorType = "MOBIL", Category = "MOBIL Hizmeti" },
            new ServiceType { Code = "POSTPAID_INTERNET", Name = "Postpaid Internet", OperatorType = "MOBIL", Category = "MOBIL Hizmeti" },
            new ServiceType { Code = "ADSL", Name = "Adsl", OperatorType = "ISS", Category = "IP Hizmeti" },
            new ServiceType { Code = "FIBER", Name = "Fiber", OperatorType = "ISS", Category = "IP Hizmeti" },
            new ServiceType { Code = "WIFI", Name = "WiFi", OperatorType = "ISS", Category = "IP Hizmeti" },
            new ServiceType { Code = "UYDU_INTERNET", Name = "Uydu üzerinden internet", OperatorType = "UHH", Category = "IP Hizmeti" },
            new ServiceType { Code = "UYDU_SES", Name = "Uydu üzerinden ses", OperatorType = "UHH", Category = "Ses Hizmeti" },
            new ServiceType { Code = "KABLONET", Name = "Kablo internet", OperatorType = "ISS", Category = "IP Hizmeti" },
            new ServiceType { Code = "SABIT_HAT", Name = "Sabit Hat", OperatorType = "PSTN", Category = "Telefon Hizmeti" },
            new ServiceType { Code = "ISDN_PRI", Name = "ISDN PRI", OperatorType = "PSTN", Category = "Telefon Hizmeti" },
            new ServiceType { Code = "SMS", Name = "SMS / Toplu SMS", OperatorType = "PSTN", Category = "SMS Hizmeti" },
            new ServiceType { Code = "SANAL_SANTRAL", Name = "Sanal Santral", OperatorType = "PSTN", Category = "Telefon Hizmeti" },
            new ServiceType { Code = "SERVIS_444", Name = "Servis 444", OperatorType = "PSTN", Category = "Telefon Hizmeti" },
            new ServiceType { Code = "KIRALIK_DEVRE_DATA", Name = "Kiralık Devre (Data)", OperatorType = "AIH", Category = "Data Hizmeti" },
            new ServiceType { Code = "VPN_MPLS", Name = "VPN MPLS", OperatorType = "AIH", Category = "Data Hizmeti" }
        );

        // EK-1 Hat Durum Kodlari
        db.LineStatuses.AddRange(
            new LineStatus { Code = 1, Status = "A", Description = "AKTIF" },
            new LineStatus { Code = 2, Status = "I", Description = "IPTAL_NUMARA_DEGISIKLIGI" },
            new LineStatus { Code = 3, Status = "I", Description = "IPTAL" },
            new LineStatus { Code = 4, Status = "I", Description = "IPTAL_SAHTE_EVRAK" },
            new LineStatus { Code = 5, Status = "I", Description = "IPTAL_MUSTERI_TALEBI" },
            new LineStatus { Code = 6, Status = "I", Description = "IPTAL_DEVIR" },
            new LineStatus { Code = 7, Status = "I", Description = "IPTAL_HAT_BENIM_DEGIL" },
            new LineStatus { Code = 8, Status = "I", Description = "IPTAL_KARA_LISTE" },
            new LineStatus { Code = 9, Status = "I", Description = "IPTAL_KULLANIM_DISI" },
            new LineStatus { Code = 10, Status = "I", Description = "IPTAL_EKSIK_EVRAK" },
            new LineStatus { Code = 11, Status = "I", Description = "IPTAL_SEHVEN_GIRIS" },
            new LineStatus { Code = 12, Status = "I", Description = "IPTAL_BAGLI_URUN_IPTALI" },
            new LineStatus { Code = 13, Status = "K", Description = "KISITLI_KONTUR_BITTI" },
            new LineStatus { Code = 14, Status = "K", Description = "KISITLI_ARAMAYA_KAPALI" },
            new LineStatus { Code = 15, Status = "D", Description = "DONDURULMUS_MUSTERI_TALEBI" },
            new LineStatus { Code = 16, Status = "D", Description = "DONDURULMUS_ISLETME" }
        );

        // EK-2 Musteri Hareket Kodlari
        db.CustomerMovements.AddRange(
            new CustomerMovement { Code = 1, Name = "YENI_ABONELIK_KAYDI", Description = "Yeni abone kaydı" },
            new CustomerMovement { Code = 2, Name = "HAT_DURUM_DEGISIKLIGI", Description = "Aktif->Suspend, Suspend->Aktif" },
            new CustomerMovement { Code = 3, Name = "SIM_KART_DEGISIKLIGI", Description = "SIM kart değişikliği" },
            new CustomerMovement { Code = 4, Name = "ODEME_TIPI_DEGISIKLIGI", Description = "Postpaid-Prepaid geçişi" },
            new CustomerMovement { Code = 5, Name = "ADRES_DEGISIKLIGI", Description = "Adres değişikliği" },
            new CustomerMovement { Code = 6, Name = "IMSI_DEGISIKLIGI", Description = "IMSI değişikliği" },
            new CustomerMovement { Code = 7, Name = "TARIFE_DEGISIKLIGI", Description = "Tarife değişikliği" },
            new CustomerMovement { Code = 8, Name = "DEVIR_MUSTERI_DEGISIKLIGI", Description = "Müşteri devir" },
            new CustomerMovement { Code = 9, Name = "NUMARA_DEGISIKLIGI", Description = "Numara değişikliği" },
            new CustomerMovement { Code = 10, Name = "HAT_IPTAL", Description = "Hat iptali (Numara Taşıma Hariç)" },
            new CustomerMovement { Code = 11, Name = "MUSTERI_BILGI_DEGISIKLIGI", Description = "Müşteri bilgi değişikliği" },
            new CustomerMovement { Code = 12, Name = "NUMARA_TASIMA", Description = "Numara taşıma (diğer operatöre)" },
            new CustomerMovement { Code = 13, Name = "NUMARA_DEGISMEDEN_NAKIL", Description = "Numara değişmeden nakil (sabit)" },
            new CustomerMovement { Code = 14, Name = "NUMARA_DEGISTIREREK_NAKIL", Description = "Numara değiştirerek nakil (sabit)" },
            new CustomerMovement { Code = 15, Name = "IP_DEGISIKLIGI", Description = "IP değişikliği" }
        );

        await db.SaveChangesAsync();
    }
}
