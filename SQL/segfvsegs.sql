
USE SistemPresensiDB;
GO

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
WHERE id_matakuliah = 1
  AND id_dosen      = 2
  AND TglPresensi BETWEEN '2024-01-01' AND '2024-06-30'
ORDER BY Tanggal;