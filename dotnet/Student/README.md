# Student MVC (EF Core) - Student & Course

## Ringkasan perubahan

Pada project `dotnet/Student/` sudah ditambahkan:

- **Entitas baru `Course`** dengan kolom:
  - **ID** (`Id`)
  - **Mata Kuliah** (`MataKuliah`)
  - **Dosen Pengampu** (`DosenPengampu`)
- **Relasi One-to-Many**:
  - 1 `Student` dapat memiliki banyak `Course`
  - `Course` memiliki `StudentId` (FK) ke `Student`
- **CRUD Course (MVC)**:
  - `CourseController` + Views (`Index/Create/Edit/Details/Delete`)
- **Tampilan daftar course per siswa**:
  - `Student/Index` menampilkan ringkas daftar mata kuliah per student
  - `Student/Details` menampilkan tabel course lengkap + tombol tambah course untuk student tsb

## File yang ditambahkan/diubah (inti)

- `Models/Course.cs` (baru)
- `Models/Students.cs` (ditambah navigation `Courses`)
- `Data/ApplicationDbContext.cs` (DbSet `Courses` + konfigurasi relasi)
- `Controllers/CourseController.cs` (baru)
- `Views/Course/*` (baru)
- `Views/Student/Index.cshtml`, `Views/Student/Details.cshtml` (ditambah tampilan course)
- `Views/Shared/_NavigationMenu.cshtml` (ditambah menu Course)
- `Migrations/*AddCourse*` (migration baru)

## Migrasi database (EF Core)

### Prasyarat

- .NET SDK terpasang
- EF Tools terpasang (jika belum):

```bash
dotnet tool install --global dotnet-ef
```

### 1) Masuk ke folder project

```bash
cd dotnet/Student
```

### 2) Buat migration baru (jika kamu mengubah model lagi)

Contoh:

```bash
dotnet ef migrations add NamaMigrationBaru
```

Untuk perubahan `Course` di tugas ini, migration yang dibuat adalah `AddCourse`.

### 3) Terapkan migration ke database

```bash
dotnet ef database update
```

## Catatan koneksi database

Di `Program.cs` saat ini project memakai:

- `UseSqlServer(connectionString)` dengan `ConnectionStrings:DefaultConnection`

Sehingga kamu butuh SQL Server berjalan. Repo ini sudah menyediakan `docker-compose.yaml` untuk SQL Server.

### Menjalankan SQL Server via Docker

```bash
cd dotnet/Student
docker compose up -d
```

Setelah SQL Server hidup, jalankan:

```bash
dotnet ef database update
dotnet run
```


