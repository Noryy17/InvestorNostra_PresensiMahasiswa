-- Sintaks ini berfungsi untuk melihat secara utuh data apa saja 
-- yang sudah berhasil masuk ke tabel Presensi milikmu
SELECT * FROM Presensi;

-- PENJELASAN SYNTAX (Untuk Pemula):
-- INSERT INTO : Perintah untuk memasukkan data baru ke dalam sebuah tabel.
-- Presensi (...) : Nama tabel tujuan, diikuti nama kolom-kolom yang mau diisi di dalam kurung.
-- VALUES (...) : Nilai atau isi data yang akan dimasukkan, urutannya harus sama dengan nama kolom di atasnya.

INSERT INTO Presensi (tanggal, status, id_mahasiswa, id_matakuliah, id_dosen)
VALUES 
('2026-06-25', 'Hadir', 1, 1, 1),
('2026-06-25', 'Sakit', 2, 1, 1);