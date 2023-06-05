namespace Models
{

    /// <summary>
    /// represents the complete location information for an IP address.
    /// </summary>
    public class UserGeoLocation
    {
        /// <summary>
        /// public (external) IP address
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// city name
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// region name (administrative division)
        /// </summary>
        public string region { get; set; }
        /// <summary>
        /// region code
        /// </summary>
        public string region_code { get; set; }
        /// <summary>
        /// country code (two letter, ISO 3166-1 alpha-2)
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// country name
        /// </summary>
        public string country_name { get; set; }
        /// <summary>
        /// continent code
        /// </summary>
        public string continent_code { get; set; }
        /// <summary>
        /// whether IP address belongs to a country that is a member of European Union (EU)
        /// </summary>
        public bool? in_eu { get; set; }
        /// <summary>
        /// postal code
        /// </summary>
        public string postal { get; set; }
        /// <summary>
        /// latitude
        /// </summary>
        public double? latitude { get; set; }
        /// <summary>
        /// longitude
        /// </summary>
        public double? longitude { get; set; }
        /// <summary>
        /// timezone (IANA format i.e. “Area/Location”)
        /// </summary>
        public string timezone { get; set; }
        /// <summary>
        /// UTC offset as +HHMM or -HHMM (HH is hours, MM is minutes)
        /// </summary>
        public string utc_offset { get; set; }
        /// <summary>
        /// country calling code (dial in code, comma separated)
        /// </summary>
        public string country_calling_code { get; set; }
        /// <summary>
        /// currency code (ISO 4217)
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// languages spoken (comma separated 2 or 3 letter ISO 639 code with optional hyphen separated country suffix)
        /// </summary>
        public string languages { get; set; }
        /// <summary>
        /// autonomous system number
        /// </summary>
        public string asn { get; set; }
        /// <summary>
        /// organinzation name
        /// </summary>
        public string org { get; set; }
    }

}