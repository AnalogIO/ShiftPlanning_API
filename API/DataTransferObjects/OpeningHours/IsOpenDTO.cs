using Newtonsoft.Json;

namespace DataTransferObjects.OpeningHours
{
    /// <summary>
    /// Contains information used to identify whether or not a given institution is open.
    /// </summary>
    public class IsOpenDTO
    {
        /// <summary>
        /// States whether or not a given institution is open.
        /// </summary>
        [JsonProperty("open")]
        public bool Open { get; set; }
    }
}
