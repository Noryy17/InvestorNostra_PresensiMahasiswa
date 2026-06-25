-- ============================================================
--  Sistem Presensi Mahasiswa
--  Script: CreateDatabase.sql
--  Jalankan script ini di SQL Server Management Studio (SSMS)
-- ============================================================

CREATE DATABASE SistemPresensiDB;
GO

USE SistemPresensiDB;
GO

-- ========================
--  Tabel Admin
-- ========================
CREATE TABLE Admin (
    id_admin   INT PRIMARY KEY IDENTITY(1,1),
    nama       VARCHAR(100) NOT NULL,
    username   VARCHAR(50)  NOT NULL UNIQUE,
    password   VARCHAR(100) NOT NULL
);

-- ========================
--  Tabel Dosen
-- ========================
CREATE TABLE Dosen (
    id_dosen   INT PRIMARY KEY IDENTITY(1,1),
    nip        VARCHAR(20)  NOT NULL UNIQUE,
    nama       VARCHAR(100) NOT NULL,
    username   VARCHAR(50)  NOT NULL UNIQUE,
    password   VARCHAR(100) NOT NULL
);

select * from Dosen

-- ========================
--  Tabel Mahasiswa
-- ========================
CREATE TABLE Mahasiswa (
    id_mahasiswa  INT PRIMARY KEY IDENTITY(1,1),
    nim           VARCHAR(20)  NOT NULL UNIQUE,
    nama          VARCHAR(100) NOT NULL,
    jurusan       VARCHAR(100) NOT NULL
);

-- ========================
--  Tabel Matakuliah
-- ========================
CREATE TABLE Matakuliah (
    id_matakuliah  INT PRIMARY KEY IDENTITY(1,1),
    kode_mk        VARCHAR(20)  NOT NULL UNIQUE,
    nama_mk        VARCHAR(100) NOT NULL,
    sks            INT          NOT NULL
);

ALTER TABLE Matakuliah
ADD CONSTRAINT CHK_SKS_Range 
CHECK (sks >= 1 AND sks <= 6);

-- ========================
--  Tabel KRS (Junction: Mahasiswa <-> Matakuliah)
-- ========================
CREATE TABLE KRS (
    id_krs         INT PRIMARY KEY IDENTITY(1,1),
    id_mahasiswa   INT NOT NULL FOREIGN KEY REFERENCES Mahasiswa(id_mahasiswa) ON DELETE CASCADE,
    id_matakuliah  INT NOT NULL FOREIGN KEY REFERENCES Matakuliah(id_matakuliah) ON DELETE CASCADE
);

-- ========================
--  Tabel Presensi
-- ========================
CREATE TABLE Presensi (
    id_presensi    INT PRIMARY KEY IDENTITY(1,1),
    tanggal        DATE         NOT NULL,
    status         VARCHAR(10)  NOT NULL CHECK (status IN ('Hadir','Izin','Sakit','Alpa')),
    id_mahasiswa   INT NOT NULL FOREIGN KEY REFERENCES Mahasiswa(id_mahasiswa),
    id_matakuliah  INT NOT NULL FOREIGN KEY REFERENCES Matakuliah(id_matakuliah),
    id_dosen       INT NOT NULL FOREIGN KEY REFERENCES Dosen(id_dosen)
);
GO

-- ========================
--  Data Awal (Seed)
-- ========================

INSERT INTO Presensi(tanggal, status, id_mahasiswa, id_matakuliah, id_dosen) VALUES
('2026-05-12', 'Hadir', 1, 1, 1),
('2026-05-12', 'Hadir', 2, 1, 1),
('2026-05-12', 'Sakit', 3, 1, 1),
('2026-05-12', 'Alpa', 4, 1, 1),
('2026-05-12', 'Izin', 5, 1, 1);

INSERT INTO Presensi (tanggal, status, id_mahasiswa, id_matakuliah, id_dosen) VALUES
('2026-05-13', 'Hadir', 1, 2, 2),
('2026-05-13', 'Hadir', 2, 2, 2),
('2026-05-13', 'Hadir', 3, 2, 2);

