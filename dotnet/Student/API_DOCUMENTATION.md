# Dokumentasi API - Student & Course Management

## 📋 Daftar Isi
1. [Overview](#overview)
2. [Base URL & Versioning](#base-url--versioning)
3. [Student API](#student-api)
4. [Course API](#course-api)
5. [Contoh Penggunaan](#contoh-penggunaan)
6. [Error Handling](#error-handling)

---

## Overview

API ini menyediakan CRUD (Create, Read, Update, Delete) untuk mengelola data **Student** dan **Course** dengan dukungan **API Versioning**. API menggunakan Entity Framework Core dengan SQL Server sebagai database.

### Fitur Utama:
- ✅ **API Versioning** (v1.0 dan v2.0)
- ✅ **RESTful API** dengan standar HTTP methods
- ✅ **Relasi One-to-Many** (Student → Courses)
- ✅ **Validasi Data** (StudentId validation untuk Course)
- ✅ **Error Handling** yang proper
- ✅ **Swagger Documentation** di `/swagger`

---

## Base URL & Versioning

### Base URL
```
http://localhost:5229/api/v{version}/
```

### Versi API yang Tersedia
- **v1.0** - Versi lengkap dengan semua data dan navigation properties
- **v2.0** - Versi ringkas dengan response yang dioptimasi

### Cara Menggunakan Versioning
1. **Via URL Path**: `/api/v1/StudentsApi` atau `/api/v2/StudentsApi`
2. **Via Query String**: `/api/StudentsApi?api-version=1.0`
3. **Via Header**: `api-version: 1.0`

---

## Student API

### Model Student
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "age": 20,
  "courses": [] // Navigation property (optional)
}
```

---

### API v1.0 - StudentsApi

#### 1. GET - Daftar Semua Student
**Endpoint:** `GET /api/v1/StudentsApi`

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 20,
    "courses": null
  },
  {
    "id": 2,
    "name": "Jane Smith",
    "email": "jane@example.com",
    "age": 22,
    "courses": null
  }
]
```

**Contoh Request (JavaScript):**
```javascript
fetch('/api/v1/StudentsApi')
  .then(res => res.json())
  .then(data => console.log(data));
```

---

#### 2. GET - Detail Student by ID
**Endpoint:** `GET /api/v1/StudentsApi/{id}`

**Parameters:**
- `id` (int, required) - ID Student

**Response:** `200 OK`
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "age": 20,
  "courses": null
}
```

**Error Response:** `404 Not Found` jika ID tidak ditemukan

**Contoh Request:**
```javascript
fetch('/api/v1/StudentsApi/1')
  .then(res => res.json())
  .then(data => console.log(data));
```

---

#### 3. POST - Tambah Student Baru
**Endpoint:** `POST /api/v1/StudentsApi`

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "age": 20
}
```

**Response:** `201 Created`
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "age": 20,
  "courses": null
}
```

**Contoh Request:**
```javascript
fetch('/api/v1/StudentsApi', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    name: 'John Doe',
    email: 'john@example.com',
    age: 20
  })
})
.then(res => res.json())
.then(data => console.log(data));
```

---

#### 4. PUT - Update Student
**Endpoint:** `PUT /api/v1/StudentsApi/{id}`

**Parameters:**
- `id` (int, required) - ID Student yang akan diupdate

**Request Body:**
```json
{
  "id": 1,
  "name": "John Doe Updated",
  "email": "john.updated@example.com",
  "age": 21
}
```

**Response:** `204 No Content` (berhasil)

**Error Responses:**
- `400 Bad Request` - jika ID di URL tidak match dengan ID di body
- `404 Not Found` - jika Student tidak ditemukan

**Contoh Request:**
```javascript
fetch('/api/v1/StudentsApi/1', {
  method: 'PUT',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    id: 1,
    name: 'John Doe Updated',
    email: 'john.updated@example.com',
    age: 21
  })
});
```

---

#### 5. DELETE - Hapus Student
**Endpoint:** `DELETE /api/v1/StudentsApi/{id}`

**Parameters:**
- `id` (int, required) - ID Student yang akan dihapus

**Response:** `204 No Content` (berhasil)

**Error Response:** `404 Not Found` jika Student tidak ditemukan

**Catatan:** Hapus Student akan otomatis menghapus semua Course terkait (Cascade Delete)

**Contoh Request:**
```javascript
fetch('/api/v1/StudentsApi/1', {
  method: 'DELETE'
});
```

---

### API v2.0 - StudentsApi

#### GET - Daftar Student (Ringkas)
**Endpoint:** `GET /api/v2/StudentsApi`

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com"
  },
  {
    "id": 2,
    "name": "Jane Smith",
    "email": "jane@example.com"
  }
]
```

