using System;
using System.Data;
using System.Data.SqlClient;

namespace DropDatabase
{
    internal class DropDatabaseAndTables
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
                    Console.WriteLine("1. Drop Database PerusahaanAirMineral");
                    Console.WriteLine("2. Drop Tabel Produk");
                    Console.WriteLine("3. Drop Tabel Penjualan");
                    Console.WriteLine("4. Drop Tabel Pelanggan");
                    Console.WriteLine("5. Drop Tabel Gudang");
                    Console.WriteLine("6. Exit");
                    Console.WriteLine("\nEnter your Choice (1-6): ");
                    char ch = Convert.ToChar(Console.ReadLine());

                    switch (ch)
                    {
                        case '1':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksiSA;

                                string str = "USE master; DROP DATABASE PerusahaanAirMineral;";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Database berhasil dihapus.");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! " + ex.Message.ToString());
                                    Console.ReadLine();

                                }
                            }
                            break;
                        case '2':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksiSA;

                                string str = "USE PerusahaanAirMineral; DROP TABLE Produk;";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Tabel Produk berhasil dihapus.");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! " + ex.Message.ToString());
                                    Console.ReadLine();
                                }
                            }
                            break;
                        case '3':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksiSA;

                                string str = "USE PerusahaanAirMineral; DROP TABLE Penjualan;";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Tabel Penjualan berhasil dihapus.");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! " + ex.Message.ToString());
                                    Console.ReadLine();
                                }
                            }
                            break;
                        case '4':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksiSA;

                                string str = "USE PerusahaanAirMineral; DROP TABLE Pelanggan;";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Tabel Pelanggan berhasil dihapus.");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! " + ex.Message.ToString());
                                    Console.ReadLine();
                                }
                            }
                            break;
                        case '5':
                            {
                                SqlConnection koneksi = new SqlConnection();
                                koneksi.ConnectionString = strKoneksiSA;

                                string str = "USE PerusahaanAirMineral; DROP TABLE Gudang;";
                                SqlCommand cmd = new SqlCommand(str, koneksi);
                                try
                                {
                                    koneksi.Open();
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Tabel Gudang berhasil dihapus.");
                                    Console.ReadLine();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Terjadi Kesalahan! " + ex.Message.ToString());
                                    Console.ReadLine();
                                }
                            }
                            break;
                        case '6':
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