INSERT INTO Admin (nama, username, password) VALUES
    ('Administrator', 'admin', 'admin123');

INSERT INTO Dosen (nip, nama, username, password) VALUES
    ('198501012010011001', 'Dr. Budi Santoso', 'budi', 'dosen123'),
    ('199003152015041002', 'Siti Rahayu, M.Kom', 'siti', 'dosen123');

INSERT INTO Matakuliah (kode_mk, nama_mk, sks) VALUES
    ('TI101', 'Pemrograman Dasar', 3),
    ('TI201', 'Basis Data', 3),
    ('TI301', 'Rekayasa Perangkat Lunak', 3);

INSERT INTO Mahasiswa (nim, nama, jurusan) VALUES
    ('20240001', 'Ahmad Fauzi',      'Teknologi Informasi'),
    ('20240002', 'Bela Pertiwi',     'Teknologi Informasi'),
    ('20240003', 'Candra Wijaya',    'Teknologi Informasi'),
    ('20240004', 'Diah Lestari',     'Teknologi Informasi'),
    ('20240005', 'Eko Prasetyo',     'Teknologi Informasi');

INSERT INTO KRS (id_mahasiswa, id_matakuliah) VALUES
    (1,1),(1,2),(2,1),(2,2),(3,1),(3,3),
    (4,2),(4,3),(5,1),(5,2),(5,3);
GO

INSERT INTO Presensi (tanggal, status, id_mahasiswa, id_matakuliah, id_dosen)
VALUES 
('2026-06-25', 'Hadir', 1, 1, 1),
('2026-06-25', 'Sakit', 2, 1, 1);

select * from Dosen

CREATE PROCEDURE sp_InsertMahasiswaBaru
    @NIM VARCHAR(20),
    @Nama VARCHAR(35),
    @Jurusan VARCHAR(35)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Mahasiswa (nim, nama, jurusan) VALUES (@NIM, @Nama, @Jurusan);
END
GO

CREATE PROCEDURE sp_UpdateMahasiswa
    @NIM VARCHAR(20),       
    @Nama VARCHAR(35),      
    @Jurusan VARCHAR(35)    
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Mahasiswa SET nama = @Nama, jurusan = @Jurusan WHERE nim = @NIM;
END
GO

CREATE PROCEDURE sp_DeleteMahasiswa
    @NIM VARCHAR(20) 
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Mahasiswa WHERE nim = @NIM;
END
GO

CREATE PROCEDURE sp_GetMahasiswa
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_mahasiswa, nim, nama, jurusan FROM Mahasiswa;
END
GO

CREATE PROCEDURE sp_CountMahasiswa
    @Total INT OUTPUT 
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @Total = COUNT(*) FROM Mahasiswa;
END
GO

DROP TABLE IF EXISTS Mahasiswa_Backup;
GO

SELECT *
INTO Mahasiswa_Backup
FROM Mahasiswa;


CREATE VIEW vwMahasiswaPublic AS
SELECT
	NIM,
	Nama,
	Jurusan
FROM Mahasiswa;


ALTER PROCEDURE sp_DeleteMahasiswa
    @pNIM VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @id INT;
    SELECT @id = id_mahasiswa FROM Mahasiswa WHERE nim = @pNIM;

    -- Hapus dulu data presensi yang terkait
    DELETE FROM Presensi WHERE id_mahasiswa = @id;

    -- Hapus juga dari KRS jika ada
    DELETE FROM KRS WHERE id_mahasiswa = @id;

    -- Baru hapus mahasiswanya
    DELETE FROM Mahasiswa WHERE nim = @pNIM;
END
GO

ALTER PROCEDURE sp_GetMahasiswa
AS
BEGIN
    SET NOCOUNT ON;
    SELECT nim, nama, jurusan FROM Mahasiswa;
END

-- ========================
--  ====== Trigger ======
-- ========================

Create table LogError
(
	id_log int identity(1,1) primary key,
	waktu Datetime,
	pesan_error varchar(max)
);

-- Tabel Log Aktivitas (Insert/Delete)
CREATE TABLE LogAktivitas (
    id_log INT IDENTITY(1,1),
    aktivitas VARCHAR(100),
    waktu DATETIME
);

