using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenericRepositoryEFCore.TestingSamples.Models
{
    public class PostEntity
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Contents { get; set; }

        public DateTime? CreatedOn { get; set; }

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        public AuthorEntity? Author { get; set; }
    }
}
