

using System.Text.Json.Serialization;

public class Barang
{
    public int Id { get; set; }
    public string? Nama { get; set; }


    public string? Serial_number { get; set; }

    public string? Type { get; set; }

    // Relasi one-to-many ke Product
    public List<Pegawai>? Pegawais { get; set; }

}