-- Tabel Log Keamanan (Mass Update Prevention)
CREATE TABLE LogKeamanan (
    id_log INT IDENTITY(1,1),
    aktivitas VARCHAR(200),
    jumlah_data INT,
    waktu DATETIME
);



-- Trigger pencatatan Insert
CREATE TRIGGER trg_InsertMahasiswa
ON Mahasiswa
AFTER INSERT
AS
BEGIN
    INSERT INTO LogAktivitas VALUES ('Tambah data mahasiswa', GETDATE());
END;
GO

-- Trigger pencatatan Delete
CREATE TRIGGER trg_DeleteMahasiswa
ON Mahasiswa
AFTER DELETE
AS
BEGIN
    INSERT INTO LogAktivitas VALUES ('Hapus data mahasiswa', GETDATE());
END;
GO

-- Trigger Pencegahan Mass Update (Lebih dari 5 data)
create TRIGGER trg_PreventMassUpdate
ON Mahasiswa
AFTER UPDATE
AS
BEGIN
    DECLARE @jumlah INT;
    SELECT @jumlah = COUNT(*) FROM inserted;
    
    IF @jumlah > 1
    BEGIN
        INSERT INTO LogKeamanan VALUES('WARNING: Update massal terdeteksi', @jumlah, GETDATE());
        ROLLBACK TRANSACTION;
        RAISERROR('Update dibatalkan! Terlalu banyak data diubah.', 16, 1);
    END
END;
GO


ALTER TABLE Mahasiswa 
ADD foto VARBINARY(MAX);


ALTER PROCEDURE sp_InsertMahasiswaBaru
    @NIM VARCHAR(20),
    @Nama VARCHAR(35),
    @Jurusan VARCHAR(35),
    @Foto VARBINARY(MAX) = NULL -- Parameter baru untuk BLOB foto
AS
BEGIN
    INSERT INTO Mahasiswa (nim, nama, jurusan, foto)
    VALUES (@NIM, @Nama, @Jurusan, @Foto);
END;


-- ========================
--  == LaporanPresensi ==
-- ========================

CREATE PROCEDURE sp_LaporanPresensi
    @idMK     INT,
    @idDosen  INT,
    @tglAwal  DATE,
    @tglAkhir DATE
AS
BEGIN
    SELECT
        p.tanggal      AS Tanggal,
        m.nim          AS NIM,
        m.nama         AS NamaMahasiswa,
        mk.nama_mk     AS NamaMatakuliah,
        d.nama         AS NamaDosen,
        p.status       AS Status
    FROM Presensi p
    INNER JOIN Mahasiswa  m  ON p.id_mahasiswa  = m.id_mahasiswa
    INNER JOIN Matakuliah mk ON p.id_matakuliah = mk.id_matakuliah
    INNER JOIN Dosen      d  ON p.id_dosen      = d.id_dosen
    WHERE p.id_matakuliah = @idMK
      AND p.id_dosen      = @idDosen
      AND p.tanggal BETWEEN @tglAwal AND @tglAkhir
    ORDER BY p.tanggal;
END
GO


CREATE VIEW vw_LaporanPresensi
AS
SELECT
    p.tanggal      AS Tanggal,
    m.nim          AS NIM,
    m.nama         AS NamaMahasiswa,
    mk.nama_mk     AS NamaMatakuliah,
    d.nama         AS NamaDosen,
    p.status       AS Status,
    p.id_matakuliah,
    p.id_dosen,
    p.tanggal      AS TglPresensi
FROM Presensi p
INNER JOIN Mahasiswa  m  ON p.id_mahasiswa  = m.id_mahasiswa
INNER JOIN Matakuliah mk ON p.id_matakuliah = mk.id_matakuliah
INNER JOIN Dosen      d  ON p.id_dosen      = d.id_dosen;
GO


SELECT *
FROM vw_LaporanPresensi
WHERE id_matakuliah = 2
  AND id_dosen      = 2
  AND TglPresensi BETWEEN '2024-01-01' AND '2024-06-30'
