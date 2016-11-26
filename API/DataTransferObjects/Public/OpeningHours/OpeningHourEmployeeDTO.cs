namespace DataTransferObjects.Public.OpeningHours
{
    /// <summary>
    /// Containing public information about a single employee in an institution.
    /// </summary>
    public class OpeningHourEmployeeDTO
    {
        /// <summary>
        /// The employee ID. Can be used across Employees and CheckedInEmployees to identify the same employee.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The first name of the employee.
        /// </summary>
        public string FirstName { get; set; }
    }
}