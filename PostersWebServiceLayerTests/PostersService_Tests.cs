using Moq;
using PosterModels;
using PostersWebDbLayer;
using PostersWebServiceLayer;
using Shouldly;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace PostersWebServiceLayerTests
{
    public class TestPostersService
    {

        private readonly IPostersService _postersService;
        private Mock<IPostersRepository> _postersRepository;

        public TestPostersService()
        {
            CreateMocks();

            _postersService = new PostersServices(_postersRepository.Object);
        }

        private void CreateMocks()
        {
            _postersRepository = new Mock<IPostersRepository>();

            //this mocks an add, but there is no backing data so you can't really rely on it to be "there"
            _postersRepository.Setup(x => x.AddOrUpdateAsync(It.IsAny<Poster>())).ReturnsAsync(7);   


            //this mocks a get
            var posters = GetPosters().Result;

            _postersRepository.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(new Poster() { ID = 1, Title = "Test Poster Title", Artist="Test Poster Artist" });
            _postersRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(posters);

        }

        private async Task<List<Poster>> GetPosters()
        {
            return new List<Poster>()
            {
                new Poster() { ID = 1, Title = "Test Item"},
                new Poster() { ID = 2, Title = "Test Item 2" },
                new Poster() { ID = 3, Title = "Test Item 3" },
                new Poster() { ID = 4, Title = "Test Item 4" },
                new Poster() { ID = 5, Title = "Test Item 5" },
                new Poster() { ID = 6, Title = "Test Item 6" }
            };
        }

        [Fact]
        public async Task TestGetAllAsync()
        {
            var items = await _postersService.GetAllAsync();
            Assert.NotNull(items);
            Assert.Equal(6, items.Count);
        }

        [Fact]
        public async Task TestGetAsync()
        {
            var poster = await _postersService.GetAsync(235432);
            Assert.NotNull(poster);
            Assert.Equal("Test Poster Title", poster.Title);
            poster.ID.ShouldBe(1);
        }

        [Fact]
        public async Task TestAdd()
        {
            var poster = new Poster() { ID = 0, Title = "Test Item 7" };
            var result = await _postersService.AddOrUpdateAsync(poster);
            result.ShouldBe(7);
        }

    }


}