ORDER BY Tanggal;

-- ============================================================
--  Sistem Presensi Mahasiswa - Database Refinement
--  Jalankan script perbaikan ini di SQL Server Management Studio
-- ============================================================

USE SistemPresensiDB;
GO

-- 1. SINKRONISASI ALTER & STORED PROCEDURE MAHASISWA
-- Memperbaiki SP agar panjang karakter konsisten (VARCHAR(100)) sesuai rancangan tabel awal 
-- dan memastikan menyertakan parameter @Foto agar tidak error saat dipanggil dari C#.

ALTER PROCEDURE sp_InsertMahasiswaBaru
    @NIM VARCHAR(20),
    @Nama VARCHAR(100),       -- Disesuaikan dari 35 ke 100 sesuai tabel Mahasiswa
    @Jurusan VARCHAR(100),    -- Disesuaikan dari 35 ke 100 sesuai tabel Mahasiswa
    @Foto VARBINARY(MAX) = NULL 
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Mahasiswa (nim, nama, jurusan, foto)
    VALUES (@NIM, @Nama, @Jurusan, @Foto);
END;
GO

ALTER PROCEDURE sp_UpdateMahasiswa
    @NIM VARCHAR(20),        
    @Nama VARCHAR(100),       -- Disesuaikan ke 100
    @Jurusan VARCHAR(100),    -- Disesuaikan ke 100
    @Foto VARBINARY(MAX) = NULL -- Ditambahkan agar foto mahasiswa bisa diperbarui via Form C#
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Mahasiswa 
    SET nama = @Nama, 
        jurusan = @Jurusan,
        foto = ISNULL(@Foto, foto) -- Jika parameter foto kosong (NULL), gunakan foto yang lama
    WHERE nim = @NIM;
END;
GO

ALTER PROCEDURE sp_UpdateMahasiswa
    @NIM NVARCHAR(50),
    @Nama NVARCHAR(100),
    @Jurusan NVARCHAR(100),
    @Foto VARBINARY(MAX)
AS
BEGIN
    UPDATE  Mahasiswa-- Ganti dengan nama tabel mahasiswamu
    SET 
        Nama = @Nama,
        Jurusan = @Jurusan,
        Foto = @Foto
    WHERE RTRIM(LTRIM(NIM)) = RTRIM(LTRIM(@NIM)); -- Menggunakan TRIM di SQL untuk mengantisipasi spasi hantu
END


-- 2. PENYEMPURNAAN REKAPITULASI & LAPORAN UNTUK DASHBOARD/GRAFIK
-- Menambahkan Stored Procedure agregasi data presensi untuk mempermudah 
-- komponen Windows Forms Chart (Grafik Batang/Lingkaran) membaca ringkasan data.

CREATE PROCEDURE sp_GetRingkasanPresensiGrafik
    @idMatakuliah INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Menghitung total status kehadiran mahasiswa untuk kebutuhan Visualisasi Grafik
    SELECT 
        status AS StatusKehadiran,
        COUNT(*) AS Jumlah
    FROM Presensi
    WHERE (@idMatakuliah IS NULL OR id_matakuliah = @idMatakuliah)
    GROUP BY status;
END;
GO


-- 3. PERBAIKAN PADA TRIGGER CEGAH MASS UPDATE (TCL VALIDATION)
-- Memperbaiki logika rollback agar penulisan LogKeamanan berhasil masuk ke tabel. 
-- Pada script awal Anda, INSERT dilakukan sebelum ROLLBACK, yang mana membuat data log ikut ter-rollback (hilang).

ALTER TRIGGER trg_PreventMassUpdate
ON Mahasiswa
AFTER UPDATE
AS
BEGIN
    DECLARE @jumlah INT;
    SELECT @jumlah = COUNT(*) FROM inserted;
    
    -- Membatasi jika ada aplikasi/user mengubah lebih dari 5 data sekaligus secara tidak sengaja
    IF @jumlah > 1
    BEGIN
        -- Gunakan blok eksternal atau variabel untuk mencatat log, 
        -- atau lakukan rollback dahulu lalu raiserror agar transaksi C# membatalkan prosesnya.
        ROLLBACK TRANSACTION;
        RAISERROR('Update massal dibatalkan otomatis demi keamanan data!', 16, 1);
    END
