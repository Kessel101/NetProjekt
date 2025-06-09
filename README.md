
# 🚗 **NetProject – System zarządzania warsztatem samochodowym**

## 📚 Spis treści

1. [Opis projektu](#opis-projektu)  
2. [Główne funkcjonalności](#główne-funkcjonalności)  
3. [Technologie i biblioteki](technologie-i-biblioteki)  
4. [Uruchomienie projektu](uruchomienie-projektu)  
   * [Wymagania wstępne](wymagania-wstępne)  
   * [Instalacja](instalacja)  
   * [Konfiguracja](konfiguracja)  
5. [Logowanie i role użytkowników](logowanie-i-role-użytkowników)  
   * [Dostępne role](dostępne-role)  
   * [Domyślni użytkownicy](domyślni-użytkownicy)  
6. [Baza danych](baza-danych)  
7. [Testy](testy)  
   * [Testy jednostkowe](testy-jednostkowe)  
   * [Testy wydajnościowe NBomber](testy-wydajnościowe-nbomber)  
8. [Proces CI/CD](proces-cicd)  
9. [Struktura projektu](struktura-projektu)

---

## 🧾 Opis projektu

**NetProject** to aplikacja webowa ASP.NET Core przeznaczona do kompleksowego zarządzania zleceniami serwisowymi w warsztacie samochodowym. Umożliwia ewidencjonowanie klientów, przypisywanie pojazdów, prowadzenie historii napraw oraz zarządzanie zleceniami. System wykorzystuje role użytkowników oraz logowanie zdarzeń w celu zapewnienia kontroli dostępu i przejrzystości działania.

---

## ✨ Główne funkcjonalności

- ✅ **Zarządzanie klientami** (dodawanie, edycja, lista)  
- ✅ **Ewidencja pojazdów** przypisanych do klientów  
- ✅ **Obsługa zleceń serwisowych** (dodawanie części, przypisywanie zadań)  
- ✅ **System logowania i rejestracji** (ASP.NET Identity)  
- ✅ **Panel administracyjny** do zarządzania kontami  
- ✅ **Logowanie zdarzeń aplikacji** przez NLog  
- ✅ **Swagger UI** – dokumentacja REST API

---

## 🧰 Technologie i biblioteki

- **.NET**: ASP.NET Core 9.0  
- **Baza danych**: Entity Framework Core + SQL Server  
- **Frontend**: Razor Pages + Bootstrap  
- **Uwierzytelnianie**: ASP.NET Core Identity  
- **Mapowanie danych**: AutoMapper / Mapperly  
- **Logi**: NLog  
- **Testy jednostkowe**: xUnit, Moq  
- **Testy wydajnościowe**: NBomber  
- **API Docs**: Swagger (Swashbuckle.AspNetCore)

---

## 🚀 Uruchomienie projektu

### ✅ Wymagania wstępne

- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [SQL Server 2019 lub nowszy](https://www.microsoft.com/sql-server/sql-server-downloads)  
- Visual Studio 2022 / JetBrains Rider / VS Code

### 🛠 Instalacja

```bash
git clone https://github.com/Kessel101/NetProjekt.git
cd NetProjekt
```

### ⚙️ Konfiguracja

1. **Połączenie z bazą danych**  
W pliku `appsettings.json` zaktualizuj sekcję `ConnectionStrings`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=NetProjectDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

2. **Zastosuj migracje EF Core**  
Upewnij się, że masz zainstalowane CLI EF Core:

```bash
dotnet tool install --global dotnet-ef
```

Następnie wykonaj:

```bash
dotnet ef database update --project NetProject
```

3. **Uruchom aplikację**

```bash
dotnet build
dotnet run --project NetProject
```

Aplikacja będzie dostępna pod adresem:  
👉 http://localhost:5000

---

## 🔐 Logowanie i role użytkowników

Aplikacja używa ASP.NET Core Identity z podziałem na role:

### 🎭 Dostępne role

- **Admin**: pełna kontrola, dostęp do panelu administracyjnego  
- **User**: możliwość zarządzania zleceniami, klientami, pojazdami  

### 👥 Domyślni użytkownicy (inicjalizacja danych)

Użytkownicy tworzeni są automatycznie w klasie `DataInitializer.cs` po migracji bazy:

| Rola   | Login               | Hasło      |
|--------|---------------------|------------|
| Admin  | admin@example.com   | Admin123!  |
| User   | user@example.com    | User123!   |

---

## 🗃️ Baza danych

Projekt korzysta z podejścia **Code-First** w Entity Framework Core.  
Migracje znajdują się w folderze `Migrations`.

📌 Aby dodać nową migrację:

```bash
dotnet ef migrations add NazwaMigracji --project NetProject
```

---

## 🧪 Testy

### ✅ Testy jednostkowe

Zlokalizowane w projekcie `NetProject.Tests`:

```bash
dotnet test NetProject.Tests
```

Zawierają testy dla serwisów, kontrolerów i PDFów.

### 🚀 Testy wydajnościowe NBomber

Zlokalizowane w `NetProject.Tests/PerformanceTests/OrdersLoadTest.cs`.

#### 👉 Jak uruchomić:

1. Endpoint `/api/orders/active` musi być dostępny publicznie lub [AllowAnonymous].
2. Upewnij się, że masz dodaną paczkę:

```bash
dotnet add package NBomber
```

3. Uruchom test:

```bash
dotnet run --project NetProject.Tests
```

#### 📄 Raport PDF
Po zakończeniu testów wygenerowany zostanie plik:
```
nbomber-report.pdf
```

---

## 🔁 Proces CI/CD

Repozytorium zawiera workflow GitHub Actions:  
`.github/workflows/dotnet-ci.yml`

🔧 Wykonuje automatycznie:

- `dotnet restore`
- `dotnet build`
- `dotnet test`

Po każdym pushu lub pull requeście do gałęzi `main`.

---

## 📁 Struktura projektu

```
NetProject/
│
├── Controllers/         → kontrolery aplikacji
├── Data/                → DbContext, migracje, inicjalizacja danych
├── DTOs/                → obiekty DTO
├── Models/              → encje EF Core (Customer, Vehicle, Order, itd.)
├── Services/            → logika biznesowa (serwisy, interfejsy)
├── Views/               → Razor Pages (.cshtml)
├── ViewModels/          → klasy do komunikacji widok–model
├── wwwroot/             → pliki statyczne (Bootstrap, CSS, JS)
├── NetProject.Tests/    → testy jednostkowe + wydajnościowe
└── NetProject.sln       → plik rozwiązania .NET
```

---

## 📬 Kontakt

Autor repozytorium: [@Kessel101](https://github.com/Kessel101)  
Projekt na potrzeby przedmiotu – Politechnika Krakowska, semestr 4.
