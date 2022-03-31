namespace testTaskUzkikh.Models
{
    public class UNP
    {
        public long UnpId { get; set; }
        public long VUNP { get; set; }
        public string VNAIMP { get; set; }
        public string VNAIMK { get; set; }
        public DateTime DREG { get; set; }
        public long NMNS { get; set; }
        public string VMNS { get; set; }
        public int CKODSOST { get; set; }
        public string VKODS { get; set; }
        public DateTime? DLIKV { get; set; }
        public string? VLIKV { get; set; }
        public User User { get; set; }
    }
}