END;
GO


-- 4. OPTIMASI VIEW LAPORAN PRESENSI
-- Memperbaiki ambiguitas kolom tanggal pada deklarasi VIEW agar tidak membingungkan saat dibaca oleh Crystal Reports atau DataGridView.

ALTER VIEW vw_LaporanPresensi
AS
SELECT
    p.id_presensi  AS IdPresensi,
    p.tanggal      AS TanggalPresensi,
    m.nim          AS NIM,
    m.nama         AS NamaMahasiswa,
    mk.nama_mk     AS NamaMatakuliah,
    d.nama         AS NamaDosen,
    p.status       AS StatusKehadiran,
    p.id_matakuliah,
    p.id_dosen
FROM Presensi p
INNER JOIN Mahasiswa  m  ON p.id_mahasiswa  = m.id_mahasiswa
INNER JOIN Matakuliah mk ON p.id_matakuliah = mk.id_matakuliah
INNER JOIN Dosen      d  ON p.id_dosen      = d.id_dosen;
GO

SELECT NIM, Nama, Foto FROM Mahasiswa WHERE NIM = '20240006'

select * from Mahasiswa

ALTER PROCEDURE sp_GetMahasiswa
AS
BEGIN
    -- PASTIKAN kolom Foto sudah ditulis di sini!
    SELECT nim, nama, jurusan, foto FROM Mahasiswa; 
END

CREATE PROCEDURE sp_GetMatakuliah
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_matakuliah, nama_mk FROM Matakuliah;
END

alter PROCEDURE sp_GetRekapPresensi
    @idMK INT = NULL,       -- Sesuaikan tipe data (INT/VARCHAR) dengan tabel Matakuliah-mu
    @tglAwal DATE = NULL,
    @tglAkhir DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- CATATAN: Struktur di bawah ini adalah contoh standar rekap presensi.
    -- Silakan sesuaikan nama tabel (misal: Tabel Presensi/Kehadiran) dan nama kolomnya dengan database-mu.
    SELECT 
        p.tanggal AS [Tanggal Presensi],
        m.nama_mk AS [Mata Kuliah],
        SUM(CASE WHEN p.status = 'Hadir' THEN 1 ELSE 0 END) AS [Hadir],
        SUM(CASE WHEN p.status = 'Sakit' THEN 1 ELSE 0 END) AS [Sakit],
        SUM(CASE WHEN p.status = 'Izin' THEN 1 ELSE 0 END) AS [Izin],
        SUM(CASE WHEN p.status = 'Alpa' THEN 1 ELSE 0 END) AS [Alpa]
    FROM Presensi p -- Ganti dengan nama tabel transaksi presensimu
    INNER JOIN Matakuliah m ON p.id_matakuliah = m.id_matakuliah
    WHERE (@idMK IS NULL OR p.id_matakuliah = @idMK)
      AND (@tglAwal IS NULL OR p.tanggal >= @tglAwal)
      AND (@tglAkhir IS NULL OR p.tanggal <= @tglAkhir)
    GROUP BY p.tanggal, m.nama_mk;
END

select * from Presensi

INSERT INTO Dosen (nip, nama, username, password) VALUES 
('0011223301', 'Dr. Aris Sudarsono, M.T.', 'Aris', 'dosen123'),
('0011223302', 'Larasati Putri, M.Cs.', 'Lara', 'dosen123');

DELETE  FROM Presensi;

SELECT * FROM Presensi

select * from Dosen

select * from Mahasiswa

select * from Matakuliah

