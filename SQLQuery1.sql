create database PerusahaanAirMineral
use PerusahaanAirMineral

CREATE TABLE Produk (
    Id_produk CHAR(6) PRIMARY KEY,
    Nama_produk VARCHAR(50),
    Deskripsi VARCHAR(255),
    Tgl_kadaluarsa DATE,
    Stok INT,
    Jumlah_tersedia INT
);

CREATE TABLE Penjualan (
    Id_penjualan CHAR(8) PRIMARY KEY,
	Tgl_penjualan DATE,
    Jumlah_terjual INT,
    Harga_jual MONEY,
	Metode_pembayaran VARCHAR (50),
	Id_produk CHAR(6),
    FOREIGN KEY (Id_produk) REFERENCES Produk(Id_produk),
);

CREATE TABLE Pelanggan (
    Id_pelanggan CHAR(8) PRIMARY KEY,
    Nama_pelanggan VARCHAR(50),
    Alamat VARCHAR(255),
    No_telepon CHAR(12),
    Email VARCHAR(50),
	Id_penjualan CHAR (8),
	FOREIGN KEY (Id_penjualan) REFERENCES Penjualan(Id_penjualan),
);

CREATE TABLE Gudang (
    Id_produk CHAR(6) PRIMARY KEY,
    Nama_produk VARCHAR(50),
    Jumlah INT,
    Tgl_masuk DATE,
    Tgl_kadaluarsa DATE
);

--input data Produk
INSERT INTO Produk (Id_produk, Nama_produk, Deskripsi, Tgl_kadaluarsa, Stok, Jumlah_tersedia) VALUES('p_0001','Aqua','Minuman', '2026-09-17',100, 100);
INSERT INTO Produk (Id_produk, Nama_produk, Deskripsi, Tgl_kadaluarsa, Stok, Jumlah_tersedia) VALUES('p_0002','Le Mineral','Minuman', '2026-12-25',50, 50);
INSERT INTO Produk (Id_produk, Nama_produk, Deskripsi, Tgl_kadaluarsa, Stok, Jumlah_tersedia) VALUES('p_0003','Cleo','Minuman', '2026-07-23',60, 60);
INSERT INTO Produk (Id_produk, Nama_produk, Deskripsi, Tgl_kadaluarsa, Stok, Jumlah_tersedia) VALUES('p_0004','Vit','Minuman', '2026-06-23',70, 70);

Select * from Produk

-- Sesuaikan
UPDATE Produk
SET
	Nama_produk = 'Danone', Deskripsi ='Minuman', Tgl_kadaluarsa = '2026-09-20', Stok = 100, Jumlah_tersedia = 100
WHERE 
	Id_produk ='p_0001';

-- Sesuaikan
DELETE FROM Produk WHERE Id_produk = 'p_0001';

-- Sesuaikan 
SELECT * FROM Produk WHERE Id_produk ='p_0001';
SELECT * FROM Produk WHERE Id_produk ='p_0002';

--input data Penjualan 
INSERT INTO Penjualan (Id_penjualan, Tgl_penjualan, Jumlah_terjual, Harga_jual, Metode_pembayaran, Id_produk)VALUES ('ip_00001', '2024-04-20', 20, 200000, 'Cash', 'p_0001');
INSERT INTO Penjualan (Id_penjualan, Tgl_penjualan, Jumlah_terjual, Harga_jual, Metode_pembayaran, Id_produk)VALUES ('ip_00002', '2024-04-28', 15, 180000, 'Credit Card', 'p_0002');
INSERT INTO Penjualan (Id_penjualan, Tgl_penjualan, Jumlah_terjual, Harga_jual, Metode_pembayaran, Id_produk)VALUES ('ip_00003', '2024-04-24', 10, 100000, 'Cash', 'p_0003');
INSERT INTO Penjualan (Id_penjualan, Tgl_penjualan, Jumlah_terjual, Harga_jual, Metode_pembayaran, Id_produk)VALUES ('ip_00004', '2024-04-23', 25, 250000, 'Cash', 'p_0004');

-- Sesuaikan
UPDATE Penjualan
SET
	Tgl_penjualan = '2024-04-25', Jumlah_terjual = 50, Harga_jual = 300000, Metode_pembayaran = 'Dana', Id_produk = 'p_0001'
WHERE 
	Id_penjualan ='ip_00001';

-- Sesuaikan
DELETE FROM Penjualan WHERE Id_penjualan = 'ip_00001';

-- Sesuaikan  
SELECT * FROM Penjualan WHERE id_penjualan ='ip_00001';

--input data Pelanggan 
INSERT INTO Pelanggan (Id_pelanggan, Nama_pelanggan, Alamat, No_telepon, Email, Id_penjualan)VALUES ('pl_00001', 'Isro Uman', 'BTN', '089866746352', 'isro@gmail.com', 'ip_00001');
INSERT INTO Pelanggan (Id_pelanggan, Nama_pelanggan, Alamat, No_telepon, Email, Id_penjualan)VALUES ('pl_00002', 'Dzar Zaki', 'Kasihan', '087765235647', 'dzar@gmail.com', 'ip_00002');
INSERT INTO Pelanggan (Id_pelanggan, Nama_pelanggan, Alamat, No_telepon, Email, Id_penjualan)VALUES ('pl_00003', 'Jamal Udin', 'Bandar Angin', '089876445634', 'jamal@gmail.com', 'ip_00003');
INSERT INTO Pelanggan (Id_pelanggan, Nama_pelanggan, Alamat, No_telepon, Email, Id_penjualan)VALUES ('pl_00004', 'Bruno Mars', 'Jiku Besar', '087765329087', 'bruno@gmail.com', 'ip_00004');

