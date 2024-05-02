using System.Data;
using System;
using System.Data.SqlClient;

namespace CREATEPerusahaanAirMineral
{
    internal class CreateDatabaseAndTables
    {
        static void Main(string[] args)
        {
            string strKoneksi = "Data Source = LAPTOP-J4ORM2F0\\ISRO;" +
              "Initial Catalog = RawatJalan;Integrated Security = True;";
            string strKoneksiSA = "Data Source = LAPTOP-J4ORM2F0\\ISRO;" +
                 "Initial Catalog = RawatJalan;User ID = sa; Password = iszak2003";
            while (true)
            {
                try
                {
                    Console.WriteLine("\nMenu");
                    Console.WriteLine("1. Koneksi Menggunakan Windows Authentication");
                    Console.WriteLine("2. Koneksi Menggunakan SQL Server Authentication");
                    Console.WriteLine("3. Buat Database PerusahaanAirMineral");
                    Console.WriteLine("4. Buat Tabel Produk");
                    Console.WriteLine("5. Buat Tabel Penjualan");
                    Console.WriteLine("6. Buat Tabel Pelanggan");
                    Console.WriteLine("7. Buat Tabel Gudang");
                    Console.WriteLine("8. Exit");
                    Console.WriteLine("\nEnter your Choice (1-8): ");
                    char ch = Convert.ToChar(Console.ReadLine());

                    switch (ch)
                    {
                        case '1':
                            {
                                try
                                {

                                    SqlConnection koneksi = new SqlConnection();
                                    koneksi.ConnectionString = strKoneksi;
                                    koneksi.Open();
                                    if (koneksi.State == ConnectionState.Open)
                                    {
                                        koneksi.Close();
                                    }
                                    Console.WriteLine("Koneksi Berhasil");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Periksa Kembali Server Anda!\n" + ex.Message.ToString());
                                    Console.ReadLine();

                                }
                            }
                            break;
                        case '2':
                            {
                                try
                                {
                                    SqlConnection koneksi = new SqlConnection();
                                    koneksi.ConnectionString = strKoneksiSA;
                                    koneksi.Open();
                                    if (koneksi.State == ConnectionState.Open)
                                    {
                                        koneksi.Close();
                                    }
                                    Console.WriteLine("Koneksi Berhasil");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Periksa Kembali Server Anda!\n" + ex.Message.ToString());
                                    Console.ReadLine();

                                }
                            }
                            break;
                        case '3':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksi;

                                string str = "CREATE DATABASE PerusahaanAirMineral ON PRIMARY " +
                                    "(NAME = PacarSaya_Data, " +
                                    "FILENAME = 'C:\\Kuliah\\Semester 4\\Semester 4(1)\\Pengembangan Aplikasi Basis Data Praktikum\\PerusahaanAirMineral\\PerusahaanAirMineral\\DataPerusahaan.mdf', SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
                                    "LOG ON (NAME = PacarSaya_Log," +
                                    "FILENAME = 'C:\\Kuliah\\Semester 4\\Semester 4(1)\\Pengembangan Aplikasi Basis Data Praktikum\\PerusahaanAirMineral\\PerusahaanAirMineral\\LogPerusahaan.ldf', SIZE = 1MB,MAXSIZE = 5 MB,FILEGROWTH = 10%)";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Database Berhasil diBuat");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! Cek Ulang Server Anda!\n" + ex.Message.ToString());
                                    Console.ReadLine();

                                }
                            }
                            break;
                        case '4':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksiSA;

                                string str = "USE PerusahaanAirMineral;" +
                                    "CREATE TABLE Produk(" +
                                    "Id_produk CHAR(6) PRIMARY KEY ," +
                                    "Nama_produk VARCHAR(50)," +
                                    "Deskripsi VARCHAR(255)," +
                                    "Tgl_kadaluarsa DATE," +
                                    "Stok INT," +
                                    "Jumlah_tersedia INT," +
                                    ")";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Tabel Berhasil Dibuat");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! Cek Ulang Server Anda!\n" + ex.Message.ToString());
                                    Console.ReadLine();

                                }
                            }
                            break;
                        case '5':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksiSA;

                                string str = "USE PerusahaanAirMineral;" +
                                    "CREATE TABLE Penjualan(" +
                                    "Id_penjualan CHAR(8) PRIMARY KEY ," +
                                    "Tgl_penjualan DATE," +
                                    "Jumlah_terjual INT, " +
                                    "Harga_jual MONEY," +
                                    "Metode_pembayaran VARCHAR(50)," +
                                    "Id_produk CHAR(6)," +
                                    "CONSTRAINT FK_Produk FOREIGN KEY (Id_produk) REFERENCES Produk(Id_produk)," +
                                    ")";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Tabel Berhasil Dibuat");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! Cek Ulang Server Anda!\n" + ex.Message.ToString());
                                    Console.ReadLine();

                                }
                            }
                            break;
                        case '6':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksiSA;

                                string str = "USE PerusahaanAirMineral;" +
                                    "CREATE TABLE Pelanggan(" +
                                    "Id_pelanggan CHAR(8) PRIMARY KEY ," +
                                    "Nama_pelanggan VARCHAR(50)," +
                                    "Alamat VARCHAR(255)," +
                                    "No_telepon CHAR(12)," +
                                    "Email VARCHAR(50)," +
                                    "Id_penjualan CHAR(8), " +
                                    "CONSTRAINT FK_Penjualan FOREIGN KEY (Id_penjualan) REFERENCES Penjualan(Id_penjualan)" +
                                    ")";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Tabel Berhasil Dibuat");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! Cek Ulang Server Anda!\n" + ex.Message.ToString());
                                    Console.ReadLine();

                                }
                            }
                            break;
                        case '7':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksiSA;

                                string str = "USE PerusahaanAirMineral;" +
                                    "CREATE TABLE Gudang(" +
                                    "Id_produk CHAR(8) PRIMARY KEY ," +
                                    "Nama_produk VARCHAR(50)," +
                                    "Jumlah INT," +
                                    "Tgl_masuk DATE," +
                                    "Tgl_kadaluarsa DATE," +
                                    ")";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Tabel Berhasil Dibuat");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! Cek Ulang Server Anda!\n" + ex.Message.ToString());
                                    Console.ReadLine();

                                }
                            }
                            break;
                        case '8':
                            return;
                        default:
                            {
                                Console.WriteLine("\nOpsi tidak valid");
                                break;
                            }


                    }
                    Console.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nPeriksa angka yang dimasukkan.\n" + e.Message.ToString());
                }
            }
        }
    }
}
