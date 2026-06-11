Specyfikacja integracji dla projektu MAUI
=====================================

Cel
----
Plik zawiera pełną listę endpointów, DTO, wymagań sieciowych i sugestii implementacyjnych potrzebnych, aby zbudować aplikację .NET MAUI korzystającą z tego backendu.

Base URL (podczas prototypu)
- Lokalnie przez hotspot: http://{IP_LAPTOPA}:5000 (np. http://192.168.43.10:5000)
- Alternatywnie publiczny tunel: ngrok http 5000 -> użyj zwróconego https://*.ngrok.io

Endpointy (stan obecny projektu)
- Users
  - POST /api/users/register  -- body: User (model), query: password -> zwraca UserDto
  - POST /api/users/login     -- query: email, password -> zwraca UserDto
  - GET  /api/users/{id}      -- zwraca UserDto

- Subscriptions
  - GET  /api/subscriptions/user/{userId}  -- lista subskrypcji użytkownika
  - GET  /api/subscriptions/{id}           -- pojedyncza subskrypcja
  - POST /api/subscriptions                  -- body: Subscription -> tworzy
  - PUT  /api/subscriptions/{id}           -- body: Subscription -> aktualizuje
  - DELETE /api/subscriptions/{id}         -- usuwa

- Notifications
  - GET  /api/notifications/pending
  - POST /api/notifications/{id}/mark-sent

- Categories
  - GET  /api/categories
  - POST /api/categories
  - GET  /api/categories/{id}

- Stats
  - GET /api/stats/user/{userId}/monthly?months={n}

Modele / DTO (istotne pola)
- User
  - Id: Guid
  - Email: string
  - PasswordHash: string (backend)
  - DisplayName: string?

- UserDto
  - Id, Email, DisplayName

- Category
  - Id, Name

- Subscription
  - Id: Guid
  - Title: string
  - Price: decimal
  - Currency: string (PLN)
  - RenewalDate: DateTime
  - UserId: Guid
  - CategoryId: Guid
  - NotificationsEnabled: bool
  - Cycle: BillingCycle (Monthly/Yearly/Weekly)
  - PaymentMethod: string?
  - ActiveSince: DateTime?
  - IsActive: bool

- SubscriptionDetailDto
  - wszystkie pola Subscription + CategoryName + YearlyCost (computed)

- MonthlyStats
  - CurrentMonthlyTotal, YearlyTotal, SubscriptionsCount, ByCategory (Dictionary<string,decimal>), Trend (List<MonthTrend>)

Autoryzacja
- Obecnie backend nie zwraca tokenów. Dla produkcji/prototypu zalecane: JWT.
- W MAUI: przechowuj token w SecureStorage i dołączaj nagłówek Authorization: Bearer {token} do HttpClient.

Sieć / prototypowanie (krok po kroku)
1. Włącz hotspot w telefonie i podłącz laptop.
2. Na laptopie: ipconfig -> zanotuj IPv4 karty Wi‑Fi.
3. Uruchom backend z publicznym nasłuchem: dotnet run --urls "http://0.0.0.0:5000" (Program.cs już skonfigurowany).
4. W Firewall Windows pozwól ruch przychodzący na port 5000.
5. W MAUI ustaw BaseUrl na http://{IP_LAPTOPA}:5000.
6. Android: w AndroidManifest.xml dodać android:usesCleartextTraffic="true" (tylko prototyp).

Rekomendowane pliki/komponenty w projekcie MAUI
- MauiProgram.cs
  - rejestracja HttpClient/Refit, rejestracja serwisów w DI
- Services/IApiService.cs / ApiService.cs (lub Refit interface)
- Models/ (kopie DTO: UserDto, SubscriptionDto, SubscriptionDetailDto, MonthlyStats, CategoryDto)
- ViewModels/ (LoginVM, DashboardVM, SubscriptionsVM, SubscriptionDetailVM, StatsVM)
- Views/ (LoginPage, DashboardPage, SubscriptionsPage, SubscriptionDetailPage, StatsPage)
- Helpers/NetworkHelper.cs (metody pomocnicze do testu hosta/IP)

