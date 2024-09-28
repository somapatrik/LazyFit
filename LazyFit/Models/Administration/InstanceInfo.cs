namespace LazyFit.Models.Administration
{
    public class InstanceInfo
    {
        public Guid InstanceId { get; set; }
        public string DeviceType { get; set; }
        public string Platform { get; set; }
        public bool IsVirtual { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }

        public InstanceInfo() { }

        public InstanceInfo(string deviceType, string platform, bool isVirtual, DateTime updateDate, DateTime createDate)
        {
            InstanceId = Guid.NewGuid();
            DeviceType = deviceType;
            Platform = platform;
            IsVirtual = isVirtual;
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
        }

        public void UpdateInstanceInfo(string deviceType, string platform, bool isVirtual)
        {
            DeviceType = deviceType;
            Platform = platform;
            IsVirtual = isVirtual;
            UpdateDate = DateTime.Now;
        }
    }
}
