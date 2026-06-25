-- Tabel Log Error
CREATE TABLE LogError (
    id_log INT IDENTITY(1,1) PRIMARY KEY,
    waktu DATETIME,
    pesan_error VARCHAR(MAX)
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
ALTER TRIGGER trg_PreventMassUpdate
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