using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenericRepositoryEFCore.TestingSamples.Models
{
    public class AuthorEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public IEnumerable<PostEntity> Posts { get; set; } = Enumerable.Empty<PostEntity>();
    }
}
