-- 1. Membuat Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SistemPresensiDB')
BEGIN
    CREATE DATABASE SistemPresensiDB;
END
GO

USE SistemPresensiDB;
GO

-- Membuat Tabel

-- Tabel Admin
CREATE TABLE Admin (
    id_admin   INT PRIMARY KEY IDENTITY(1,1),
    nama       VARCHAR(35) NOT NULL,
    username   VARCHAR(35) NOT NULL UNIQUE,
    password   VARCHAR(35) NOT NULL
);

-- Tabel Dosen
CREATE TABLE Dosen (
    id_dosen   INT PRIMARY KEY IDENTITY(1,1),
    nip        VARCHAR(20) NOT NULL UNIQUE,
    nama       VARCHAR(35) NOT NULL,
    username   VARCHAR(35) NOT NULL UNIQUE,
    password   VARCHAR(355) NOT NULL
);

-- Tabel Mahasiswa
CREATE TABLE Mahasiswa (
    id_mahasiswa  INT PRIMARY KEY IDENTITY(1,1),
    nim           VARCHAR(20) NOT NULL UNIQUE,
    nama          VARCHAR(35) NOT NULL,
    jurusan       VARCHAR(35) NOT NULL
);

-- Tabel Matakuliah
CREATE TABLE Matakuliah (
    id_matakuliah  INT PRIMARY KEY IDENTITY(1,1),
    kode_mk        VARCHAR(20) NOT NULL UNIQUE,
    nama_mk        VARCHAR(35) NOT NULL,
    sks            INT          NOT NULL
);

-- Tabel KRS (Junction: Mahasiswa <-> Matakuliah)
CREATE TABLE KRS (
    id_krs         INT PRIMARY KEY IDENTITY(1,1),
    id_mahasiswa   INT NOT NULL FOREIGN KEY REFERENCES Mahasiswa(id_mahasiswa) ON DELETE CASCADE,
    id_matakuliah  INT NOT NULL FOREIGN KEY REFERENCES Matakuliah(id_matakuliah) ON DELETE CASCADE
);

-- Tabel Presensi
CREATE TABLE Presensi (
    id_presensi    INT PRIMARY KEY IDENTITY(1,1),
    tanggal        DATE         NOT NULL,
    status         VARCHAR(10)  NOT NULL CHECK (status IN ('Hadir','Izin','Sakit','Alpa')),
    id_mahasiswa   INT NOT NULL FOREIGN KEY REFERENCES Mahasiswa(id_mahasiswa),
    id_matakuliah  INT NOT NULL FOREIGN KEY REFERENCES Matakuliah(id_matakuliah),
    id_dosen       INT NOT NULL FOREIGN KEY REFERENCES Dosen(id_dosen)
);
GO

--Data Awal

INSERT INTO Admin (nama, username, password) VALUES ('Administrator', 'admin', 'admin123');

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

-- View dan backup

-- Pembuatan View untuk publik
CREATE VIEW vwMahasiswaPublic AS
SELECT
	NIM,
	Nama,
	Jurusan
FROM Mahasiswa;
GO

-- Pembuatan Tabel Backup Otomatis
DROP TABLE IF EXISTS Mahasiswa_Backup;
GO

SELECT * INTO Mahasiswa_Backup FROM Mahasiswa;
GO

-- SP

-- 1. SP Insert
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

-- 2. SP Update
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

-- 3. SP Delete
CREATE PROCEDURE sp_DeleteMahasiswa
    @NIM VARCHAR(20) 
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Mahasiswa WHERE nim = @NIM;
END
GO

-- 4. SP Load Data
CREATE PROCEDURE sp_GetMahasiswa
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_mahasiswa, nim, nama, jurusan FROM Mahasiswa;
END
GO

-- 5. SP Hitung Total
CREATE PROCEDURE sp_CountMahasiswa
    @Total INT OUTPUT 
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @Total = COUNT(*) FROM Mahasiswa;
END
GO

USE SistemPresensiDB;
GO

ALTER PROCEDURE sp_DeleteMahasiswa
    @NIM VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @id INT;
    SELECT @id = id_mahasiswa FROM Mahasiswa WHERE nim = @NIM;

    -- Hapus dulu data presensi yang terkait
    DELETE FROM Presensi WHERE id_mahasiswa = @id;

    -- Hapus juga dari KRS jika ada
    DELETE FROM KRS WHERE id_mahasiswa = @id;

    -- Baru hapus mahasiswanya
    DELETE FROM Mahasiswa WHERE nim = @NIM;
END
GO

ALTER PROCEDURE sp_GetMahasiswa
AS
BEGIN
    SET NOCOUNT ON;
    SELECT nim, nama, jurusan FROM Mahasiswa;
END