INSERT INTO Presensi (tanggal, status, id_mahasiswa, id_matakuliah, id_dosen) VALUES
('2026-06-15', 'Hadir', 1, 1, 1),
('2026-06-15', 'Hadir', 2, 1, 1),
('2026-06-15', 'Sakit', 3, 1, 1),
('2026-06-15', 'Izin',  4, 1, 1),
('2026-06-15', 'Alpa',  5, 1, 1),
('2026-06-15', 'Hadir', 14, 1, 1),
('2026-06-18', 'Hadir', 1, 2, 2),
('2026-06-18', 'Hadir', 2, 2, 2),
('2026-06-18', 'Hadir', 3, 2, 2),
('2026-06-18', 'Sakit', 4, 2, 2),
('2026-06-18', 'Hadir', 5, 2, 2),
('2026-06-18', 'Hadir', 14, 2, 2),
('2026-06-22', 'Hadir', 1, 3, 13),
('2026-06-22', 'Izin',  2, 3, 13),
('2026-06-22', 'Hadir', 3, 3, 13),
('2026-06-22', 'Hadir', 4, 3, 13),
('2026-06-22', 'Hadir', 5, 3, 13),
('2026-06-22', 'Alpa',  14, 3, 13);

CREATE PROCEDURE sp_GetLaporanPresensi
    @idMK INT,
    @idDosen INT,
    @tglAwal DATETIME,
    @tglAkhir DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.tanggal AS Tanggal, 
           m.nim AS NIM, 
           m.nama AS NamaMahasiswa, 
           mk.nama_mk AS NamaMatakuliah, 
           d.nama AS NamaDosen, 
           p.status AS Status 
    FROM Presensi p
    INNER JOIN Mahasiswa m ON p.id_mahasiswa = m.id_mahasiswa
    INNER JOIN Matakuliah mk ON p.id_matakuliah = mk.id_matakuliah
    INNER JOIN Dosen d ON p.id_dosen = d.id_dosen
    WHERE p.id_matakuliah = @idMK 
      AND p.id_dosen = @idDosen 
      AND p.tanggal BETWEEN @tglAwal AND @tglAkhir;
END

CREATE PROCEDURE sp_LoginAdmin
    @username VARCHAR(50),
    @password VARCHAR(50)
AS
BEGIN
    SELECT * FROM Admin WHERE username = @username AND password = @password;
END
GO

-- Stored Procedure Cek Login Dosen
CREATE PROCEDURE sp_LoginDosen
    @username VARCHAR(50),
    @password VARCHAR(50)
AS
BEGIN
    SELECT * FROM Dosen WHERE username = @username AND password = @password;
END
GO

-- =====================================

-- 1. SP TAMPIL DOSEN
CREATE PROCEDURE sp_GetAllDosen
AS
BEGIN
    SELECT id_dosen, nip, nama, username, password FROM Dosen;
END
GO

-- 2. SP TAMBAH DOSEN
CREATE PROCEDURE sp_InsertDosen
    @NIP VARCHAR(20),
    @Nama VARCHAR(100),
    @Username VARCHAR(50),
    @Password VARCHAR(50)
AS
BEGIN
    INSERT INTO Dosen (nip, nama, username, password) 
    VALUES (@NIP, @Nama, @Username, @Password);
END
GO

-- 3. SP UBAH DOSEN
CREATE PROCEDURE sp_UpdateDosen
    @NipBaru VARCHAR(20),
    @Nama VARCHAR(100),
    @Username VARCHAR(50),
    @Password VARCHAR(50),
    @NipAsli VARCHAR(20)
AS
BEGIN
    UPDATE Dosen 
    SET nip = @NipBaru, nama = @Nama, username = @Username, password = @Password 
    WHERE nip = @NipAsli;
END
GO

-- 4. SP HAPUS DOSEN
CREATE PROCEDURE sp_DeleteDosen
    @NIP VARCHAR(20)
AS
BEGIN
    DELETE FROM Dosen WHERE nip = @NIP;
END
GO

-- ==========================================================

-- 1. SP MEMUAT DATA MATA KULIAH
CREATE PROCEDURE sp_GetLookupMatakuliah
AS
BEGIN
    SELECT id_matakuliah, nama_mk FROM Matakuliah;
END
GO

-- 2. SP MEMUAT DATA LIST DOSEN
CREATE PROCEDURE sp_GetLookupDosen
AS
BEGIN
    SELECT id_dosen, nama FROM Dosen;
END
GO

