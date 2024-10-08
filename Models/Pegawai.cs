public class Pegawai
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Nik { get; set; }
    public string? Alamat { get; set; }
    public string? Status { get; set; }
    public  int ShiftingId { get; set; }
    public int BarangId { get; set; }

    public int FasilitasId { get; set; }
    public Shifting? Shifting { get; set; }
    public Barang? Barang { get; set; }
    public Fasilitas? Fasilitas { get; set;}
}
