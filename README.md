# **NetProject \- System zarządzania warsztatem samochodowym**

## **Spis treści**

1. [Opis projektu](#opis-projektu)  
2. [Główne funkcjonalności](#główne-funkcjonalności)  
3. [Technologie i biblioteki](#technologie-i-biblioteki)  
4. [Uruchomienie projektu](#uruchomienie-projektu)  
   * [Wymagania wstępne](#wymagania-wstępne)  
   * [Instalacja](#instalacja)  
   * [Konfiguracja](#konfiguracja)  
5. [Logowanie i role użytkowników](#logowanie-i-role-użytkowników)  
   * [Dostępne role](#dostępne-role)  
   * [Domyślni użytkownicy](#domyślni-użytkownicy)  
6. [Baza danych](#baza-danych)  
7. [Proces CI/CD](#proces-cicd)  
8. [Struktura projektu](#struktura-projektu)

## **Opis projektu**

**NetProject** to aplikacja webowa oparta na technologii ASP.NET Core, stworzona do zarządzania zleceniami serwisowymi w warsztacie samochodowym. System umożliwia ewidencję klientów, ich pojazdów oraz historii napraw. Aplikacja wspiera różne poziomy dostępu dzięki systemowi ról.

## **Główne funkcjonalności**

* **Zarządzanie klientami**: Dodawanie, edycja i przeglądanie danych klientów.  
* **Ewidencja pojazdów**: Przypisywanie pojazdów do klientów.  
* **Obsługa** zleceń **serwisowych**: Tworzenie nowych zleceń, przypisywanie zadań, dodawanie części i komentarzy.  
* **System uwierzytelniania**: Logowanie i rejestracja użytkowników.  
* **Panel administracyjny**: Zarządzanie użytkownikami i ich rolami.  
* **Logowanie zdarzeń**: Zapisywanie logów aplikacji przy użyciu NLog.

## **Technologie i biblioteki**

* **Backend**: ASP.NET Core 9.0  
* **Baza danych**: Entity Framework Core, SQL Server  
* **Uwierzytelnianie**: ASP.NET Core Identity  
* **Frontend**: Razor Pages, Bootstrap  
* **Logowanie**: NLog  
* **Mapowanie obiektów**: AutoMapper / Mapperly  
* **API**: Swagger (Swashbuckle)

## **Uruchomienie projektu**

### **Wymagania wstępne**

* [ASP.NET Core: .NET 9.0](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-9.0?view=aspnetcore-9.0)  
* [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (wersja Developer)  
* Dowolne IDE (Visual Studio, JetBrains Rider, VS Code)

### **Instalacja**

1. Sklonuj repozytorium:  
   git clone https://github.com/Kessel101/NetProjekt

2. Otwórz projekt w wybranym IDE i/lub wykonaj komendę:
   `cd NetProject` w swojej powłoce (np. Powershell).

### **Konfiguracja**

1. **Baza danych**:  
   * W pliku appsettings.json zaktualizuj ConnectionStrings, aby wskazywały na Twoją instancję SQL Server, np. w przypadku domyślnego localhosta:  
    ` "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TestDb;Trusted_Connection=True;TrustServerCertificate=True;"`
  }

2. **Migracje bazy danych**:  
   * Upewnij się, że masz zainstalowane narzędzia EF Core CLI. Jeśli nie, wykonaj polecenie:  
     `dotnet tool install \--global dotnet-ef`

   * Zastosuj migracje, aby utworzyć schemat bazy danych. W głównym folderze projektu (NetProject) wykonaj:  
     `dotnet ef database update`

3. **Uruchomienie**:  
   * Zbuduj, a następnie uruchom aplikację z poziomu IDE lub za pomocą poleceń: 
     1. `dotnet build` 
     2. `dotnet run`

Aplikacja będzie dostępna pod adresem http://localhost:5000.

## **Logowanie i role użytkowników**

System wykorzystuje ***ASP.NET Core Identity*** do zarządzania uwierzytelnianiem i autoryzacją.

### **Dostępne role**

* **Admin**: Pełny dostęp do systemu, w tym do panelu administracyjnego, gdzie może zarządzać kontami innych użytkowników.  
* **User**: Standardowy użytkownik (np. pracownik serwisu), który może zarządzać klientami, pojazdami i zleceniami, ale nie ma dostępu do panelu administracyjnego.

### **Domyślni użytkownicy**

Po pierwszym uruchomieniu i zastosowaniu migracji, w systemie tworzeni są domyślni użytkownicy (dzięki DataInitializer.cs):

* **Administrator**:  
  * **Login**: admin@example.com  
  * **Hasło**: Admin123\!  
* **Użytkownik standardowy**:  
  * **Login**: user@example.com  
  * **Hasło**: User123\!

## **Baza danych**

Projekt korzysta z ***Entity Framework Core*** jako ORM. Podejście "Code-First" oznacza, że schemat bazy danych jest generowany na podstawie modeli zdefiniowanych w kodzie (w folderze Models).

Wszystkie migracje znajdują się w folderze Migrations. Aby dodać nową migrację, wykonaj polecenie w folderze projektu:

dotnet ef migrations add NazwaMigracji

## **Proces CI/CD**

Repozytorium zawiera skonfigurowany workflow GitHub Actions (`.github/workflows/dotnet-ci.yml`), który automatycznie wykonuje następujące zadania przy każdym pushu lub pull requeście do gałęzi `main`:

- **Restore dependencies**: Pobranie wszystkich niezbędnych pakietów.
- **Build**: Kompilacja projektu przy użyciu `dotnet build`.
- **Test**: Uruchomienie testów jednostkowych przy użyciu `dotnet test`.

Dzięki temu procesowi mamy pewność, że każda zmiana wprowadzona do kodu jest natychmiast kompilowana i testowana, co ułatwia wczesne wykrywanie błędów oraz poprawia jakość kodu.

## **Struktura projektu**

Najważniejsze foldery w projekcie NetProject:

* Controllers: Kontrolery MVC obsługujące żądania HTTP.  
* Data: Kontekst bazy danych (MyAppDbContext), migracje oraz inicjalizator danych.  
* DTOs: (Data Transfer Objects) Obiekty używane do transferu danych między warstwami (np. między kontrolerem a widokiem).  
* Models: Klasy reprezentujące encje w bazie danych (np. Customer, Vehicle).  
* Services: Logika biznesowa aplikacji.  
* Views: Pliki .cshtml (Razor) definiujące interfejs użytkownika.  
* ViewModels: Modele przeznaczone specjalnie dla widoków, łączące dane z różnych źródeł.  
* wwwroot: Pliki statyczne, takie jak CSS, JavaScript i biblioteki front-endowe.