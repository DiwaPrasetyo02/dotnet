# Readme
## Model
- `Product`
  - Properti: `Id` (Guid), `Name`, `Description`, `Price`, `Stock`, `ImageUrl`.
  - Validasi data: `[Required]`, `[StringLength]`, `[Range]`, `[Url]`.
- `CartItem`
  - Properti: `ProductId`, `Name`, `Price`, `Quantity`, `Subtotal` (computed).
- `CartViewModel`
  - Menyimpan koleksi `CartItem` dan `Total` (computed).

## Service (DI)
- `IProductRepository` (contract) & `InMemoryProductRepository` (implementasi)
  - CRUD in-memory untuk `Product`.
  - Registrasi DI: **Singleton** (`AddSingleton`).
- `ICartService` (contract) & `SessionCartService` (implementasi)
  - Menyimpan keranjang di Session (serialize JSON).
  - Operasi: `GetCart`, `AddToCart`, `Decrease`, `Remove`, `Clear`.
  - Registrasi DI: **Scoped** (`AddScoped`), sesuai siklus request.
- `IRequestIdProvider` & `GuidRequestIdProvider`
  - Transient ID per-resolve untuk demo lifetime.
  - Registrasi DI: **Transient** (`AddTransient`).
- Infrastruktur
  - `AddHttpContextAccessor`, `AddSession` (30 menit), `UseSession` di pipeline.
  - `AddControllersWithViews`.

## Controller
- `HomeController`
  - `Index`: landing page, menyisipkan `RequestId` (Transient) ke `ViewBag`.
  - `Privacy`: bawaan template.
- `ProductsController` (admin CRUD, tanpa auth)
  - `Index`: daftar produk.
  - `Details(Guid id)`: detail produk.
  - `Create` GET/POST: tambah produk (model binding + validasi).
  - `Edit` GET/POST: ubah produk (cek `id` vs model, validasi).
  - `Delete` GET/POST: konfirmasi dan hapus.
  - Menggunakan `IProductRepository` dan `IRequestIdProvider`.
- `CatalogController` (customer)
  - `Index`: katalog produk.
  - `Cart`: tampilkan keranjang.
  - `AddToCart(Guid id, int qty)`: tambah produk ke keranjang.
  - `Decrease(Guid id, int qty)`: kurangi qty; hapus jika 0.
  - `Remove(Guid id)`: hapus item.
  - `Clear()`: kosongkan keranjang.
  - Menggunakan `IProductRepository` dan `ICartService`.

## View & Partial
- Layouts
  - `_Layout.cshtml`: layout utama (landing/katalog/keranjang), navbar ke Landing/Katalog/Keranjang/Admin.
  - `_AdminLayout.cshtml`: layout admin, sudah merender `@RenderSectionAsync("Scripts", required: false)`.
- Shared partials
  - `_Notification.cshtml`: tampilkan `TempData["Message"]` dan `RequestId`.
  - `_ProductCard.cshtml`: kartu produk + form tambah ke keranjang.
- Admin views (`Views/Products`)
  - `_Form.cshtml`: form reusable (Name, Description, Price, Stock, ImageUrl).
  - `Index`, `Create`, `Edit`, `Details`, `Delete`.
- Catalog views (`Views/Catalog`)
  - `Index`: grid produk dengan `_ProductCard`.
  - `Cart`: tabel keranjang, tombol +/-/hapus/kosongkan.
- Landing (`Views/Home/Index`)
  - Hero + ringkasan fitur (CRUD, Keranjang, DI).

## Pipeline (Program.cs)
- Services: `AddControllersWithViews`, `AddHttpContextAccessor`, `AddSession`.
- DI registrations: `IProductRepository` (Singleton), `ICartService` (Scoped), `IRequestIdProvider` (Transient).
- Middleware: `UseHttpsRedirection`, `UseStaticFiles`, `UseRouting`, `UseSession`, `UseAuthorization`.
- Default route: `{controller=Home}/{action=Index}/{id?}`.

## Alur utama
- Admin CRUD menggunakan repo in-memory → menambah/mengubah produk di koleksi (persist selama runtime).
- Customer melihat katalog dari repo yang sama (Singleton) → tambah ke keranjang (session) via `ICartService`.
- Keranjang tersimpan per user session; qty bisa ditambah/dikurangi, item dihapus, atau keranjang dikosongkan.
- Notifikasi menggunakan `TempData["Message"]`; RequestId menampilkan contoh DI Transient.

## Database & Migrasi (SQL Server)

### Prasyarat
- .NET SDK terpasang
- Docker Desktop terpasang
- EF Tools terpasang (jika belum):
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Menjalankan SQL Server via Docker

```bash
cd dotnet/E-Commerce
docker compose up -d
```

Tunggu beberapa detik sampai container SQL Server siap, lalu:

```bash
dotnet ef database update
dotnet run
```

### Migrasi Database

```bash
# Buat migration baru (jika ada perubahan model)
dotnet ef migrations add NamaMigrationBaru

# Terapkan migration ke database
dotnet ef database update
```

### Connection String

Project menggunakan SQL Server dengan connection string:
- **Production**: `DefaultConnection` di `appsettings.json`
- **Development**: `DefaultConnection` di `appsettings.Development.json`

Format: `Server=localhost,1433;Database=ECommerceDb;User Id=sa;Password=Str0ngP@ssw0rd!123;TrustServerCertificate=True;Encrypt=False`

### Catatan
- Database akan otomatis dibuat saat pertama kali menjalankan aplikasi (via `DbInitializer`).
- Migration akan otomatis diterapkan saat startup (via `Program.cs`).
- Data seed (3 produk contoh) akan otomatis ditambahkan jika tabel `Products` masih kosong.