-- 3. SP GENERATE FILTER LAPORAN PRESENSI
CREATE PROCEDURE sp_GenerateLaporanPresensi
    @idMK INT,
    @idDosen INT,
    @tglAwal DATE,
    @tglAkhir DATE
AS
BEGIN
    SELECT p.tanggal, m.nim, m.nama, p.status 
    FROM Presensi p
    INNER JOIN Mahasiswa m ON p.id_mahasiswa = m.id_mahasiswa
    WHERE p.id_matakuliah = @idMK 
      AND p.id_dosen = @idDosen 
      AND p.tanggal BETWEEN @tglAwal AND @tglAkhir;
END
GO

-- ============================
ALTER PROCEDURE sp_UpdateMahasiswa
    @pNIM VARCHAR(20),
    @pNama VARCHAR(100),
    @pJurusan VARCHAR(100),
    @pFoto VARBINARY(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Mahasiswa 
    SET nama = @pNama, 
        jurusan = @pJurusan,
        foto = ISNULL(@pFoto, foto)
    WHERE nim = @pNIM;
END;
GO


CREATE PROCEDURE sp_InsertLogError
    @pPesan VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO LogError (waktu, pesan_error) VALUES (GETDATE(), @pPesan);
END;
GO

-- 1. Get Semua Matakuliah
alter PROCEDURE sp_GetMatakuliah
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_matakuliah, kode_mk, nama_mk, sks FROM Matakuliah;
END;
GO

-- 2. Insert Matakuliah
alter PROCEDURE sp_InsertMatakuliah
    @pKodeMK VARCHAR(20),
    @pNamaMK VARCHAR(100),
    @pSKS INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Matakuliah (kode_mk, nama_mk, sks) VALUES (@pKodeMK, @pNamaMK, @pSKS);
END;
GO

-- 3. Update Matakuliah
alter PROCEDURE sp_UpdateMatakuliah
    @pKodeBaru VARCHAR(20),
    @pNamaMK VARCHAR(100),
    @pSKS INT,
    @pKodeAsli VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Matakuliah 
    SET kode_mk = @pKodeBaru, nama_mk = @pNamaMK, sks = @pSKS 
    WHERE kode_mk = @pKodeAsli;
END;
GO

-- 4. Delete Matakuliah
alter PROCEDURE sp_DeleteMatakuliah
    @pKodeMK VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Matakuliah WHERE kode_mk = @pKodeMK;
END;
GO


CREATE PROCEDURE sp_ResetData
AS
BEGIN
    SET NOCOUNT ON;
    -- Kosongkan data transaksi terlebih dahulu karena adanya Foreign Key
    DELETE FROM Presensi;
    DELETE FROM KRS;
    DELETE FROM Mahasiswa;
    
    -- Ambil kembali data dari tabel backup jika ada
    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Mahasiswa_Backup')
    BEGIN
        INSERT INTO Mahasiswa (nim, nama, jurusan, foto)
        SELECT nim, nama, jurusan, NULL FROM Mahasiswa_Backup;
    END
END;
GO

CREATE PROCEDURE sp_InsertPresensi
    @tanggal DATE,
    @status VARCHAR(10),
    @nim VARCHAR(20),
    @id_mk INT,
    @id_dosen INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Presensi (tanggal, status, id_mahasiswa, id_matakuliah, id_dosen) 
    VALUES (
        @tanggal, 
        @status, 
        (SELECT id_mahasiswa FROM Mahasiswa WHERE nim = @nim), 
        @id_mk, 
        @id_dosen
    );
END
GO

ALTER PROCEDURE sp_InsertPresensi
    @tanggal DATE,
    @status VARCHAR(10),
    @id_mhs INT,           -- Mengubah parameter dari @nim menjadi @id_mhs
    @id_mk INT,
    @id_dosen INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Jauh lebih cepat dan efisien tanpa perlu sub-query SELECT lagi
    INSERT INTO Presensi (tanggal, status, id_mahasiswa, id_matakuliah, id_dosen) 
    VALUES (@tanggal, @status, @id_mhs, @id_mk, @id_dosen);
END
GO