**Perbedaan dengan v1.0:**
- Hanya mengembalikan `Id`, `Name`, dan `Email`
- Tidak mengembalikan `Age` dan `Courses`
- Response lebih ringkas, cocok untuk aplikasi mobile atau bandwidth terbatas

---

## Course API

### Model Course
```json
{
  "id": 1,
  "mataKuliah": "Matematika",
  "dosenPengampu": "Dr. Ahmad",
  "studentId": 1,
  "student": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 20
  }
}
```

---

### API v1.0 - CourseApi

#### 1. GET - Daftar Semua Course
**Endpoint:** `GET /api/v1/CourseApi`

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "mataKuliah": "Matematika",
    "dosenPengampu": "Dr. Ahmad",
    "studentId": 1,
    "student": {
      "id": 1,
      "name": "John Doe",
      "email": "john@example.com",
      "age": 20
    }
  }
]
```

**Fitur:**
- Mengembalikan semua Course dengan **navigation property Student** lengkap
- Cocok untuk aplikasi yang butuh data lengkap

**Contoh Request:**
```javascript
fetch('/api/v1/CourseApi')
  .then(res => res.json())
  .then(data => console.log(data));
```

---

#### 2. GET - Detail Course by ID
**Endpoint:** `GET /api/v1/CourseApi/{id}`

**Parameters:**
- `id` (int, required) - ID Course

**Response:** `200 OK`
```json
{
  "id": 1,
  "mataKuliah": "Matematika",
  "dosenPengampu": "Dr. Ahmad",
  "studentId": 1,
  "student": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 20
  }
}
```

**Contoh Request:**
```javascript
fetch('/api/v1/CourseApi/1')
  .then(res => res.json())
  .then(data => console.log(data));
```

---

#### 3. GET - Daftar Course by Student ID
**Endpoint:** `GET /api/v1/CourseApi/ByStudent/{studentId}`

**Parameters:**
- `studentId` (int, required) - ID Student

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "mataKuliah": "Matematika",
    "dosenPengampu": "Dr. Ahmad",
    "studentId": 1,
    "student": {
      "id": 1,
      "name": "John Doe",
      "email": "john@example.com",
      "age": 20
    }
  },
  {
    "id": 2,
    "mataKuliah": "Fisika",
    "dosenPengampu": "Dr. Budi",
    "studentId": 1,
    "student": {
      "id": 1,
      "name": "John Doe",
      "email": "john@example.com",
      "age": 20
    }
  }
]
```

**Fitur:**
- Mengembalikan semua Course yang dimiliki oleh Student tertentu
- Berguna untuk menampilkan daftar mata kuliah per siswa

**Contoh Request:**
```javascript
fetch('/api/v1/CourseApi/ByStudent/1')
  .then(res => res.json())
  .then(data => console.log(data));
```

---

#### 4. POST - Tambah Course Baru
**Endpoint:** `POST /api/v1/CourseApi`

**Request Body:**
```json
{
  "mataKuliah": "Matematika",
  "dosenPengampu": "Dr. Ahmad",
  "studentId": 1
}
```

