namespace DataTransferObjects.Volunteers
{
    /// <summary>
    /// Contains public information about the volunteers in an institution.
    /// </summary>
    public class VolunteerDTO
    {
        /// <summary>
        /// The shown name of the volunteer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The title of the volunteer. This can for instance be "Barista".
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A photo of the volunteer.
        /// </summary>
        public byte[] Photo { get; set; }
    }
}
