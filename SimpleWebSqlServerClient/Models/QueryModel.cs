using System.ComponentModel.DataAnnotations;

namespace SimpleWebSqlServerClient.Models
{
    public class QueryModel
    {
        [Required]
        public string Query { get; set; }
    }
}