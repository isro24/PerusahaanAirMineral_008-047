using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PerusahaanAirMineral
{
    internal class MenuPerusahaanAirMineral
    {
        static void Main(string[] args)
        {
            MenuPerusahaanAirMineral pr = new MenuPerusahaanAirMineral();
            while (true)
            {
                try
                {
                    {
                        Console.Clear();
                        SqlConnection conn = null;
                        string strKoneksi = "Data Source = LAPTOP-J4ORM2F0\\ISRO;" +
                            "Initial Catalog = PerusahaanAirMineral; " +
                            "User ID = sa; Password = iszak2003";
                        conn = new SqlConnection(string.Format(strKoneksi));
                        conn.Open();
                        Console.Clear();
                        while (true)
                        {
                            try
                            {
                                Console.WriteLine("\nHalaman Aplikasi");
                                Console.WriteLine("---------------------");
                                Console.WriteLine("1. Menu Produk");
                                Console.WriteLine("2. Menu Penjualan");
                                Console.WriteLine("3. Menu Pelanggan");
                                Console.WriteLine("4. Menu Gudang");
                                Console.WriteLine("5. Keluar");
                                Console.WriteLine("\nEnter your choice (1-5): ");
                                char ch = Convert.ToChar(Console.ReadLine());
                                switch (ch)
                                {
                                    case '1':
                                        {
                                            Console.Clear();
                                            Console.WriteLine();
                                            pr.menuProduk(conn);
                                        }
                                        break;
                                    case '2':
                                        {
                                            Console.Clear();
                                            Console.WriteLine();
                                            pr.menuPenjualan(conn);
                                        }
                                        break;
                                    case '3':
                                        {
                                            Console.Clear();
                                            Console.WriteLine();
                                            pr.menuPelanggan(conn);
                                        }
                                        break;
                                    case '4':
                                        {
                                            Console.Clear();
                                            Console.WriteLine();
                                            pr.menuGudang(conn);
                                        }
                                        break;
                                    case '5':
                                        conn.Close();
                                        Console.Clear();
                                        return;
                                    default:
                                        {
                                            Console.Clear();
                                            Console.WriteLine("\nInvalid option");
                                        }
                                        break;

                                }
                            }
                            catch
                            {
                                Console.Clear();
                                Console.WriteLine("\nCheck for the value entered");
                            }
                        }
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Tidak Dapat Mengakses Database Tersebut\n");
                    Console.ResetColor();
                }
            }
        }

        //Menu Produk
        public void menuProduk(SqlConnection conn)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nMenu Produk");
                    Console.WriteLine("-------------------");
                    Console.WriteLine("1. Tambah Produk");
                    Console.WriteLine("2. Edit Produk");
                    Console.WriteLine("3. Hapus Produk");
                    Console.WriteLine("4. Cari Produk");
                    Console.WriteLine("5. Seluruh Data Produk");
                    Console.WriteLine("6. Kembali");
                    Console.WriteLine("\nEnter your choice (1-6): ");
                    char ch = Convert.ToChar(Console.ReadLine());
                    switch (ch)
                    {
                        case '1':
                            {
                                Console.Clear();
                                Console.WriteLine("Tambah Produk\n");
                                bool inputValid = false;
                                while (!inputValid)
                                {
                                    try
                                    {
                                        Console.WriteLine("Masukkan ID Produk (Format: p_0000):");
                                        string idProduk = Console.ReadLine()?.Trim();
                                        if (idProduk?.ToLower() == "exit")
                                        {
                                            return;
                                        }

                                        if (string.IsNullOrEmpty(idProduk) || !idProduk.StartsWith("p_") || !idProduk.Substring(2).All(char.IsDigit) || idProduk.Length != 6)
                                        {
                                            throw new FormatException("Format ID Produk tidak valid. Harap masukkan sesuai format: p_0000");
                                        }

                                        if (isProductExists(idProduk, conn))
                                        {
                                            Console.WriteLine($"ID produk {idProduk} sudah ada. Masukkan ID produk yang berbeda.");
                                            continue; 
                                        }

                                        Console.WriteLine("Masukkan Nama Produk:");
                                        string namaProduk = Console.ReadLine()?.Trim();
                                        while (string.IsNullOrEmpty(namaProduk))
                                        {
                                            Console.WriteLine("Nama Produk tidak boleh kosong. Harap masukkan nama produk:");
                                            namaProduk = Console.ReadLine()?.Trim();
                                        }

                                        Console.WriteLine("Masukkan Deskripsi Produk:");
                                        string deskripsi = Console.ReadLine()?.Trim();
                                        while (string.IsNullOrWhiteSpace(deskripsi))
                                        {
                                            Console.WriteLine("Deskripsi Produk tidak boleh kosong. Harap masukkan deskripsi produk:");
                                            deskripsi = Console.ReadLine()?.Trim();
                                        }
                                        Console.WriteLine("Masukkan Tanggal Kadaluarsa (YYYY-MM-DD):");
                                        DateTime tglKadaluarsa;
                                        string inputTanggal = Console.ReadLine()?.Trim();
                                        while (!DateTime.TryParseExact(inputTanggal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tglKadaluarsa) || tglKadaluarsa < DateTime.Today)
                                        {
                                            if (string.IsNullOrWhiteSpace(inputTanggal))
                                            {
                                                Console.WriteLine("Tanggal kadaluarsa tidak valid. Harap masukkan tanggal dalam format YYYY-MM-DD:");
                                            }
                                            else if (tglKadaluarsa < DateTime.Today)
                                            {
                                                Console.WriteLine("Tanggal kadaluarsa tidak valid. Harap masukkan tanggal setelah hari ini (YYYY-MM-DD):");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Format tanggal tidak valid. Harap masukkan tanggal dalam format YYYY-MM-DD:");
                                            }
                                            inputTanggal = Console.ReadLine()?.Trim();
                                        }


                                        Console.WriteLine("Masukkan Stok Produk:");
                                        int stok;
                                        while (!int.TryParse(Console.ReadLine()?.Trim(), out stok) || stok < 1)
                                        {
                                            if (stok < 1)
                                            {
                                                Console.WriteLine("Stok tidak valid. Harap masukkan angka tidak kurang dari 1:");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Stok tidak valid. Harap masukkan angka:");
                                            }
                                        }

                                        Console.WriteLine("Masukkan Jumlah Tersedia:");
                                        int jumlahTersedia;
                                        while (!int.TryParse(Console.ReadLine()?.Trim(), out jumlahTersedia) || jumlahTersedia < 1)
                                        {
                                            if (jumlahTersedia < 1)
                                            {
                                                Console.WriteLine("Jumlah Tersedia tidak valid. Harap masukkan angka tidak kurang dari 1:");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Jumlah Tersedia tidak valid. Harap masukkan angka:");
                                            }
                                        }
                                        insert(idProduk, namaProduk, deskripsi, tglKadaluarsa, stok, jumlahTersedia, conn);
                                        inputValid = true;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
                                    }
                                    catch (SqlException ex) when (ex.Number == 2627)
                                    {
                                        Console.WriteLine($"ID produk  sudah ada. Masukkan ID produk yang berbeda.");
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nGagal menambahkan data produk.");
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                            }
                            break;

                        case '2':
                            {
                                EditMenu:
                                Console.Clear();
                                Console.WriteLine("Edit Produk\n");
                                string Id;

                                while (true)
                                {
                                    Console.WriteLine("Masukkan Id Produk yang ingin diUbah (ketik 'exit' untuk kembali):");
                                    Id = Console.ReadLine();
                                    if (Id.ToLower() == "exit")
                                    {
                                        return;
                                    }

                                    if (isProductExists(Id, conn))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nId_produk tidak ditemukan. Silakan masukkan Id_produk yang sesuai.\n");
                                    }
                                }

                                bool inputValid = false;
                                while (!inputValid)
                                {
                                    try
                                    {
                                        Console.WriteLine("\nPilih data yang ingin diubah:\n");
                                        Console.WriteLine("1. Nama Produk");
                                        Console.WriteLine("2. Deskripsi Produk");
                                        Console.WriteLine("3. Tanggal Kadaluarsa");
                                        Console.WriteLine("4. Stok");
                                        Console.WriteLine("5. Jumlah Tersedia");
                                        Console.WriteLine("6. Selesai");

                                        Console.Write("\nPilihan Anda: ");
                                        int choice;
                                        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 6)
                                        {
                                            Console.WriteLine("Masukkan angka dari 1 hingga 6 sesuai dengan pilihan yang tersedia.");
                                            Console.Write("Pilihan Anda: ");
                                        }

                                        string newNama;
                                        string newDeskripsi;
                                        int newStok;
                                        int newJumlah;

                                        switch (choice)
                                        {
                                            case 1:
                                                Console.Clear();
                                                Console.WriteLine("\nMasukkan Nama Produk Baru:");
                                                newNama = Console.ReadLine()?.Trim();
                                                while (string.IsNullOrEmpty(newNama))
                                                {
                                                    Console.WriteLine("Nama Produk tidak boleh kosong. Harap masukkan nama produk:");
                                                    newNama = Console.ReadLine()?.Trim();
                                                }
                                                update(Id, newNama, null, default(DateTime), 0, 0, conn);
                                                break;

                                            case 2:
                                                Console.Clear();
                                                Console.WriteLine("\nMasukkan Deskripsi Produk Baru:");
                                                newDeskripsi = Console.ReadLine()?.Trim();
                                                while (string.IsNullOrWhiteSpace(newDeskripsi))
                                                {
                                                    Console.WriteLine("Deskripsi Produk tidak boleh kosong. Harap masukkan deskripsi produk:");
                                                    newDeskripsi = Console.ReadLine()?.Trim();
                                                }
                                                update(Id, null, newDeskripsi, default(DateTime), 0, 0, conn);
                                                break;

                                            case 3:
                                                Console.Clear();
                                                Console.WriteLine("\nMasukkan Tanggal Kadaluarsa Baru (YYYY-MM-DD):");
                                                DateTime newExpirationDate;
                                                string inputTanggal = Console.ReadLine()?.Trim();
                                                while (!DateTime.TryParseExact(inputTanggal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out newExpirationDate) || newExpirationDate < DateTime.Today)
                                                {
                                                    if (string.IsNullOrWhiteSpace(inputTanggal))
                                                    {
                                                        Console.WriteLine("Tanggal kadaluarsa tidak valid. Harap masukkan tanggal dalam format YYYY-MM-DD:");
                                                    }
                                                    else if (newExpirationDate < DateTime.Today)
                                                    {
                                                        Console.WriteLine("Tanggal kadaluarsa tidak valid. Harap masukkan tanggal setelah hari ini (YYYY-MM-DD):");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Format tanggal tidak valid. Harap masukkan tanggal dalam format YYYY-MM-DD:");
                                                    }
                                                    inputTanggal = Console.ReadLine()?.Trim();
                                                }
                                                update(Id, null, null, newExpirationDate, 0, 0, conn);
                                                break;

                                            case 4:
                                                Console.Clear();
                                                Console.WriteLine("\nMasukkan Stok Baru:");
                                                while (!int.TryParse(Console.ReadLine()?.Trim(), out newStok) || newStok < 0)
                                                {
                                                    if (newStok < 0)
                                                    {
                                                        Console.WriteLine("Stok baru tidak valid. Harap masukkan angka tidak kurang dari 0:");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Stok baru tidak valid. Harap masukkan angka:");
                                                    }
                                                }
                                                update(Id, null, null, default(DateTime), newStok, 0, conn);
                                                break;

                                            case 5:
                                                Console.Clear();
                                                Console.WriteLine("\nMasukkan Jumlah Tersedia Baru:");
                                                while (!int.TryParse(Console.ReadLine()?.Trim(), out newJumlah) || newJumlah < 0)
                                                {
                                                    if (newJumlah < 0)
                                                    {
                                                        Console.WriteLine("Jumlah Tersedia baru tidak valid. Harap masukkan angka tidak kurang dari 0:");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Jumlah Tersedia baru tidak valid. Harap masukkan angka:");
                                                    }
                                                }
                                                update(Id, null, null, default(DateTime), 0, newJumlah, conn);
                                                break;

                                            case 6:
                                                Console.Clear();
                                                inputValid = true;
                                                goto EditMenu;

                                            default:
                                                Console.Clear();
                                                Console.WriteLine("Pilihan tidak valid.");
                                                break;
                                        }
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nGagal mengubah data produk.");
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                            }
                            break;

                        case '3':
                            {
                                Console.Clear();
                                Console.WriteLine("Hapus Produk\n");
                                string idProduk;

                                while (true)
                                {
                                    Console.WriteLine("Masukkan Id Produk yang ingin Dihapus:");
                                    idProduk = Console.ReadLine();
                                    if (idProduk.ToLower() == "exit")
                                    {
                                        return;
                                    }

                                    if (isProductExists(idProduk, conn))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nId_produk yang dimasukkan tidak sesuai.\n");
                                    }
                                }

                                try
                                {
                                    delete(idProduk, conn);
                                    Console.WriteLine("Data Berhasil Dihapus");
                                }
                                catch (Exception e)
                                {
                                    if (e is SqlException sqlEx && sqlEx.Number == 547)
                                    {
                                        Console.WriteLine("Tidak dapat menghapus produk karena masih ada data terkait.");
                                        Console.WriteLine("Silakan hapus terlebih dahulu data terkait.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nGagal menghapus data produk.");
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                            }
                            break;

                        case '4':
                            {
                                Console.Clear();
                                Console.WriteLine("Cari Produk\n");
                                string idProduk;

                                while (true)
                                {
                                    Console.WriteLine("Masukkan ID Produk yang ingin dicari:");
                                    idProduk = Console.ReadLine();
                                    if (idProduk.ToLower() == "exit")
                                    {
                                        return;
                                    }

                                    if (isProductExists(idProduk, conn))
                                    {
                                        break; 
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nMasukkan Id_produk yang sesuai.\n");
                                    }
                                }  

                                Console.WriteLine("\nMenampilkan Produk:");
                                Console.WriteLine("------------------");
                                search(idProduk, conn);
                            }
                            break;

                        case '5':
                            {
                                Console.Clear();
                                Console.WriteLine("Seluruh Data Produk");
                                Console.WriteLine("-------------------");
                                Console.WriteLine();
                                baca(conn);
                            }
                            break;

                        case '6':
                            return;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("\nInvalid option");
                            }
                            break;

                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("\nCheck for the value entered");
                }
            }
        }
        public void baca(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT Id_produk, Nama_produk, Deskripsi, Tgl_Kadaluarsa, Stok, Jumlah_tersedia FROM Produk", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine("ID Produk         : " + r["Id_produk"]);
                Console.WriteLine("Nama Produk       : " + r["Nama_produk"]);
                Console.WriteLine("Deskripsi         : " + r["Deskripsi"]);
                Console.WriteLine("Tanggal Kadaluarsa: " + r["Tgl_Kadaluarsa"]);
                Console.WriteLine("Stok              : " + r["Stok"]);
                Console.WriteLine("Jumlah Tersedia   : " + r["Jumlah_tersedia"]);
                Console.WriteLine();
            }
            r.Close();
        }

        public void insert(string idProduk, string namaProduk, string deskripsi, DateTime tglKadaluarsa, int stok, int jumlahTersedia, SqlConnection con)
        {
            string str = "insert into Produk (Id_produk, Nama_produk, Deskripsi, Tgl_Kadaluarsa, Stok, Jumlah_tersedia) " +
                "values(@idProduk, @nama, @deskripsi, @tglKadaluarsa, @stok, @jumlahTersedia)";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("idProduk", idProduk));
            cmd.Parameters.Add(new SqlParameter("nama", namaProduk));
            cmd.Parameters.Add(new SqlParameter("deskripsi", deskripsi));
            cmd.Parameters.Add(new SqlParameter("tglKadaluarsa", tglKadaluarsa));
            cmd.Parameters.Add(new SqlParameter("stok", stok));
            cmd.Parameters.Add(new SqlParameter("jumlahTersedia", jumlahTersedia));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }
        public void delete(string idProduk, SqlConnection con)
        {
            string str = "Delete Produk where Id_produk = @idProduk";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("idProduk", idProduk));
            cmd.ExecuteNonQuery();
        }
        public void search(string idProduk, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Produk WHERE Id_produk = @idProduk", con);
            cmd.Parameters.AddWithValue("@idProduk", idProduk);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.HasRows)
            {
                while (r.Read())
                {
                    Console.WriteLine("ID Produk         : " + r["Id_produk"]);
                    Console.WriteLine("Nama Produk       : " + r["Nama_produk"]);
                    Console.WriteLine("Deskripsi         : " + r["Deskripsi"]);
                    Console.WriteLine("Tanggal Kadaluarsa: " + r["Tgl_Kadaluarsa"]);
                    Console.WriteLine("Stok              : " + r["Stok"]);
                    Console.WriteLine("Jumlah Tersedia   : " + r["Jumlah_tersedia"]);
                }
            }
            else
            {
                Console.WriteLine("Data tidak ditemukan.");
            }
            r.Close();
        }


        public void update(string Id, string newNama, string newDeskripsi, DateTime newTgl, int newStok, int newJumlah, SqlConnection con)
        {
            string str = "UPDATE Produk SET ";

            if (!string.IsNullOrEmpty(newNama))
            {
                str += "Nama_produk = @newNama, ";
            }
            if (!string.IsNullOrEmpty(newDeskripsi))
            {
                str += "Deskripsi = @newDeskripsi, ";
            }
            if (newTgl != default(DateTime))
            {
                str += "Tgl_Kadaluarsa = @newTgl, ";
            }
            if (newStok >= 0)
            {
                str += "Stok = @newStok, ";
            }
            if (newJumlah >= 0)
            {
                str += "Jumlah_tersedia = @newJumlah, ";
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE Id_produk = @Id";

            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            if (!string.IsNullOrEmpty(newNama))
            {
                cmd.Parameters.AddWithValue("@newNama", newNama);
            }
            if (!string.IsNullOrEmpty(newDeskripsi))
            {
                cmd.Parameters.AddWithValue("@newDeskripsi", newDeskripsi);
            }
            if (newTgl != default(DateTime))
            {
                cmd.Parameters.AddWithValue("@newTgl", newTgl);
            }
            if (newStok >= 0)
            {
                cmd.Parameters.AddWithValue("@newStok", newStok);
            }
            if (newJumlah >= 0)
            {
                cmd.Parameters.AddWithValue("@newJumlah", newJumlah);
            }

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Diubah");
        }

        public bool isProductExists(string idProduk, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Produk WHERE Id_produk = @idProduk", con);
            cmd.Parameters.AddWithValue("@idProduk", idProduk);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        //Menu Penjualan

        public void menuPenjualan(SqlConnection conn)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nMenu Penjualan");
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("1. Tambah Penjualan");
                    Console.WriteLine("2. Edit Penjualan");
                    Console.WriteLine("3. Hapus Penjualan");
                    Console.WriteLine("4. Detail Penjualan");
                    Console.WriteLine("5. Seluruh Data Penjualan");
                    Console.WriteLine("6. Kembali");
                    Console.WriteLine("\nMasukkan pilihan Anda (1-5): ");
                    char ch = Convert.ToChar(Console.ReadLine());
                    switch (ch)
                    {
                        case '1':
                            {
                                Console.Clear();
                                Console.WriteLine("Tambah Penjualan\n");
                                bool inputValid = false;
                                while (!inputValid)
                                {
                                    try
                                    {
                                        Console.WriteLine("Masukkan ID Penjualan (Format: ip_00000):");
                                        string idPenjualan = Console.ReadLine()?.Trim();
                                        if (idPenjualan?.ToLower() == "exit")
                                        {
                                            return;
                                        }

                                        if (string.IsNullOrEmpty(idPenjualan) || !idPenjualan.StartsWith("ip_") || !idPenjualan.Substring(3).All(char.IsDigit) || idPenjualan.Length != 8)
                                        {
                                            throw new FormatException("Format ID Penjualan tidak valid. Harap masukkan sesuai format: ip_00000");
                                        }

                                        if (isPenjualanExists(idPenjualan, conn))
                                        {
                                            Console.WriteLine($"ID penjualan {idPenjualan} sudah ada. Masukkan ID penjualan yang berbeda.");
                                            continue;
                                        }

                                        Console.WriteLine("Masukkan Tanggal Penjualan (YYYY-MM-DD):");
                                        DateTime tanggalPenjualan;
                                        string inputTanggalPenjualan = Console.ReadLine()?.Trim();
                                        while (!DateTime.TryParseExact(inputTanggalPenjualan, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tanggalPenjualan) || tanggalPenjualan > DateTime.Today)
                                        {
                                            if (string.IsNullOrWhiteSpace(inputTanggalPenjualan))
                                            {
                                                Console.WriteLine("Tanggal penjualan tidak valid. Harap masukkan tanggal dalam format YYYY-MM-DD:");
                                            }
                                            else if (tanggalPenjualan > DateTime.Today)
                                            {
                                                Console.WriteLine("Tanggal penjualan tidak valid. Harap masukkan tanggal hari ini atau sebelum hari ini (YYYY-MM-DD):");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Format tanggal tidak valid. Harap masukkan tanggal dalam format YYYY-MM-DD:");
                                            }
                                            inputTanggalPenjualan = Console.ReadLine()?.Trim();
                                        }


                                        Console.WriteLine("Masukkan Jumlah Terjual:");
                                        int jumlahTerjual;
                                        while (!int.TryParse(Console.ReadLine()?.Trim(), out jumlahTerjual) || jumlahTerjual < 1)
                                        {
                                            if (jumlahTerjual < 1)
                                            {
                                                Console.WriteLine("Jumlah Terjual tidak valid. Harap masukkan angka tidak kurang dari 1:");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Jumlah Terjual tidak valid. Harap masukkan angka:");
                                            }
                                        }

                                        Console.WriteLine("Masukkan Harga Jual:");
                                        decimal hargaJual;
                                        while (!decimal.TryParse(Console.ReadLine()?.Trim(), out hargaJual) || hargaJual < 1)
                                        {
                                            if (hargaJual < 1)
                                            {
                                                Console.WriteLine("Harga Jual tidak valid. Harap masukkan angka tidak kurang dari 1:");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Harga Jual tidak valid. Harap masukkan angka:");
                                            }
                                        }

                                        Console.WriteLine("Masukkan Metode Pembayaran:");
                                        string metodePembayaran = Console.ReadLine()?.Trim();
                                        while (string.IsNullOrEmpty(metodePembayaran))
                                        {
                                            Console.WriteLine("Metode Pembayaran tidak boleh kosong. Harap masukkan metode pembayaran:");
                                            metodePembayaran = Console.ReadLine()?.Trim();
                                        }

                                        string idProduk;
                                        bool isIdProdukValid = false;
                                        do
                                        {
                                            Console.WriteLine("Masukkan ID Produk (Format: p_0000):");
                                            idProduk = Console.ReadLine()?.Trim();

                                            if (!idProduk.StartsWith("p_") || !idProduk.Substring(2).All(char.IsDigit) || idProduk.Length != 6)
                                            {
                                                Console.WriteLine("Format ID Produk tidak valid. Harap masukkan sesuai format: p_0000");
                                            }
                                            else
                                            {
                                                if (isProdukExists(idProduk, conn))
                                                {
                                                    isIdProdukValid = true;
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"ID produk {idProduk} tidak ditemukan. Masukkan ID produk yang valid.");
                                                }
                                            }
                                        } while (!isIdProdukValid);

                                        insertPenjualan(idPenjualan, tanggalPenjualan, jumlahTerjual, hargaJual, metodePembayaran, idProduk, conn);
                                        inputValid = true;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
                                    }
                                    catch (SqlException ex) when (ex.Number == 2627)
                                    {
                                        Console.WriteLine($"ID penjualan sudah ada. Masukkan ID penjualan yang berbeda.");
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nGagal menambahkan data penjualan.");
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                            }
                            break;

                        case '2':
                            {
                                EditMenu:
                                Console.Clear();
                                Console.WriteLine("Edit Penjualan\n");
                                string Id;

                                while (true)
                                {
                                    Console.WriteLine("Masukkan Id Penjualan yang ingin diubah:");
                                    Id = Console.ReadLine();
                                    if (Id.ToLower() == "exit")
                                    {
                                        return;
                                    }

                                    if (isPenjualanExists(Id, conn))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nId Penjualan tidak ditemukan. Silakan masukkan Id Penjualan yang sesuai format: ip_00000.\n");
                                    }
                                }

                                bool inputValid = false;
                                while (!inputValid)
                                {
                                    try
                                    {
                                        Console.WriteLine("\nPilih data yang ingin diubah:\n");
                                        Console.WriteLine("1. Tanggal Penjualan");
                                        Console.WriteLine("2. Jumlah Terjual");
                                        Console.WriteLine("3. Harga Jual");
                                        Console.WriteLine("4. Metode Pembayaran");
                                        Console.WriteLine("5. ID Produk");
                                        Console.WriteLine("6. Selesai");

                                        Console.Write("\nPilihan Anda: ");
                                        int choice;
                                        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 6)
                                        {
                                            Console.WriteLine("Masukkan angka dari 1 hingga 6 sesuai dengan pilihan yang tersedia.");
                                            Console.Write("Pilihan Anda: ");
                                        }

                                        switch (choice)

                                        {
                                            case 1:
                                                Console.Clear();
                                                Console.WriteLine("\nMasukkan Tanggal Penjualan Baru (YYYY-MM-DD):");
                                                DateTime newTanggalPenjualan;
                                                string inputNewTanggalPenjualan = Console.ReadLine()?.Trim();
                                                while (!DateTime.TryParseExact(inputNewTanggalPenjualan, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out newTanggalPenjualan) || newTanggalPenjualan > DateTime.Today)
                                                {
                                                    if (string.IsNullOrWhiteSpace(inputNewTanggalPenjualan))
                                                    {
                                                        Console.WriteLine("Tanggal penjualan tidak valid. Harap masukkan tanggal dalam format YYYY-MM-DD:");
                                                    }
                                                    else if (newTanggalPenjualan > DateTime.Today)
                                                    {
                                                        Console.WriteLine("Tanggal penjualan tidak valid. Harap masukkan tanggal hari ini atau sebelum hari ini (YYYY-MM-DD):");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Format tanggal tidak valid. Harap masukkan tanggal dalam format YYYY-MM-DD:");
                                                    }
                                                    inputNewTanggalPenjualan = Console.ReadLine()?.Trim();
                                                }
                                                updatePenjualan(Id, newTanggalPenjualan, default, default, null, null, conn);
                                                break;

                                            case 2:
                                                Console.Clear();
                                                Console.WriteLine("\nMasukkan Jumlah Terjual Baru:");
                                                int newJumlahTerjual;
                                                while (!int.TryParse(Console.ReadLine()?.Trim(), out newJumlahTerjual) || newJumlahTerjual < 1)
                                                {
                                                    if (newJumlahTerjual < 1)
                                                    {
                                                        Console.WriteLine("Jumlah Terjual tidak valid. Harap masukkan angka tidak kurang dari 1:");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Jumlah Terjual tidak valid. Harap masukkan angka:");
                                                    }
                                                }
                                                updatePenjualan(Id, default, newJumlahTerjual, default, null, null, conn);
                                                break;

                                            case 3:
                                                Console.Clear();
                                                Console.WriteLine("\nMasukkan Harga Jual Baru:");
                                                decimal newHargaJual;
                                                while (!decimal.TryParse(Console.ReadLine()?.Trim(), out newHargaJual) || newHargaJual < 1)
                                                {
                                                    if (newHargaJual < 1)
                                                    {
                                                        Console.WriteLine("Harga Jual tidak valid. Harap masukkan angka tidak kurang dari 1:");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Harga Jual tidak valid. Harap masukkan angka:");
                                                    }
                                                }
                                                updatePenjualan(Id, default, default, newHargaJual, null, null, conn);
                                                break;

                                            case 4:
                                                Console.Clear();
                                                Console.WriteLine("\nMasukkan Metode Pembayaran Baru:");
                                                string newMetodePembayaran = Console.ReadLine()?.Trim();
                                                while (string.IsNullOrEmpty(newMetodePembayaran))
                                                {
                                                    Console.WriteLine("Metode Pembayaran tidak boleh kosong. Harap masukkan metode pembayaran:");
                                                    newMetodePembayaran = Console.ReadLine()?.Trim();
                                                }
                                                updatePenjualan(Id, default, default, default, newMetodePembayaran, null, conn);
                                                break;

                                            case 5:
                                                Console.Clear();
                                                string newIdProduk;
                                                do
                                                {
                                                    Console.WriteLine("\nMasukkan ID Produk Baru (Format: p_0000):");
                                                    newIdProduk = Console.ReadLine()?.Trim();
                                                    if (!newIdProduk.StartsWith("p_") || !newIdProduk.Substring(2).All(char.IsDigit) || newIdProduk.Length != 6)
                                                    {
                                                        Console.WriteLine("Format ID Produk tidak valid. Harap masukkan sesuai format: p_0000");
                                                    }
                                                } while (!newIdProduk.StartsWith("p_") || !newIdProduk.Substring(2).All(char.IsDigit) || newIdProduk.Length != 6);
                                                updatePenjualan(Id, default, default, default, null, newIdProduk, conn);
                                                break;

                                            case 6:
                                                Console.Clear();
                                                inputValid = true;
                                                goto EditMenu;
                                            default:
                                                Console.Clear();
                                                Console.WriteLine("Pilihan tidak valid.");
                                                break;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nGagal mengubah data penjualan.");
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                            }
                            break;
                        case '3':
                            {
                                Console.Clear();
                                Console.WriteLine("Hapus Penjualan\n");
                                string idPenjualan;

                                while (true)
                                {
                                    Console.WriteLine("Masukkan Id Penjualan yang ingin Dihapus:");
                                    idPenjualan = Console.ReadLine();
                                    if (idPenjualan.ToLower() == "exit")
                                    {
                                        return;
                                    }

                                    if (isPenjualanExists(idPenjualan, conn))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nMasukkan Id_penjualan yang sesuai.\n");
                                    }
                                }

                                try
                                {
                                    deletePenjualan(idPenjualan, conn);
                                    Console.WriteLine("Data Berhasil Dihapus");
                                }
                                catch (Exception e)
                                {
                                    if (e is SqlException sqlEx && sqlEx.Number == 547)
                                    {
                                        Console.WriteLine("Tidak dapat menghapus penjualan karena masih ada data terkait di tabel Pelanggan.");
                                        Console.WriteLine("Silakan hapus terlebih dahulu data terkait di tabel Pelanggan.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nGagal menghapus data penjualan.");
                                        Console.WriteLine(e.ToString());
                                    }
                                }

                            }
                            break;

                        case '4':
                            {
                                Console.Clear();
                                Console.WriteLine("Detail Penjualan\n");
                                string idPenjualan;
                                while (true)
                                {
                                    Console.WriteLine("Masukkan ID Penjualan yang ingin dilihat detailnya:");
                                    idPenjualan = Console.ReadLine();
                                    if (idPenjualan.ToLower() == "exit")
                                    {
                                        return;
                                    }
                                    if (isPenjualanExists(idPenjualan, conn))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nMasukkan Id_penjualan yang sesuai.\n");
                                    }
                                }
                                Console.WriteLine("\nMenampilkan Detail Penjualan:");
                                Console.WriteLine("----------------------------");
                                detailPenjualan(idPenjualan, conn);
                            }
                            break;

                        case '5':
                            {
                                Console.Clear();
                                Console.WriteLine("Seluruh Data Penjualan");
                                Console.WriteLine("---------------------");
                                Console.WriteLine();
                                bacaPenjualan(conn);
                            }
                            break;
                        case '6':
                            return;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("\nPilihan tidak valid");
                            }
                            break;
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("\nPeriksa nilai yang dimasukkan");
                }

            }
        }

        public void insertPenjualan(string idPenjualan, DateTime tanggalPenjualan, int jumlahTerjual, decimal hargaJual, string metodePembayaran, string idProduk, SqlConnection con)
        {
            string str = "INSERT INTO Penjualan (Id_penjualan, Tgl_penjualan, Jumlah_terjual, Harga_jual, Metode_pembayaran, Id_produk) " +
                        "VALUES (@idPenjualan, @tanggalPenjualan, @jumlahTerjual, @hargaJual, @metodePembayaran, @idProduk)";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("idPenjualan", idPenjualan));
            cmd.Parameters.Add(new SqlParameter("tanggalPenjualan", tanggalPenjualan));
            cmd.Parameters.Add(new SqlParameter("jumlahTerjual", jumlahTerjual));
            cmd.Parameters.Add(new SqlParameter("hargaJual", hargaJual));
            cmd.Parameters.Add(new SqlParameter("metodePembayaran", metodePembayaran));
            cmd.Parameters.Add(new SqlParameter("idProduk", idProduk));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void deletePenjualan(string idPenjualan, SqlConnection con)
        {
            string str = "DELETE FROM Penjualan WHERE Id_penjualan = @idPenjualan";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@idPenjualan", idPenjualan);
            cmd.ExecuteNonQuery();
        }


        public void detailPenjualan(string idPenjualan, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Penjualan WHERE Id_penjualan = @idPenjualan", con);
            cmd.Parameters.AddWithValue("@idPenjualan", idPenjualan);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("ID Penjualan     : " + reader["Id_penjualan"]);
                    Console.WriteLine("Tanggal Penjualan: " + reader["Tgl_penjualan"]);
                    Console.WriteLine("Jumlah Terjual   : " + reader["Jumlah_terjual"]);
                    Console.WriteLine("Harga Jual       : " + reader["Harga_jual"]);
                    Console.WriteLine("Metode Pembayaran: " + reader["Metode_pembayaran"]);
                    Console.WriteLine("ID Produk        : " + reader["Id_produk"]);
                }
            }
            else
            {
                Console.WriteLine("Data tidak ditemukan.");
            }
            reader.Close();
        }


        public void bacaPenjualan(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("Select Id_penjualan, Tgl_penjualan, Jumlah_terjual, Harga_jual, Metode_pembayaran, Id_produk From Penjualan ", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine("ID Penjualan     : " + r["Id_penjualan"]);
                Console.WriteLine("Tanggal Penjualan: " + r["Tgl_penjualan"]);
                Console.WriteLine("Jumlah Terjual   : " + r["Jumlah_terjual"]);
                Console.WriteLine("Harga Jual       : " + r["Harga_jual"]);
                Console.WriteLine("Metode Pembayaran: " + r["Metode_pembayaran"]);
                Console.WriteLine("ID Produk        : " + r["Id_produk"]);
                Console.WriteLine();
            }
            r.Close();
        }


        public void updatePenjualan(string Id, DateTime newTanggalPenjualan, int newJumlahTerjual, decimal newHargaJual, string newMetodePembayaran, string newIdProduk, SqlConnection con)
        {
            string str = "UPDATE Penjualan SET ";

            if (newTanggalPenjualan != default(DateTime))
            {
                str += "Tanggal_penjualan = @newTanggalPenjualan, ";
            }
            if (newJumlahTerjual >= 0)
            {
                str += "Jumlah_terjual = @newJumlahTerjual, ";
            }
            if (newHargaJual >= 0)
            {
                str += "Harga_jual = @newHargaJual, ";
            }
            if (!string.IsNullOrEmpty(newMetodePembayaran))
            {
                str += "Metode_pembayaran = @newMetodePembayaran, ";
            }
            if (!string.IsNullOrEmpty(newIdProduk))
            {
                str += "Id_produk = @newIdProduk, ";
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE Id_penjualan = @Id";

            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            if (newTanggalPenjualan != default(DateTime))
            {
                cmd.Parameters.AddWithValue("@newTanggalPenjualan", newTanggalPenjualan);
            }
            if (newJumlahTerjual >= 0)
            {
                cmd.Parameters.AddWithValue("@newJumlahTerjual", newJumlahTerjual);
            }
            if (newHargaJual >= 0)
            {
                cmd.Parameters.AddWithValue("@newHargaJual", newHargaJual);
            }
            if (!string.IsNullOrEmpty(newMetodePembayaran))
            {
                cmd.Parameters.AddWithValue("@newMetodePembayaran", newMetodePembayaran);
            }
            if (!string.IsNullOrEmpty(newIdProduk))
            {
                cmd.Parameters.AddWithValue("@newIdProduk", newIdProduk);
            }

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Diubah");
        }


        public bool isPenjualanExists(string idPenjualan, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Penjualan WHERE Id_penjualan = @idPenjualan", con);
            cmd.Parameters.AddWithValue("@idPenjualan", idPenjualan);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        //Menu Pelanggan
        public void menuPelanggan(SqlConnection conn)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nMenu Pelanggan");
                    Console.WriteLine("--------------------");
                    Console.WriteLine("1. Tambah Pelanggan");
                    Console.WriteLine("2. Edit Pelanggan");
                    Console.WriteLine("3. Hapus Pelanggan");
                    Console.WriteLine("4. Cari Pelanggan");
                    Console.WriteLine("5. Seluruh Data Pelanggan");
                    Console.WriteLine("6. Kembali");
                    Console.WriteLine("\nMasukkan pilihan Anda (1-6): ");
                    char ch = Convert.ToChar(Console.ReadLine());
                    switch (ch)
                    {
                        case '1':
                            {
                                Console.Clear();
                                Console.WriteLine("Tambah Pelanggan\n");
                                bool inputValid = false;
                                while (!inputValid)
                                {
                                    try
                                    {
                                        Console.WriteLine("Masukkan Id Pelanggan (Format: pl_00000):");
                                        string idPelanggan = Console.ReadLine()?.Trim();
                                        if (idPelanggan?.ToLower() == "exit")
                                        {
                                            return;
                                        }

                                        if (string.IsNullOrEmpty(idPelanggan) || !idPelanggan.StartsWith("pl_") || !idPelanggan.Substring(3).All(char.IsDigit) || idPelanggan.Length != 8)
                                        {
                                            throw new FormatException("Format ID Pelanggan tidak valid. Harap masukkan sesuai format: pl_00000");
                                        }

                                        if (isPelangganExists(idPelanggan, conn))
                                        {
                                            Console.WriteLine($"ID Pelanggan {idPelanggan} sudah ada. Masukkan ID pelanggan yang berbeda.");
                                            continue;
                                        }

                                        Console.WriteLine("Masukkan Nama Pelanggan:");
                                        string namaPelanggan;
                                        do
                                        {
                                            namaPelanggan = Console.ReadLine()?.Trim();
                                            if (string.IsNullOrEmpty(namaPelanggan))
                                            {
                                                Console.WriteLine("Nama Pelanggan tidak boleh kosong. Harap masukkan nama pelanggan:");
                                            }
                                        } while (string.IsNullOrEmpty(namaPelanggan));

                                        Console.WriteLine("Masukkan Alamat Pelanggan:");
                                        string alamat;
                                        do
                                        {
                                            alamat = Console.ReadLine()?.Trim();
                                            if (string.IsNullOrEmpty(alamat))
                                            {
                                                Console.WriteLine("Alamat Pelanggan tidak boleh kosong. Harap masukkan alamat pelanggan:");
                                            }
                                        } while (string.IsNullOrEmpty(alamat));

                                        Console.WriteLine("Masukkan Nomor Telepon Pelanggan:");
                                        string noTelp;
                                        do
                                        {
                                            noTelp = Console.ReadLine()?.Trim();
                                            if (!noTelp.All(char.IsDigit) || noTelp.Length != 12)
                                            {
                                                Console.WriteLine("Nomor Telepon tidak valid. Harap masukkan hanya angka dan panjang maksimum 12 digit.");
                                            }
                                        } while (!noTelp.All(char.IsDigit) || noTelp.Length != 12);

                                        string email;
                                        do
                                        {
                                            Console.WriteLine("Masukkan Email Pelanggan:");
                                            email = Console.ReadLine()?.Trim();
                                            if (!IsValidemail(email))
                                            {
                                                Console.WriteLine("Email tidak valid. Harap masukkan email yang benar.");
                                            }
                                        } while (!IsValidemail(email));

                                        Console.WriteLine("Masukkan Id Penjualan (Format: ip_00000):");
                                        string idPenjualan = Console.ReadLine()?.Trim();

                                        if (!idPenjualan.StartsWith("ip_") || !idPenjualan.Substring(3).All(char.IsDigit) || idPenjualan.Length != 8)
                                        {
                                            throw new FormatException("Format ID Penjualan tidak valid. Harap masukkan sesuai format: ip_00000");
                                        }

                                        if (!isPenjualanExists(idPenjualan, conn))
                                        {
                                            throw new Exception($"ID penjualan {idPenjualan} tidak ditemukan. Masukkan ID penjualan yang valid.");
                                        }

                                        insertPelanggan(idPelanggan, namaPelanggan, alamat, noTelp, email, idPenjualan, conn);
                                        inputValid = true;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
                                    }
                                    catch (SqlException ex) when (ex.Number == 2627)
                                    {
                                        Console.WriteLine($"ID pelanggan sudah ada. Masukkan ID pelanggan yang berbeda.");
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nGagal menambahkan data pelanggan.");
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                            }
                            break;

                        case '2':
                            {
                                EditMenu:
                                Console.Clear();
                                Console.WriteLine("Edit Pelanggan\n");
                                string idPelanggan;

                                while (true)
                                {
                                    Console.WriteLine("Masukkan Id Pelanggan yang ingin diubah (ketik 'exit' untuk kembali):");
                                    idPelanggan = Console.ReadLine();
                                    if (idPelanggan.ToLower() == "exit")
                                    {
                                        return;
                                    }

                                    if (isPelangganExists(idPelanggan, conn))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nId Pelanggan tidak ditemukan. Silakan masukkan Id Pelanggan yang sesuai.\n");
                                    }
                                }

                                bool inputValid = false;
                                while (!inputValid)
                                {
                                    try
                                    {
                                        Console.WriteLine("\nPilih data yang ingin diubah:\n");
                                        Console.WriteLine("1. Nama Pelanggan");
                                        Console.WriteLine("2. Alamat");
                                        Console.WriteLine("3. Nomor Telepon");
                                        Console.WriteLine("4. Email");
                                        Console.WriteLine("5. Id Penjualan");
                                        Console.WriteLine("6. Selesai");

                                        Console.Write("\nPilihan Anda: ");
                                        int choice;
                                        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 6)
                                        {
                                            Console.WriteLine("Masukkan angka dari 1 hingga 6 sesuai dengan pilihan yang tersedia.");
                                            Console.Write("Pilihan Anda: ");
                                        }

                                        switch (choice)
                                        {
                                            case 1:
                                                Console.Clear();
                                                string newNama;
                                                do
                                                {
                                                    Console.WriteLine("\nMasukkan Nama Pelanggan Baru:");
                                                    newNama = Console.ReadLine()?.Trim();
                                                    if (string.IsNullOrEmpty(newNama))
                                                    {
                                                        Console.WriteLine("Nama Pelanggan tidak boleh kosong.");
                                                    }
                                                } while (string.IsNullOrEmpty(newNama));

                                                updatePelanggan(idPelanggan, newNama, null, null, null, null, conn);
                                                inputValid = true;
                                                break;

                                            case 2:
                                                Console.Clear();
                                                string newAlamat;
                                                do
                                                {
                                                    Console.WriteLine("\nMasukkan Alamat Baru:");
                                                    newAlamat = Console.ReadLine()?.Trim();
                                                    if (string.IsNullOrEmpty(newAlamat))
                                                    {
                                                        Console.WriteLine("Alamat tidak boleh kosong.");
                                                    }
                                                } while (string.IsNullOrEmpty(newAlamat));

                                                updatePelanggan(idPelanggan, null, newAlamat, null, null, null, conn);
                                                inputValid = true;
                                                break;

                                            case 3:
                                                Console.Clear();
                                                string newNoTelp;
                                                do
                                                {
                                                    Console.WriteLine("\nMasukkan Nomor Telepon Baru:");
                                                    newNoTelp = Console.ReadLine()?.Trim();
                                                    if (string.IsNullOrEmpty(newNoTelp))
                                                    {
                                                        Console.WriteLine("Nomor Telepon tidak boleh kosong.");
                                                    }
                                                } while (string.IsNullOrEmpty(newNoTelp));

                                                updatePelanggan(idPelanggan, null, null, newNoTelp, null, null, conn);
                                                inputValid = true;
                                                break;

                                            case 4:
                                                Console.Clear();
                                                string newEmail;
                                                do
                                                {
                                                    Console.WriteLine("\nMasukkan Email Baru:");
                                                    newEmail = Console.ReadLine()?.Trim();
                                                    if (string.IsNullOrEmpty(newEmail))
                                                    {
                                                        Console.WriteLine("Email tidak boleh kosong.");
                                                    }
                                                } while (string.IsNullOrEmpty(newEmail));

                                                updatePelanggan(idPelanggan, null, null, null, newEmail, null, conn);
                                                inputValid = true;
                                                break;

                                            case 5:
                                                Console.Clear();
                                                string idPenjualan;
                                                do
                                                {
                                                    Console.WriteLine("\nMasukkan Id Penjualan (Format: ip_00000):");
                                                    idPenjualan = Console.ReadLine()?.Trim();
                                                    if (string.IsNullOrEmpty(idPenjualan) || !idPenjualan.StartsWith("ip_") || idPenjualan.Length != 8 || !idPenjualan.Substring(3).All(char.IsDigit))
                                                    {
                                                        Console.WriteLine("Format ID Penjualan tidak valid. Harap masukkan sesuai format: ip_00000");
                                                    }
                                                } while (string.IsNullOrEmpty(idPenjualan) || !idPenjualan.StartsWith("ip_") || idPenjualan.Length != 8 || !idPenjualan.Substring(3).All(char.IsDigit));

                                                updatePelanggan(idPelanggan, null, null, null, null, idPenjualan, conn);
                                                inputValid = true;
                                                break;

                                            case 6:
                                                Console.Clear();
                                                inputValid = true;
                                                goto EditMenu;

                                            default:
                                                Console.Clear();
                                                Console.WriteLine("Pilihan tidak valid.");
                                                break;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nGagal mengubah data pelanggan.");
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                            }
                            break;

                        case '3':
                            {
                                Console.Clear();
                                Console.WriteLine("Hapus Pelanggan\n");
                                string idPelanggan;

                                while (true)
                                {
                                    Console.WriteLine("Masukkan Id Pelanggan yang ingin Dihapus:");
                                    idPelanggan = Console.ReadLine();
                                    if (idPelanggan.ToLower() == "exit")
                                    {
                                        return;
                                    }

                                    if (isPelangganExists(idPelanggan, conn))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nMasukkan Id_pelanggan yang sesuai.\n");
                                    }
                                }

                                try
                                {
                                    deletePelanggan(idPelanggan, conn);
                                    Console.WriteLine("Data Berhasil Dihapus");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\nGagal menghapus data pelanggan.");
                                    Console.WriteLine(e.ToString());
                                }
                            }
                            break;

                        case '4':
                            {
                                Console.Clear();
                                Console.WriteLine("Cari Pelanggan\n");
                                string idPelanggan;
                                while (true)
                                {
                                    Console.WriteLine("Masukkan ID Pelanggan yang ingin dicari:");
                                    idPelanggan = Console.ReadLine();
                                    if (idPelanggan.ToLower() == "exit")
                                    {
                                        return;
                                    }

                                    if (isPelangganExists(idPelanggan, conn))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nMasukkan Id_pelanggan yang sesuai.\n");
                                    }
                                }

                                Console.WriteLine("\nMenampilkan Pelanggan:");
                                Console.WriteLine("---------------------");
                                searchPelanggan(idPelanggan, conn);

                            }
                            break;


                        case '5':
                            {
                                Console.Clear();
                                Console.WriteLine("Seluruh Data Pelanggan");
                                Console.WriteLine("----------------------");
                                Console.WriteLine();
                                bacaPelanggan(conn);
                            }
                            break;
                        case '6':
                            return;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("\nPilihan tidak valid");
                            }
                            break;
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("\nPeriksa nilai yang dimasukkan");
                }
            }
        }

        public void bacaPelanggan(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT Id_pelanggan, Nama_pelanggan, Alamat, No_telepon, Email, Id_penjualan FROM Pelanggan", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine("ID Pelanggan  : " + r["Id_pelanggan"]);
                Console.WriteLine("Nama Pelanggan: " + r["Nama_pelanggan"]);
                Console.WriteLine("Alamat        : " + r["Alamat"]);
                Console.WriteLine("No Telepon    : " + r["No_telepon"]);
                Console.WriteLine("Email         : " + r["Email"]);
                Console.WriteLine("ID Penjualan  : " + r["Id_penjualan"]);
                Console.WriteLine();
            }
            r.Close();
        }


        public void insertPelanggan(string idPelanggan, string namaPelanggan, string alamat, string noTelp, string email, string idPenjualan, SqlConnection con)
        {
            string str = "INSERT INTO Pelanggan (Id_pelanggan, Nama_pelanggan, Alamat, No_telepon, Email, Id_penjualan) " +
                        "VALUES (@idPelanggan, @namaPelanggan, @alamat, @noTelp, @email, @idPenjualan)";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("idPelanggan", idPelanggan));
            cmd.Parameters.Add(new SqlParameter("namaPelanggan", namaPelanggan));
            cmd.Parameters.Add(new SqlParameter("alamat", alamat));
            cmd.Parameters.Add(new SqlParameter("noTelp", noTelp));
            cmd.Parameters.Add(new SqlParameter("email", email));
            cmd.Parameters.Add(new SqlParameter("idPenjualan", idPenjualan));
            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Ditambahkan");
        }

        public void updatePelanggan(string idPelanggan, string newNamaPelanggan, string newAlamat, string newNoTelp, string newEmail, string idPenjualan, SqlConnection con)
        {
            string str = "UPDATE Pelanggan SET ";

            if (!string.IsNullOrEmpty(newNamaPelanggan))
            {
                str += "Nama_pelanggan = @newNamaPelanggan, ";
            }
            if (!string.IsNullOrEmpty(newAlamat))
            {
                str += "Alamat = @newAlamat, ";
            }
            if (!string.IsNullOrEmpty(newNoTelp))
            {
                str += "No_telepon = @newNoTelp, ";
            }
            if (!string.IsNullOrEmpty(newEmail))
            {
                str += "Email = @newEmail, ";
            }
            if (!string.IsNullOrEmpty(idPenjualan))
            {
                str += "Id_penjualan = @idPenjualan, ";
            }

            str = str.TrimEnd(',', ' ');

            str += " WHERE Id_pelanggan = @idPelanggan";

            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            if (!string.IsNullOrEmpty(newNamaPelanggan))
            {
                cmd.Parameters.AddWithValue("@newNamaPelanggan", newNamaPelanggan);
            }
            if (!string.IsNullOrEmpty(newAlamat))
            {
                cmd.Parameters.AddWithValue("@newAlamat", newAlamat);
            }
            if (!string.IsNullOrEmpty(newNoTelp))
            {
                cmd.Parameters.AddWithValue("@newNoTelp", newNoTelp);
            }
            if (!string.IsNullOrEmpty(newEmail))
            {
                cmd.Parameters.AddWithValue("@newEmail", newEmail);
            }
            if (!string.IsNullOrEmpty(idPenjualan))
            {
                cmd.Parameters.AddWithValue("@idPenjualan", idPenjualan);
            }

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil Diubah");
        }


        public void deletePelanggan(string idPelanggan, SqlConnection con)
        {
            string str = "DELETE FROM Pelanggan WHERE Id_pelanggan = @idPelanggan";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            cmd.ExecuteNonQuery();
        }

      public void searchPelanggan(string idPelanggan, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pelanggan WHERE Id_pelanggan = @idPelanggan", con);
            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
        {
        while (reader.Read())
        {
            Console.WriteLine("ID Pelanggan  : " + reader["Id_pelanggan"]);
            Console.WriteLine("Nama Pelanggan: " + reader["Nama_pelanggan"]);
            Console.WriteLine("Alamat        : " + reader["Alamat"]);
            Console.WriteLine("No Telepon    : " + reader["No_telepon"]);
            Console.WriteLine("Email         : " + reader["Email"]);
            Console.WriteLine("ID Penjualan  : " + reader["Id_penjualan"]);
        }
        }
        else
            {
                Console.WriteLine("Data tidak ditemukan.");
            }
            reader.Close();
        }
        private static bool IsValidemail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.IgnoreCase);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }


        public bool isPelangganExists(string idPelanggan, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Pelanggan WHERE Id_pelanggan = @idPelanggan", con);
            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        //Menu Gudang
        public void menuGudang(SqlConnection conn)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nMenu Gudang");
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("1. Simpan Produk");
                    Console.WriteLine("2. Hapus Produk");
                    Console.WriteLine("3. Seluruh Data Produk");
                    Console.WriteLine("4. Kembali");
                    Console.WriteLine("\nMasukkan pilihan Anda (1-4): ");
                    char ch = Convert.ToChar(Console.ReadLine());
                    switch (ch)
                    {
                        case '1':
                            {
                                Console.Clear();
                                Console.WriteLine("Simpan Produk di Gudang\n");
                                bool inputValid = false;
                                string idProduk = "";
                                string namaProduk = "";
                                int jumlahProduk = 0;
                                DateTime tglMasuk = DateTime.MinValue;
                                DateTime tglKadaluarsa = DateTime.MinValue;

                                while (!inputValid)
                                {
                                    try
                                    {
                                        if (string.IsNullOrEmpty(idProduk))
                                        {
                                            Console.WriteLine("Masukkan ID Produk (Format: p_0000):");
                                            idProduk = Console.ReadLine();
                                            if (idProduk.ToLower() == "exit")
                                            {
                                                return;
                                            }

                                            if (string.IsNullOrEmpty(idProduk) || !idProduk.StartsWith("p_") || !idProduk.Substring(2).All(char.IsDigit) || idProduk.Length != 6)
                                            {
                                                throw new FormatException("Format ID Produk tidak valid. Harap masukkan sesuai format: p_0000");
                                            }

                                            if (isProductExists(idProduk, conn))
                                            {
                                                Console.WriteLine($"ID produk {idProduk} sudah ada. Masukkan ID produk yang berbeda.");

                                                idProduk = "";
                                                continue;
                                            }

                                        }

                                        if (string.IsNullOrEmpty(namaProduk))
                                        {
                                            Console.WriteLine("Masukkan Nama Produk:");
                                            namaProduk = Console.ReadLine();
                                            while (string.IsNullOrEmpty(namaProduk))
                                            {
                                                Console.WriteLine("Nama Produk tidak boleh kosong. Harap masukkan nama produk:");
                                                namaProduk = Console.ReadLine()?.Trim();
                                            }
                                        }

                                        if (jumlahProduk <= 0)
                                        {
                                            Console.WriteLine("Masukkan Jumlah:");
                                            while (!int.TryParse(Console.ReadLine()?.Trim(), out jumlahProduk) || jumlahProduk < 0)
                                            {
                                                if (jumlahProduk < 0)
                                                {
                                                    Console.WriteLine("Jumlah produk tidak valid. Harap masukkan angka tidak kurang dari 0:");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Jumlah produk tidak valid. Harap masukkan angka:");
                                                }
                                            }
                                        }

                                        if (tglMasuk == DateTime.MinValue || tglMasuk < DateTime.Today)
                                        {
                                            Console.WriteLine("Masukkan Tanggal Masuk (YYYY-MM-DD):");
                                            string inputTglMasuk = Console.ReadLine()?.Trim();
                                            while (!DateTime.TryParseExact(inputTglMasuk, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tglMasuk) || tglMasuk < DateTime.Today)
                                            {
                                                if (tglMasuk < DateTime.Today)
                                                {
                                                    Console.WriteLine("Tanggal masuk tidak boleh kurang dari hari ini. Harap masukkan kembali:");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Format tanggal tidak valid. Harap masukkan kembali (YYYY-MM-DD):");
                                                }
                                                inputTglMasuk = Console.ReadLine()?.Trim();
                                            }
                                        }

                                        if (tglKadaluarsa == DateTime.MinValue || tglKadaluarsa < tglMasuk)
                                        {
                                            Console.WriteLine("Masukkan Tanggal Kadaluarsa (YYYY-MM-DD):");
                                            string inputTglKadaluarsa = Console.ReadLine()?.Trim();
                                            while (!DateTime.TryParseExact(inputTglKadaluarsa, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tglKadaluarsa) || tglKadaluarsa < tglMasuk)
                                            {
                                                if (tglKadaluarsa < tglMasuk)
                                                {
                                                    Console.WriteLine("Tanggal kadaluarsa tidak boleh kurang dari tanggal masuk. Harap masukkan kembali:");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Format tanggal tidak valid. Harap masukkan kembali (YYYY-MM-DD):");
                                                }
                                                inputTglKadaluarsa = Console.ReadLine()?.Trim();
                                            }
                                        }


                                        simpanProdukGudang(idProduk, namaProduk, jumlahProduk, tglMasuk, tglKadaluarsa, conn);

                                        Console.WriteLine("Produk Berhasil Disimpan di Gudang");
                                        inputValid = true;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
                                    }
                                    catch (SqlException ex) when (ex.Number == 2627)
                                    {
                                        Console.WriteLine($"ID produk {idProduk} sudah ada. Masukkan ID produk yang berbeda.");
                                        continue;
                                    }


                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\nGagal menyimpan produk di gudang.");
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                            }
                            break;
                        case '2':
                            {
                                Console.Clear();
                                Console.WriteLine("Hapus Produk dari Gudang\n");
                                string idProduk;
                                while (true)
                                {
                                    Console.WriteLine("Masukkan ID Produk yang Ingin Dihapus:");
                                    idProduk = Console.ReadLine();
                                    if (idProduk.ToLower() == "exit")
                                    {
                                        return;
                                    }

                                    if (isProdukExists(idProduk, conn))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nMasukkan Id_produk yang sesuai.\n");
                                    }

                                }
                                try
                                {
                                    hapusProdukGudang(idProduk, conn);
                                    Console.WriteLine("Data Berhasil Dihapus");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\nGagal menghapus produk dari gudang.");
                                    Console.WriteLine(e.ToString());
                                }
                            }
                            break;

                        case '3':
                            {
                                Console.Clear();
                                Console.WriteLine("Seluruh Data Produk");
                                Console.WriteLine("-----------");
                                Console.WriteLine();
                                tampilkanSeluruhDataProdukGudang(conn);
                            }
                            break;
                        case '4':
                            return;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("\nPilihan tidak valid");
                            }
                            break;
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("\nPeriksa nilai yang dimasukkan");
                }
            }
        }

        public void simpanProdukGudang(string idProduk, string namaProduk, int jumlahProduk, DateTime tglMasuk, DateTime tglKadaluarsa, SqlConnection con)
        {
            string str = "INSERT INTO Gudang (Id_produk, Nama_produk, Jumlah, Tgl_masuk, Tgl_kadaluarsa) " +
                         "VALUES (@idProduk, @namaProduk, @jumlahProduk, @tglMasuk, @tglKadaluarsa)";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@idProduk", idProduk));
            cmd.Parameters.Add(new SqlParameter("@namaProduk", namaProduk));
            cmd.Parameters.Add(new SqlParameter("@jumlahProduk", jumlahProduk));
            cmd.Parameters.Add(new SqlParameter("@tglMasuk", tglMasuk));
            cmd.Parameters.Add(new SqlParameter("@tglKadaluarsa", tglKadaluarsa));

            cmd.ExecuteNonQuery();
        }

        public void hapusProdukGudang(string idProduk, SqlConnection con)
        {
            string str = "DELETE FROM Gudang WHERE Id_produk = @idProduk";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@idProduk", idProduk);
            cmd.ExecuteNonQuery();
        }

        public void tampilkanSeluruhDataProdukGudang(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT Id_produk, Nama_produk, Jumlah, Tgl_masuk, Tgl_kadaluarsa FROM Gudang", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine("ID Produk         : " + r["Id_produk"]);
                Console.WriteLine("Nama Produk       : " + r["Nama_produk"]);
                Console.WriteLine("Jumlah            : " + r["Jumlah"]);
                Console.WriteLine("Tanggal Masuk     : " + r["Tgl_masuk"]);
                Console.WriteLine("Tanggal Kadaluarsa: " + r["Tgl_kadaluarsa"]);
                Console.WriteLine();
            }
            r.Close();
        }

        public bool isProdukExists(string idProduk, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Produk WHERE Id_produk = @idProduk", con);
            cmd.Parameters.AddWithValue("@idProduk", idProduk);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }
}
