using System.Net;
using AvaStorage.Application.UseCases.PutAvatar;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MyLab.ApiClient.Test;
using Xunit.Abstractions;

namespace AvaStorage.Tests
{
    public class AdminPutApiBehavior : IClassFixture<TestApiFixture<Program, IAdminContractV1>>
    {
        private readonly TestApiFixture<Program, IAdminContractV1> _fxt;

        public AdminPutApiBehavior(TestApiFixture<Program, IAdminContractV1> fxt, ITestOutputHelper output)
        {
            fxt.Output = output;
            _fxt = fxt;
        }

        [Fact]
        public async Task ShouldPutPicture()
        {
            //Arrange
            var putHandlerMock = new Mock<IRequestHandler<PutAvatarCommand>>();

            var outHandlerDescriptor = ServiceDescriptor.Transient(typeof(IRequestHandler<PutAvatarCommand>), s => putHandlerMock.Object);

            var proxyAsset = _fxt.StartWithProxy
                (
                    s => s
                        .Replace(outHandlerDescriptor)
                        .AddSingleton(TestTools.DefaultRepoMock.Object),
                    TestTools.SetAdminPort);
            var client = proxyAsset.ApiClient;

            //Act
            var response = await client.PutAsync("foo", TestTools.PictureBin);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            putHandlerMock.Verify(h => h.Handle
                (
                    It.Is<PutAvatarCommand>(c => 
                            c.Id == "foo" &&
                            c.Picture[0] == 1 &&
                            c.Picture[1] == 2 &&
                            c.Picture[2] == 3
                        ),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
            putHandlerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetInvalidParameters))]
        public async Task ShouldNotReturn200WhenPutBadRequest(string id, byte[] picBin)
        {
            //Arrange

            var proxyAsset = _fxt.StartWithProxy
                (
                    s => s.AddSingleton(TestTools.DefaultRepoMock.Object),
                    c =>
                    {
                        c.BaseAddress = new UriBuilder(c.BaseAddress!)
                        {
                            Port = ListenConstants.AdminPort
                        }.Uri;
                    });
            var client = proxyAsset.ApiClient;

            //Act
            var response = await client.PutAsync(id, picBin);

            //Assert
            Assert.True((int)response.StatusCode >= 400 && (int)response.StatusCode < 500);
        }

        public static object[][] GetInvalidParameters()
        {
            var validPicBin = new byte[] { 1, 2, 3 };

            return new object[][]
            {
                new object[] { "foo", Array.Empty<byte>() },
                new object[] { null, validPicBin },
            };
        }
    }
}