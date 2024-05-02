using System;
using System.Data;
using System.Data.SqlClient;

public class CreateStoreProcedureAndTrigger
{
    static void Main()
    {
        string connectionString = "Data Source = LAPTOP-J4ORM2F0\\ISRO;" +
            "Initial Catalog = PerusahaanAirMineral; " +
            "User ID = sa; Password = iszak2003";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                bool exitRequested = false;

                while (!exitRequested)
                {
                    Console.WriteLine("Pilih opsi:");
                    Console.WriteLine("1. Buat Stored Procedure");
                    Console.WriteLine("2. Buat Trigger");
                    Console.WriteLine("3. Keluar");
                    Console.WriteLine("\nEnter your Choice (1-3): ");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            Console.Clear();
                            CreateStoredProcedures(connection);
                            break;
                        case "2":
                            Console.Clear();
                            CreateTriggers(connection);
                            break;
                        case "3":
                            exitRequested = true;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Pilihan tidak valid.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    static void CreateStoredProcedures(SqlConnection connection)
    {
        try
        {
            string createProcedurePenjualan = @"
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
                        Penjualan.Id_penjualan = @IdPenjualan
                END";

            string createProcedureProduk = @"
                CREATE PROCEDURE TransferProdukDariGudangKeProduk
                AS
                BEGIN
                    DECLARE @Id_produk CHAR(6)
                    DECLARE @Jumlah INT

                    DECLARE gudang_cursor CURSOR FOR
                    SELECT Id_produk, Jumlah FROM Gudang

                    OPEN gudang_cursor
                    FETCH NEXT FROM gudang_cursor INTO @Id_produk, @Jumlah

                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        PRINT 'Jumlah produk yang berkurang di gudang untuk produk dengan Id_produk ' + @Id_produk + ': ' + CAST(@Jumlah AS VARCHAR(10))

                        UPDATE Produk
                        SET Stok = Stok + @Jumlah,
                            Jumlah_tersedia = Jumlah_tersedia + @Jumlah
                        WHERE Id_produk = @Id_produk

                        FETCH NEXT FROM gudang_cursor INTO @Id_produk, @Jumlah
                    END

                    CLOSE gudang_cursor
                    DEALLOCATE gudang_cursor
                END";

            using (SqlCommand commandPenjualan = new SqlCommand(createProcedurePenjualan, connection))
            using (SqlCommand commandProduk = new SqlCommand(createProcedureProduk, connection))
            {
                commandPenjualan.CommandType = CommandType.Text;
                commandPenjualan.ExecuteNonQuery();
                commandProduk.CommandType = CommandType.Text;
                commandProduk.ExecuteNonQuery();
                Console.WriteLine("Stored procedure berhasil dibuat.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static void CreateTriggers(SqlConnection connection)
    {
        try
        {
            string triggerKurangiStok = @"
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
                END";

            string triggerKurangiJumlahTersedia = @"
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
                END";

            string triggerJumlahProdukBerkurang = @"
                CREATE TRIGGER JumlahProdukBerkurangDariGudang
                ON Produk
                AFTER INSERT
                AS
                BEGIN
                    DECLARE @id_produk CHAR(6)
                    DECLARE @stok INT

                    SELECT @id_produk = Id_produk, @stok = Jumlah_tersedia FROM inserted

                    UPDATE Gudang
                    SET Jumlah = Jumlah - @stok
                    WHERE Id_produk = @id_produk
                END";

            using (SqlCommand commandKurangiStok = new SqlCommand(triggerKurangiStok, connection))
            using (SqlCommand commandKurangiJumlahTersedia = new SqlCommand(triggerKurangiJumlahTersedia, connection))
            using (SqlCommand commandJumlahProdukBerkurang = new SqlCommand(triggerJumlahProdukBerkurang, connection))
            {
                commandKurangiStok.ExecuteNonQuery();
                commandKurangiJumlahTersedia.ExecuteNonQuery();
                commandJumlahProdukBerkurang.ExecuteNonQuery();
                Console.WriteLine("Trigger berhasil dibuat.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
