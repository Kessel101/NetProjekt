# NetProjekt

README.md z opisem projektu, logowania, rolami (zarówno tymi wewnątrz projektu takimi jak recepcjonista, mechanik, admin, jak i podział na obowiązki kto co robi)

## CI/CD

Repozytorium zawiera skonfigurowany workflow GitHub Actions (`.github/workflows/dotnet-ci.yml`), który automatycznie wykonuje następujące zadania przy każdym pushu lub pull requeście do gałęzi `main`:

- **Restore dependencies**: Pobranie wszystkich niezbędnych pakietów.
- **Build**: Kompilacja projektu przy użyciu `dotnet build`.
- **Test**: Uruchomienie testów jednostkowych przy użyciu `dotnet test`.

Dzięki temu procesowi mamy pewność, że każda zmiana wprowadzona do kodu jest natychmiast kompilowana i testowana, co ułatwia wczesne wykrywanie błędów oraz poprawia jakość kodu.
