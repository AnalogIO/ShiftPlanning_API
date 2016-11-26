namespace DataTransferObjects.Public.Employees
{
    /// <summary>
    /// Contains public information about the volunteers in an institution.
    /// </summary>
    public class EmployeeDTO
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
        public string PhotoRef { get; set; }
    }
}
