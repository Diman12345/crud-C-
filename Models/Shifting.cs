

using System.Text.Json.Serialization;

public class Shifting
{
    public int Id { get; set; }
    public string? Waktu { get; set; }

    // Relasi one-to-many ke Product
    public List<Pegawai>? Pegawais { get; set; }
}
