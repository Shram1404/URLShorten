using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace URLShorten.Models
{
    public class URLShort
    {
        [Key]
        public int URLId { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        public string FullURL { get; set; }

        public string ShortURL { get; set; }

        public string CreatedBy { get; set; }

        public string? OwnerID { get; set; }
    }
}
