using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.Model.Models
{
    public class EmailSettings
    {
        [Key]
        public int Id { get; set; }
        public string EmailHost { get; set; }
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
        public int Port { get; set; }
    }
}
