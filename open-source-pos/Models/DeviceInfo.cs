namespace Models
{
    /// <summary>
    /// represents device detected information including browser, os and other useful information regarding the device. The information is based on user-agent.
    /// </summary>
    public class DeviceInfo
    {
        public string browser { get; set; }
        public string os { get; set; }
        public string device { get; set; }
        public string userAgent { get; set; }
        public string os_version { get; set; }
        public bool isMobile { get; set; }
        public bool isTablet { get; set; }
        public bool isDesktop { get; set; }
    }
}