UPDATE Pelanggan
SET
	Nama_pelanggan = 'Isro Usman', Alamat ='Dermaga', No_telepon = '081240978767', Email = 'isro@gmail.com', Id_penjualan = 'ip_00001'
WHERE 
	Id_pelanggan ='pl_00001';

-- Sesuaikan
DELETE FROM Pelanggan WHERE Id_pelanggan = 'pl_00001';

-- Sesuaikan 
SELECT * FROM Pelanggan WHERE Id_pelanggan ='pl_00001';

--input data Gudang 
INSERT INTO Gudang (Id_produk, Nama_produk, Jumlah, Tgl_masuk, Tgl_kadaluarsa)VALUES ('p_0001', 'Aqua', 120, '2024-04-20', '2026-09-17');
INSERT INTO Gudang (Id_produk, Nama_produk, Jumlah, Tgl_masuk, Tgl_kadaluarsa)VALUES ('p_0002', 'Le Mineral', 100, '2024-04-20', '2026-12-25');
INSERT INTO Gudang (Id_produk, Nama_produk, Jumlah, Tgl_masuk, Tgl_kadaluarsa)VALUES ('p_0003', 'Cleo', 100, '2024-04-20', '2026-07-23');
INSERT INTO Gudang (Id_produk, Nama_produk, Jumlah, Tgl_masuk, Tgl_kadaluarsa)VALUES ('p_0004', 'Vit', 100, '2024-04-20', '2026-06-23');

-- Sesuaikan
DELETE FROM Gudang WHERE Id_produk= 'p_0001';

-- Sesuaikan 
SELECT * FROM Gudang WHERE Id_produk ='p_0001';

select * from Produk
select * from Penjualan
select * from Pelanggan
select * from Gudang

CREATE TRIGGER JumlahProdukBerkurangDariGudang
ON Produk
AFTER INSERT
AS
BEGIN
    DECLARE @id_produk CHAR(6);
    DECLARE @stok INT;

    SELECT @id_produk = Id_produk, @stok = Jumlah_tersedia FROM inserted;

    UPDATE Gudang
    SET Jumlah = Jumlah - @stok
    WHERE Id_produk = @id_produk;
END;


CREATE TRIGGER KurangiStokSetelahPenjualan
ON Penjualan
AFTER INSERT
AS
BEGIN
    DECLARE @IdProduk CHAR(6)
    DECLARE @JumlahTerjual INT

    SELECT @IdProduk = inserted.Id_produk, @JumlahTerjual = inserted.Jumlah_terjual
    FROM inserted

    UPDATE Produk
    SET Stok = Stok - @JumlahTerjual
    WHERE Id_produk = @IdProduk
END

CREATE TRIGGER KurangiJumlahTersediaSetelahPenjualan
ON Penjualan
AFTER INSERT
AS
BEGIN
    DECLARE @IdProduk CHAR(6)
    DECLARE @JumlahTerjual INT

    SELECT @IdProduk = inserted.Id_produk, @JumlahTerjual = inserted.Jumlah_terjual
    FROM inserted

    UPDATE Produk
    SET Jumlah_tersedia = Jumlah_tersedia - @JumlahTerjual
    WHERE Id_produk = @IdProduk
END

CREATE PROCEDURE GetPenjualanDetail
    @IdPenjualan CHAR(8)
AS
BEGIN
    SELECT 
        Penjualan.Id_penjualan,
        Penjualan.Tgl_penjualan,
        Penjualan.Jumlah_terjual,
        Penjualan.Harga_jual,
        Penjualan.Metode_pembayaran,
        Produk.Id_produk,
        Produk.Nama_produk,
        Produk.Deskripsi,
        Produk.Tgl_kadaluarsa,
        Pelanggan.ID_pelanggan,
        Pelanggan.Nama_pelanggan,
        Pelanggan.Alamat,
        Pelanggan.No_telepon,
        Pelanggan.Email
    FROM 
        Penjualan
    INNER JOIN 
        Produk ON Penjualan.Id_produk = Produk.Id_produk
    INNER JOIN 
        Pelanggan ON Penjualan.Id_penjualan = Pelanggan.Id_penjualan
    WHERE 
        Penjualan.Id_penjualan = @IdPenjualan;
END

EXEC GetPenjualanDetail @IdPenjualan = 'ip_00001';
EXEC GetPenjualanDetail @IdPenjualan = 'ip_00002';
EXEC GetPenjualanDetail @IdPenjualan = 'ip_00003';
EXEC GetPenjualanDetail @IdPenjualan = 'ip_00004';

CREATE PROCEDURE TransferProdukDariGudangKeProduk
AS
BEGIN
    CREATE TABLE #JumlahProdukBerkurang (
        Id_produk CHAR(6),
        Jumlah INT
    );

    DECLARE @Id_produk CHAR(6);
    DECLARE @Jumlah INT;

    DECLARE gudang_cursor CURSOR FOR
    SELECT Id_produk, Jumlah FROM Gudang;
    
    OPEN gudang_cursor;
    FETCH NEXT FROM gudang_cursor INTO @Id_produk, @Jumlah;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN        
        INSERT INTO #JumlahProdukBerkurang (Id_produk, Jumlah)
        VALUES (@Id_produk, @Jumlah);
        
        UPDATE Produk
        SET Stok = Stok + @Jumlah,
            Jumlah_tersedia = Jumlah_tersedia + @Jumlah
        WHERE Id_produk = @Id_produk;
        
        FETCH NEXT FROM gudang_cursor INTO @Id_produk, @Jumlah;
    END
    
    CLOSE gudang_cursor;
    DEALLOCATE gudang_cursor;

    SELECT Id_produk, Jumlah
    FROM #JumlahProdukBerkurang;

    DROP TABLE #JumlahProdukBerkurang;
END;

EXEC TransferProdukDariGudangKeProduk;


