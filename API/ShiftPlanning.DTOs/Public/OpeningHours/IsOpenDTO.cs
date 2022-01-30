namespace ShiftPlanning.DTOs.Public.OpeningHours
{
    /// <summary>
    /// Contains information used to identify whether or not a given organization is open.
    /// </summary>
    public class IsOpenDTO
    {
        /// <summary>
        /// States whether or not a given organization is open.
        /// </summary>
        public bool Open { get; set; }
    }
}
