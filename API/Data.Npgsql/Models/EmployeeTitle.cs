namespace Data.Npgsql.Models
{
    public class EmployeeTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual Institution Institution { get; set; }
    }
}