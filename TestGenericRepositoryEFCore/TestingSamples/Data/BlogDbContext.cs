using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGenericRepositoryEFCore.TestingSamples.Models;

namespace TestGenericRepositoryEFCore.TestingSamples.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        #nullable disable
        public DbSet<AuthorEntity> Authors { get; set; }

        public DbSet<PostEntity> Posts { get; set; }
        #nullable enable
    }
}