**Response:** `201 Created`
```json
{
  "id": 1,
  "mataKuliah": "Matematika",
  "dosenPengampu": "Dr. Ahmad",
  "studentId": 1,
  "student": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 20
  }
}
```

**Validasi:**
- ✅ `StudentId` harus ada di database
- ✅ `MataKuliah` wajib diisi (max 150 karakter)
- ✅ `DosenPengampu` wajib diisi (max 150 karakter)

**Error Response:** `400 Bad Request`
```json
{
  "error": "StudentId tidak ditemukan"
}
```

**Contoh Request:**
```javascript
fetch('/api/v1/CourseApi', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    mataKuliah: 'Matematika',
    dosenPengampu: 'Dr. Ahmad',
    studentId: 1
  })
})
.then(res => res.json())
.then(data => console.log(data));
```

---

#### 5. PUT - Update Course
**Endpoint:** `PUT /api/v1/CourseApi/{id}`

**Parameters:**
- `id` (int, required) - ID Course yang akan diupdate

**Request Body:**
```json
{
  "id": 1,
  "mataKuliah": "Matematika Lanjutan",
  "dosenPengampu": "Dr. Ahmad",
  "studentId": 1
}
```

**Response:** `204 No Content` (berhasil)

**Error Responses:**
- `400 Bad Request` - jika ID tidak match atau StudentId tidak ditemukan
- `404 Not Found` - jika Course tidak ditemukan

**Contoh Request:**
```javascript
fetch('/api/v1/CourseApi/1', {
  method: 'PUT',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    id: 1,
    mataKuliah: 'Matematika Lanjutan',
    dosenPengampu: 'Dr. Ahmad',
    studentId: 1
  })
});
```

---

#### 6. DELETE - Hapus Course
**Endpoint:** `DELETE /api/v1/CourseApi/{id}`

**Parameters:**
- `id` (int, required) - ID Course yang akan dihapus

**Response:** `204 No Content` (berhasil)

**Error Response:** `404 Not Found` jika Course tidak ditemukan

**Contoh Request:**
```javascript
fetch('/api/v1/CourseApi/1', {
  method: 'DELETE'
});
```

---

### API v2.0 - CourseApi

#### GET - Daftar Course (Ringkas)
**Endpoint:** `GET /api/v2/CourseApi`

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "mataKuliah": "Matematika",
    "dosenPengampu": "Dr. Ahmad",
    "studentId": 1,
    "studentName": "John Doe"
  }
]
```

**Perbedaan dengan v1.0:**
- Tidak mengembalikan object `student` lengkap
- Hanya mengembalikan `studentName` sebagai string
- Response lebih ringkas, cocok untuk aplikasi mobile

---

#### GET - Detail Course (Ringkas)
**Endpoint:** `GET /api/v2/CourseApi/{id}`

**Response:** `200 OK`
```json
{
  "id": 1,
  "mataKuliah": "Matematika",
  "dosenPengampu": "Dr. Ahmad",
  "studentId": 1,
  "studentName": "John Doe"
}
```

---

#### GET - Course by Student (Ringkas)
**Endpoint:** `GET /api/v2/CourseApi/ByStudent/{studentId}`

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "mataKuliah": "Matematika",
    "dosenPengampu": "Dr. Ahmad",
    "studentId": 1,
    "studentName": "John Doe"
  }
]
```

---

#### POST - Tambah Course (Ringkas Response)
**Endpoint:** `POST /api/v2/CourseApi`

**Request Body:** (sama dengan v1.0)
```json
{
  "mataKuliah": "Matematika",
  "dosenPengampu": "Dr. Ahmad",
  "studentId": 1
}
```

**Response:** `201 Created` (ringkas)
```json
{
  "id": 1,
  "mataKuliah": "Matematika",
  "dosenPengampu": "Dr. Ahmad",
  "studentId": 1,
  "studentName": "John Doe"
}
```

---

## Contoh Penggunaan Lengkap

