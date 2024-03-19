using PostersWebDbLayer;
using PosterWebDBLibrary;
using PosterModels;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using PosterWebDBContext;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PostersWebDatabaseLayer_Tests;

public class TestPostersRepository
{
    private PostersRepository _repo;
    private DbContextOptions<PosterWebDbContext> _options;

    public TestPostersRepository()
    {
        SetupOptions();
        SeedData();
    }

    private void SetupOptions()
    {
        _options = new DbContextOptionsBuilder<PosterWebDbContext>()
                    .UseInMemoryDatabase(databaseName: "PosterWebManagerDbTests")
                    .Options;
    }

    private List<Category> Categories()
    {
        //create at least two categories
        return new List<Category>(){
            new Category() { Id = 1, Name = "Retro" },
            new Category() { Id = 2, Name = "Sci-Fi" }
        };
    }

    private List<Poster> Posters()
    {
        //create at least three items
        return new List<Poster>() {
            new Poster() { ID = 1, CategoryId = 1, ImgPath = "Some Path", Title = "Rosebud", Artist = "Orson Wells" },
            new Poster() { ID = 2, CategoryId = 1, ImgPath = "Some Path", Title = "A Series of Unfortunate Events", Artist = "Limony Snicket" },
            new Poster() { ID = 3, CategoryId = 2, ImgPath = "Some Path", Title = "Rebel Moon", Artist = "Gary Gerald" }
        };
    }

    private void SeedData()
    {
        var cats = Categories();
        var posters = Posters();

        using (var context = new PosterWebDbContext(_options))
        {
            var existingCategories = Task.Run(() => context.Categories.ToListAsync()).Result;
            if (existingCategories is null || existingCategories.Count == 0)
            {
                context.Categories.AddRange(cats);
                context.SaveChanges();
            }

            var existingPosters = Task.Run(() => context.Posters.ToListAsync()).Result;
            if (existingPosters is null || existingPosters.Count == 0)
            {
                context.Posters.AddRange(posters);
                context.SaveChanges();
            }
        }

    }

    [Fact]
    public async Task TestGetAllPosters()
    {
        using (var context = new PosterWebDbContext(_options))
        {
            //Arrange
            _repo = new PostersRepository(context);

            //Act
            var posters = await _repo.GetAllAsync();

            //Assert
            Assert.NotNull(posters);
            posters.ShouldNotBeNull();

            posters.Count.ShouldBe(3, "You didn't make the items or something...");
        }
    }

    // TODO: Make tests for GetAsync(Id) and GetAsync(), AddOrUpdateAsync, DeleteAsync(poster), DeleteAsync(id), ExistsAsync
    // for BOTH categories and posters
    [Fact]
    public async Task TestGetAsyncById()
    {
        using (var context = new PosterWebDbContext(_options))
        {
            //Arrange
            _repo = new PostersRepository(context);

            //Act
            var poster = await _repo.GetAsync(1);

            //Assert
            Assert.NotNull(poster);
            poster.ShouldNotBeNull();
            poster.Title.ShouldBe("Rosebud", "The retrieved poster title should be 'Rosebud'");
        }
    }
    
    


}