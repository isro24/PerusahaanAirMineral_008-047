using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
                                        Console.WriteLine("Masukkan Id Produk (Format: p_0000):");
                                        string idProduk = Console.ReadLine();
                                        if (idProduk.ToLower() == "exit")
                                        {
                                            return;
                                        }

                                        if (!idProduk.StartsWith("p_") || !idProduk.Substring(2).All(char.IsDigit) || idProduk.Length != 6)
                                        {
                                            throw new FormatException("Format id_produk tidak valid. Harap masukkan sesuai format: p_0000");
                                        }

                                        Console.WriteLine("Masukkan Nama Produk :");
                                        string namaProduk = Console.ReadLine();
                                        Console.WriteLine("Masukkan Deskripsi Produk :");
                                        string deskripsi = Console.ReadLine();
                                        Console.WriteLine("Masukkan Tanggal Kadaluarsa (YYYY-MM-DD):");
                                        DateTime tglKadaluarsa = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("Masukkan Stok Produk :");
                                        int stok = int.Parse(Console.ReadLine());

                                        if (stok < 0)
                                        {
                                            throw new FormatException("Stok tidak valid. Harap masukkan angka tidak kurang dari 0.");
                                        }

                                        Console.WriteLine("Masukkan Jumlah Tersedia :");
                                        int jumlahTersedia = int.Parse(Console.ReadLine());

                                        if (jumlahTersedia < 0)
                                        {
                                            throw new FormatException("Jumlah Tersedia tidak valid. Harap masukkan angka tidak kurang dari 0.");
                                        }

                                        insert(idProduk, namaProduk, deskripsi, tglKadaluarsa, stok, jumlahTersedia, conn);
                                        inputValid = true;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
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
                                Console.Clear();
                                Console.WriteLine("Edit Produk\n");
                                string Id;

                                while (true)
                                {
                                    Console.WriteLine("Masukkan Id Produk yang ingin diUbah:");
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
                                        Console.WriteLine("Masukkan Nama Produk Baru:");
                                        string newNama = Console.ReadLine();
                                        if (newNama.ToLower() == "exit")
                                        {
                                            return;
                                        }
                                        Console.WriteLine("Masukkan Deskripsi Baru:");
                                        string newDeskripsi = Console.ReadLine();
                                        Console.WriteLine("Masukkan Tanggal Kadaluarsa Baru (YYYY-MM-DD):");
                                        DateTime newTgl = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("Masukkan Stok Baru:");
                                        int newStok = int.Parse(Console.ReadLine());

                                        if (newStok < 0)
                                        {
                                            throw new FormatException("Stok baru tidak valid. Harap masukkan angka tidak kurang dari 0.");
                                        }

                                        Console.WriteLine("Masukkan Jumlah Tersedia Baru:");
                                        int newJumlah = int.Parse(Console.ReadLine());

                                        if (newJumlah < 0)
                                        {
                                            throw new FormatException("Jumlah tersedia baru tidak valid. Harap masukkan angka tidak kurang dari 0.");
                                        }

                                        update(Id, newNama, newDeskripsi, newTgl, newStok, newJumlah, conn);
                                        inputValid = true;
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
                                        Console.WriteLine("\nMasukkan Id_produk yang sesuai.\n");
                                    }
                                }

                                try
                                {
                                    delete(idProduk, conn);
                                    Console.WriteLine("Data Berhasil Dihapus");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\nGagal menghapus data produk.");
                                    Console.WriteLine(e.ToString());
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
            string str = "UPDATE Produk SET Nama_produk = @newNama, Deskripsi = @newDeskripsi, Tgl_Kadaluarsa = @newTgl, Stok = @newStok, Jumlah_tersedia = @newJumlah WHERE Id_produk = @Id";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@newNama", newNama);
            cmd.Parameters.AddWithValue("@newDeskripsi", newDeskripsi);
            cmd.Parameters.AddWithValue("@newTgl", newTgl);
            cmd.Parameters.AddWithValue("@newStok", newStok);
            cmd.Parameters.AddWithValue("@newJumlah", newJumlah);
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
                                        string idPenjualan = Console.ReadLine();
                                        if (idPenjualan.ToLower() == "exit")
                                        {
                                            return;
                                        }

                                        if (!idPenjualan.StartsWith("ip_") || !idPenjualan.Substring(3).All(char.IsDigit) || idPenjualan.Length != 8)
                                        {
                                            throw new FormatException("Format ID Penjualan tidak valid. Harap masukkan sesuai format: ip_00000");
                                        }

                                        Console.WriteLine("Masukkan Tanggal Penjualan (YYYY-MM-DD):");
                                        DateTime tanggalPenjualan = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("Masukkan Jumlah Terjual:");
                                        int jumlahTerjual = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Masukkan Harga Jual:");
                                        decimal hargaJual = decimal.Parse(Console.ReadLine());
                                        Console.WriteLine("Masukkan Metode Pembayaran:");
                                        string metodePembayaran = Console.ReadLine();
                                        Console.WriteLine("Masukkan ID Produk (Format: p_0000):");
                                        string idProduk = Console.ReadLine();

                                       if (!idProduk.StartsWith("p_") || !idProduk.Substring(2).All(char.IsDigit) || idProduk.Length != 6)
                                        {
                                            throw new FormatException("Format ID Produk tidak valid. Harap masukkan sesuai format: p_0000");
                                        }

                                        insertPenjualan(idPenjualan, tanggalPenjualan, jumlahTerjual, hargaJual, metodePembayaran, idProduk, conn);
                                        inputValid = true;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
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
                                        Console.WriteLine("Masukkan Tanggal Penjualan Baru (YYYY-MM-DD):");
                                        string inputTanggalPenjualan = Console.ReadLine();
                                        if (inputTanggalPenjualan.ToLower() == "exit")
                                        {
                                            return;
                                        }
                                        DateTime newTanggalPenjualan = DateTime.Parse(inputTanggalPenjualan);
                                        Console.WriteLine("Masukkan Jumlah Terjual Baru:");
                                        int newJumlahTerjual = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Masukkan Harga Jual Baru:");
                                        decimal newHargaJual = decimal.Parse(Console.ReadLine());
                                        Console.WriteLine("Masukkan Metode Pembayaran Baru:");
                                        string newMetodePembayaran = Console.ReadLine();
                                        Console.WriteLine("Masukkan ID Produk Baru (Format: p_0000):");
                                        string newIdProduk = Console.ReadLine();

                                        if (!newIdProduk.StartsWith("p_") || !newIdProduk.Substring(2).All(char.IsDigit) || newIdProduk.Length != 6)
                                        {
                                            throw new FormatException("Format ID Produk tidak valid. Harap masukkan sesuai format: p_0000");
                                        }

                                        update(Id, newTanggalPenjualan, newJumlahTerjual, newHargaJual, newMetodePembayaran, newIdProduk, conn);
                                        inputValid = true;
                                    }
                                    catch (FormatException)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nFormat input tidak valid. Harap masukkan data yang sesuai.\n");
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
                                    Console.WriteLine("\nGagal menghapus data penjualan.");
                                    Console.WriteLine(e.ToString());
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

        
        public void update(string Id, DateTime newTanggalPenjualan, int newJumlahTerjual, decimal newHargaJual, string newMetodePembayaran, string newIdProduk, SqlConnection con)
        {
            string str = "UPDATE Penjualan SET Tanggal_penjualan = @newTanggalPenjualan, Jumlah_terjual = @newJumlahTerjual, Harga_jual = @newHargaJual, " +
                "Metode_pembayaran = @newMetodePembayaran, Id_produk = @newIdProduk WHERE Id_penjualan = @Id";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@newTanggalPenjualan", newTanggalPenjualan);
            cmd.Parameters.AddWithValue("@newJumlahTerjual", newJumlahTerjual);
            cmd.Parameters.AddWithValue("@newHargaJual", newHargaJual);
            cmd.Parameters.AddWithValue("@newMetodePembayaran", newMetodePembayaran);
            cmd.Parameters.AddWithValue("@newIdProduk", newIdProduk);
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
                                        string idPelanggan = Console.ReadLine();
                                        if (idPelanggan.ToLower() == "exit")
                                        {
                                            return;
                                        }

                                        if (!idPelanggan.StartsWith("pl_") || !idPelanggan.Substring(3).All(char.IsDigit) || idPelanggan.Length != 8)
                                        {
                                            throw new FormatException("Format ID Pelanggan tidak valid. Harap masukkan sesuai format: pl_00000");
                                        }

                                        Console.WriteLine("Masukkan Nama Pelanggan:");
                                        string namaPelanggan = Console.ReadLine();
                                        Console.WriteLine("Masukkan Alamat Pelanggan:");
                                        string alamat = Console.ReadLine();
                                        Console.WriteLine("Masukkan Nomor Telepon Pelanggan:");
                                        string noTelp = Console.ReadLine();

                                        if (!noTelp.All(char.IsDigit) || noTelp.Length != 12)
                                        {
                                            throw new FormatException("Nomor Telepon tidak valid. Harap masukkan hanya angka dan panjang maksimum 12 digit.");
                                        }

                                        Console.WriteLine("Masukkan Email Pelanggan:");
                                        string email = Console.ReadLine();
                                        Console.WriteLine("Masukkan Id Penjualan (Format: ip_00000):");
                                        string idPenjualan = Console.ReadLine();

                                        if (!idPenjualan.StartsWith("ip_") || !idPenjualan.Substring(3).All(char.IsDigit) || idPenjualan.Length != 8)
                                        {
                                            throw new FormatException("Format ID Penjualan tidak valid. Harap masukkan sesuai format: ip_00000");
                                        }

                                        insertPelanggan(idPelanggan, namaPelanggan, alamat, noTelp, email, idPenjualan, conn);
                                        Console.WriteLine("Data Pelanggan Berhasil Ditambahkan");
                                        inputValid = true;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
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
                                Console.Clear();
                                Console.WriteLine("Edit Pelanggan\n");
                                string idPelanggan;

                                while (true)
                                {
                                    Console.WriteLine("Masukkan Id Pelanggan yang ingin diubah:");
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
                                        Console.WriteLine("Masukkan Nama Pelanggan Baru:");
                                        string newNamaPelanggan = Console.ReadLine();
                                        if (newNamaPelanggan.ToLower() == "exit")
                                        {
                                            return;
                                        }
                                        Console.WriteLine("Masukkan Alamat Baru:");
                                        string newAlamat = Console.ReadLine();
                                        Console.WriteLine("Masukkan Nomor Telepon Baru:");
                                        string newNoTelp = Console.ReadLine();

                                        if (!newNoTelp.All(char.IsDigit) || newNoTelp.Length != 12)
                                        {
                                            throw new FormatException("Nomor Telepon tidak valid. Harap masukkan hanya angka dan panjang maksimum 12 digit.");
                                        }

                                        Console.WriteLine("Masukkan Email Baru:");
                                        string newEmail = Console.ReadLine();
                                        Console.WriteLine("Masukkan Id Penjualan (Format: ip_00000):");
                                        string idPenjualan = Console.ReadLine();

                                        if (!idPenjualan.StartsWith("ip_") || !idPenjualan.Substring(3).All(char.IsDigit) || idPenjualan.Length != 8)
                                        {
                                            throw new FormatException("Format ID Penjualan tidak valid. Harap masukkan sesuai format: ip_00000");
                                        }

                                        updatePelanggan(idPelanggan, newNamaPelanggan, newAlamat, newNoTelp, newEmail, idPenjualan, conn);

                                        Console.WriteLine("Data Pelanggan Berhasil Diubah");
                                        inputValid = true;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
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
            string str = "UPDATE Pelanggan SET Nama_pelanggan = @newNamaPelanggan, Alamat = @newAlamat, No_telepon = @newNoTelp, Email = @newEmail, Id_penjualan = @idPenjualan WHERE Id_pelanggan = @idPelanggan";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            cmd.Parameters.AddWithValue("@newNamaPelanggan", newNamaPelanggan);
            cmd.Parameters.AddWithValue("@newAlamat", newAlamat);
            cmd.Parameters.AddWithValue("@newNoTelp", newNoTelp);
            cmd.Parameters.AddWithValue("@newEmail", newEmail);
            cmd.Parameters.AddWithValue("@idPenjualan", idPenjualan);
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
                                while (!inputValid)
                                {
                                    try
                                    {
                                        Console.WriteLine("Masukkan ID Produk (Format: p_0000):");
                                        string idProduk = Console.ReadLine();
                                        if (idProduk.ToLower() == "exit")
                                        {
                                            return;
                                        }

                                        if (!idProduk.StartsWith("p_") || !idProduk.Substring(2).All(char.IsDigit) || idProduk.Length != 6)
                                        {
                                            throw new FormatException("Format ID Produk tidak valid. Harap masukkan sesuai format: p_0000");
                                        }

                                        Console.WriteLine("Masukkan Nama Produk:");
                                        string namaProduk = Console.ReadLine();
                                        Console.WriteLine("Masukkan Jumlah:");
                                        int jumlahProduk = int.Parse(Console.ReadLine());

                                        if (jumlahProduk < 0)
                                        {
                                            throw new FormatException("Jumlah produk tidak valid. Harap masukkan angka tidak kurang dari 0.");
                                        }

                                        Console.WriteLine("Masukkan Tanggal Masuk (YYYY-MM-DD):");
                                        DateTime tglMasuk = DateTime.Parse(Console.ReadLine());
                                        Console.WriteLine("Masukkan Tanggal Kadaluarsa (YYYY-MM-DD):");
                                        DateTime tglKadaluarsa = DateTime.Parse(Console.ReadLine());

                                        simpanProdukGudang(idProduk, namaProduk, jumlahProduk, tglMasuk, tglKadaluarsa, conn);

                                        Console.WriteLine("Produk Berhasil Disimpan di Gudang");
                                        inputValid = true;
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n" + ex.Message + "\n");
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
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Gudang WHERE Id_produk = @idProduk", con);
            cmd.Parameters.AddWithValue("@idProduk", idProduk);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }
}