### Scenario: Menambah Student dan Course-nya

```javascript
// 1. Tambah Student baru
const newStudent = await fetch('/api/v1/StudentsApi', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    name: 'John Doe',
    email: 'john@example.com',
    age: 20
  })
}).then(res => res.json());

console.log('Student created:', newStudent);

// 2. Tambah Course untuk Student tersebut
const newCourse = await fetch('/api/v1/CourseApi', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    mataKuliah: 'Matematika',
    dosenPengampu: 'Dr. Ahmad',
    studentId: newStudent.id
  })
}).then(res => res.json());

console.log('Course created:', newCourse);

// 3. Ambil semua Course milik Student
const studentCourses = await fetch(`/api/v1/CourseApi/ByStudent/${newStudent.id}`)
  .then(res => res.json());

console.log('Student courses:', studentCourses);
```

---

## Error Handling

### HTTP Status Codes

| Status Code | Arti | Kapan Terjadi |
|------------|------|---------------|
| `200 OK` | Success | GET request berhasil |
| `201 Created` | Created | POST request berhasil membuat resource baru |
| `204 No Content` | Success (No Body) | PUT/DELETE request berhasil |
| `400 Bad Request` | Bad Request | Request body tidak valid, ID tidak match, atau StudentId tidak ditemukan |
| `404 Not Found` | Not Found | Resource (Student/Course) tidak ditemukan |
| `500 Internal Server Error` | Server Error | Error di server (database, dll) |

### Contoh Error Response

**400 Bad Request:**
```json
{
  "error": "StudentId tidak ditemukan"
}
```

**404 Not Found:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

---

## Testing dengan Swagger

1. **Akses Swagger UI:**
   ```
   http://localhost:5229/swagger
   ```

2. **Pilih API Version:**
   - Dropdown di kanan atas: pilih "Student API V1" atau "Student API V2"

3. **Test Endpoint:**
   - Klik endpoint yang ingin di-test
   - Klik "Try it out"
   - Isi parameter jika diperlukan
   - Klik "Execute"
   - Lihat response di bagian bawah

---

## Catatan Penting

1. **API Versioning:**
   - v1.0: Full data dengan navigation properties
   - v2.0: Ringkas, cocok untuk mobile/bandwidth terbatas

2. **Relasi One-to-Many:**
   - 1 Student bisa punya banyak Course
   - Hapus Student akan otomatis hapus semua Course-nya (Cascade Delete)

3. **Validasi:**
   - Course memerlukan `StudentId` yang valid
   - Semua field required harus diisi

4. **Content-Type:**
   - Selalu gunakan `Content-Type: application/json` untuk POST/PUT

---

## Summary Endpoint

### Student API
- `GET /api/v1/StudentsApi` - List semua student
- `GET /api/v1/StudentsApi/{id}` - Detail student
- `POST /api/v1/StudentsApi` - Tambah student
- `PUT /api/v1/StudentsApi/{id}` - Update student
- `DELETE /api/v1/StudentsApi/{id}` - Hapus student
- `GET /api/v2/StudentsApi` - List student (ringkas)

### Course API
- `GET /api/v1/CourseApi` - List semua course
- `GET /api/v1/CourseApi/{id}` - Detail course
- `GET /api/v1/CourseApi/ByStudent/{studentId}` - Course by student
- `POST /api/v1/CourseApi` - Tambah course
- `PUT /api/v1/CourseApi/{id}` - Update course
- `DELETE /api/v1/CourseApi/{id}` - Hapus course
- `GET /api/v2/CourseApi` - List course (ringkas)
- `GET /api/v2/CourseApi/{id}` - Detail course (ringkas)
- `GET /api/v2/CourseApi/ByStudent/{studentId}` - Course by student (ringkas)
- `POST /api/v2/CourseApi` - Tambah course (ringkas response)

---

**Dokumentasi ini dibuat untuk memudahkan penggunaan API Student & Course Management.**

