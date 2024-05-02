using System;
using System.Data;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=LAPTOP-J4ORM2F0\\ISRO;" +
            "Initial Catalog=PerusahaanAirMineral; " +
            "User ID=sa;Password=iszak2003";

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Menu Stored Procedure:");
            Console.WriteLine("----------------------");
            Console.WriteLine("1. Detail Penjualan");
            Console.WriteLine("2. Produk tersisa dari Gudang");
            Console.WriteLine("3. Keluar");

            Console.WriteLine("\nEnter your Choice (1-3): ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    Console.Write("Masukkan ID Penjualan (Format: ip_00000): ");
                    string idPenjualan = Console.ReadLine();
                    if (idPenjualan.ToLower() == "exit")
                        break;
                    GetPenjualanDetail(connectionString, idPenjualan);
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Produk tersisa dari Gudang");
                    Console.WriteLine("--------------------------");
                    TransferProdukDariGudangKeProduk(connectionString);
                    break;
                case "3":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Pilihan tidak valid. Silakan pilih opsi yang benar.");
                    break;
            }
            Console.ReadKey();
        }
    }

    static void GetPenjualanDetail(string connectionString, string idPenjualan)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("GetPenjualanDetail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdPenjualan", idPenjualan);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("\nDetail Penjualan");
                            Console.WriteLine("----------------");
                            Console.WriteLine($"ID Penjualan        : {reader["Id_penjualan"]}");
                            Console.WriteLine($"Tanggal Penjualan   : {reader["Tgl_penjualan"]}");
                            Console.WriteLine($"Jumlah Terjual      : {reader["Jumlah_terjual"]}");
                            Console.WriteLine($"Harga Jual          : {reader["Harga_jual"]}");
                            Console.WriteLine($"Metode Pembayaran   : {reader["Metode_pembayaran"]}");
                            Console.WriteLine($"ID Produk           : {reader["Id_produk"]}");
                            Console.WriteLine($"Nama Produk         : {reader["Nama_produk"]}");
                            Console.WriteLine($"Deskripsi Produk    : {reader["Deskripsi"]}");
                            Console.WriteLine($"Tanggal Kadaluarsa  : {reader["Tgl_kadaluarsa"]}");
                            Console.WriteLine($"ID Pelanggan        : {reader["ID_pelanggan"]}");
                            Console.WriteLine($"Nama Pelanggan      : {reader["Nama_pelanggan"]}");
                            Console.WriteLine($"Alamat Pelanggan    : {reader["Alamat"]}");
                            Console.WriteLine($"No Telepon Pelanggan: {reader["No_telepon"]}");
                            Console.WriteLine($"Email Pelanggan     : {reader["Email"]}");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Data tidak ditemukan.");
                    }
                }
            }
        }
    }

    static void TransferProdukDariGudangKeProduk(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("TransferProdukDariGudangKeProduk", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Jumlah Produk Yang tersisa dari Gudang");
                    Console.WriteLine();

                    while (reader.Read())
                    {
                        string idProduk = reader["Id_produk"].ToString();
                        int jumlahBerkurang = Convert.ToInt32(reader["Jumlah"]);

                        Console.WriteLine($"{idProduk} : {jumlahBerkurang}");
                    }
                }
            }
        }
    }
}
