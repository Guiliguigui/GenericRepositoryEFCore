using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGenericRepositoryEFCore.TestingSamples.Data;
using TestGenericRepositoryEFCore.TestingSamples.Models;

namespace TestGenericRepositoryEFCore
{
    public class GenericRepositoryTests
    {
        private DbContextOptions<BlogDbContext> dbContextOptions;

        public GenericRepositoryTests()
        {
            var dbName = $"AuthorPostsDb_{DateTime.Now.ToFileTimeUtc()}";
            dbContextOptions = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
        }

        [Fact]
        public async Task GetAuthorsAsync_Success_Test()
        {
            var repository = await CreateRepositoryAsync();

            // Act
            var authorList = await repository.FindAll();

            // Assert
            Assert.Equal(3, authorList.ToList().Count);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_Success_Test()
        {
            var repository = await CreateRepositoryAsync();

            // Act
            var author = await repository.Find(1);

            // Assert
            Assert.NotNull(author);
            Assert.Equal("Author_1", author.Name);
            Assert.Equal(2, author.Posts.ToList().Count);
        }

        [Fact]
        public async Task CreateAsync_Success_Test()
        {
            var repository = await CreateRepositoryAsync();

            // Act
            await repository.Create(new AuthorEntity()
            {
                Name = "Some Author",
                Posts = new List<PostEntity>()
                {
                    new PostEntity { Title = "Some Post Title", Contents = "Some Contents" }
                }
            });

            // Assert
            var authorList = await repository.FindAll();
            Assert.Equal(4, authorList.ToList().Count);
        }

        [Fact]
        public async Task DeleteAsync_Success_Test()
        {
            var repository = await CreateRepositoryAsync();

            // Act
            await repository.Delete(3);

            // Assert
            var authorList = await repository.FindAll();
            Assert.Equal(2, authorList.ToList().Count);
        }

        private async Task<GenericRepository<AuthorEntity>> CreateRepositoryAsync()
        {
            BlogDbContext context = new BlogDbContext(dbContextOptions);
            await PopulateDataAsync(context);
            return new GenericRepository<AuthorEntity>(context);
        }

        private async Task PopulateDataAsync(BlogDbContext context)
        {
            int index = 1;

            while (index <= 3)
            {
                var author = new AuthorEntity()
                {
                    Name = $"Author_{index}",
                    Posts = new List<PostEntity>()
                    {
                        new PostEntity
                        {
                            Title = "First", Contents = "Some contents",
                            CreatedOn = DateTime.Now
                        },
                        new PostEntity
                        {
                            Title = "Second", Contents = "Some contents",
                            CreatedOn = DateTime.Now
                        },
                    }
                };

                index++;
                await context.Authors.AddAsync(author);
            }

            await context.SaveChangesAsync();
        }
    }
}