Szybki przykład Refit interfejsu (MAUI) – użyć pakietu Refit
```csharp
public interface IBackendApi
{
	[Get("/api/subscriptions/user/{userId}")]
	Task<List<SubscriptionDto>> GetForUser(Guid userId);

	[Get("/api/subscriptions/{id}")]
	Task<SubscriptionDetailDto> GetSubscription(Guid id);

	[Post("/api/subscriptions")]
	Task<SubscriptionDto> CreateSubscription([Body] SubscriptionDto dto);

	[Put("/api/subscriptions/{id}")]
	Task UpdateSubscription(Guid id, [Body] SubscriptionDto dto);

	[Get("/api/stats/user/{userId}/monthly")] 
	Task<MonthlyStats> GetMonthlyStats(Guid userId, [AliasAs("months")] int months = 6);
}
```

Rejestracja w MauiProgram.cs
```csharp
builder.Services.AddRefitClient<IBackendApi>()
	.ConfigureHttpClient(c => c.BaseAddress = new Uri("http://192.168.43.10:5000"));
```

Przykład użycia w ViewModel
```csharp
public class DashboardViewModel
{
	private readonly IBackendApi _api;
	public ObservableCollection<SubscriptionDto> Subscriptions { get; } = new();

	public DashboardViewModel(IBackendApi api)
	{
		_api = api;
	}

	public async Task Load(Guid userId)
	{
		var list = await _api.GetForUser(userId);
		Subscriptions.Clear();
		foreach(var s in list) Subscriptions.Add(s);
	}
}
```

Mapowanie ekranów -> wymagane wywołania API (minimalny zestaw)
- Dashboard
  - GET /api/stats/user/{userId}/monthly  (statystyki, donut chart, trend)
  - GET /api/subscriptions/user/{userId}   (lista nadchodzących płatności)

- Subscriptions list
  - GET /api/subscriptions/user/{userId}
  - operacje: POST / PUT / DELETE

- Subscription detail
  - GET /api/subscriptions/{id}
  - PUT /api/subscriptions/{id}

- Categories
  - GET /api/categories (lista do filtrowania/kategorii)

- Notifications (opcjonalnie)
  - GET /api/notifications/pending
  - POST /api/notifications/{id}/mark-sent

Wskazówki dodatkowe
- Shared DTO: rozważ utworzenie biblioteki WirtualnyPortfelAPI.Shared zawierającej DTO i użycie jej w backend + MAUI, żeby uniknąć duplikacji.
- Wersjonowanie API: dodaj prefiks /api/v1/ jeśli planujesz zmiany.
- Błędy i retry: użyj Polly do retry/circuit-breaker w HttpClient.
- Logger: loguj błędy sieciowe i zwracaj przyjazne komunikaty na UI.

Pliki pomocnicze i skrypty które możesz dodać do repo
- scripts/quick-run.ps1  — uruchamia dotnet run i pokazuje lokalne IPv4
- docs/MAUI_README.md   — instrukcja konfiguracji BaseUrl, usesCleartextTraffic, ngrok

Troubleshooting (częste problemy)
- Telefon nie widzi laptopa: sprawdź hotspot, izolację klientów, lub użyj ngrok.
- Firewall blokuje port: dodaj regułę dla portu 5000 w Windows Firewall.
- Emulator Android: użyj 10.0.2.2 jako host dla Android Emulator, lub 10.0.3.2 dla Genymotion.
- Błąd CORS: backend ma politykę AllowAnyPrototype – upewnij się, że uruchomiłeś właściwy profil.

Gotowe kroki do startu prototypu (checklista)
1. Uruchom backend: dotnet run --urls "http://0.0.0.0:5000"  (lub uruchom z VS).
2. Włącz hotspot i podłącz laptop.
3. W MAUI ustaw BaseUrl na http://{IP_LAPTOPA}:5000 lub na ngrok URL.
4. Na Android dodać usesCleartextTraffic (jeśli HTTP).
5. Zarejestruj klient API (Refit/HttpClient) i zaimplementuj podstawowe ViewModel -> Views.

Jeśli chcesz, wygeneruję teraz:
- gotowy plik Refit interface (C#) dla wszystkich endpointów opisanych wyżej,
- lub skrypt NSwag do wygenerowania klienta z swagger.json (z backendu).

---
Plik wygenerowany automatycznie. Jeśli chcesz wersję angielską lub gotowe pliki C# do wklejenia do projektu MAUI, powiedz które zasoby generować.
