using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;
using Travel_Explorer.Application.Features.Blogs.Commands.CreateBlog;
using Travel_Explorer.Application.Features.Blogs.Commands.DeleteBlog;
using Travel_Explorer.Application.Features.Blogs.Commands.UpdateBlog;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Interfaces;

namespace UnitTest.TravelExplorer
{
    public class BlogCommandHandlerTests
    {
        [Fact]
        public async Task CreateBlogCommandHandler_Should_Return_Created_Blog()
        {
            // Arrange
            var blogRepoMock = new Mock<IGenericRepository<Blog>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var currentuserMock = new Mock<ICurrentUserService>();


            unitOfWorkMock.Setup(uow => uow.Repository<Blog>())
                .Returns(blogRepoMock.Object);

            currentuserMock.Setup(service => service.UserId).Returns(1);

            blogRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Blog>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mapperMock.Setup(m => m.Map<Blog>(It.IsAny<CreateBlogCommand>()))
                .Returns(new Blog());

            var handler = new CreateBlogCommandHandler(unitOfWorkMock.Object, mapperMock.Object, currentuserMock.Object);
            var command = new CreateBlogCommand("Test Blog", "This is a test blog.", "test-image.png", true,2);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            blogRepoMock.Verify(x => x.AddAsync(It.IsAny<Blog>()), Times.Once);
            unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handler_Should_DeleteBlog(int id)
        {
            var blogRepoMock = new Mock<IGenericRepository<Blog>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var currentuserMock = new Mock<ICurrentUserService>();

            unitOfWorkMock.Setup(uow => uow.Repository<Blog>())
                .Returns(blogRepoMock.Object);

            currentuserMock.Setup(service=>service.UserId).Returns(1);

            blogRepoMock.Setup(repo=>repo.GetAsync(id))
                .ReturnsAsync(new Blog { Id = id, AuthorId = 1 });

            unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var handler = new DeleteBlogCommandHandler(unitOfWorkMock.Object, currentuserMock.Object);
            var command = new DeleteBlogCommand(id);

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);

            blogRepoMock.Verify(x=>x.GetAsync(id), Times.Once);

            unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task Handle_Should_Update_Blog()
        {
            // Arrange
            var blogRepoMock = new Mock<IGenericRepository<Blog>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var currentUserMock = new Mock<ICurrentUserService>();
            var mapperMock = new Mock<IMapper>();

            unitOfWorkMock
                .Setup(uow => uow.Repository<Blog>())
                .Returns(blogRepoMock.Object);

            currentUserMock
                .Setup(x => x.UserId)
                .Returns(1);

            var existingBlog = new Blog
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Blog",
                Content = "This is a test blog.",
                ImageUrl = "test-image.png",
                IsPublished = true,
                CategoryId = 2,
                IsDeleted = false
            };

            blogRepoMock
                .Setup(repo => repo.GetAsync(1))
                .ReturnsAsync(existingBlog);

            blogRepoMock
                .Setup(repo => repo.Update(It.IsAny<Blog>()))
                .Verifiable();

            unitOfWorkMock
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mapperMock
                .Setup(m => m.Map(
                    It.IsAny<UpdateBlogCommand>(),
                    It.IsAny<Blog>()))
                .Callback<UpdateBlogCommand, Blog>((src, dest) =>
                {
                    dest.Title = src.Title;
                    dest.Content = src.Content;
                    dest.ImageUrl = src.ImageUrl;
                    dest.IsPublished = src.IsPublished;
                    dest.CategoryId = src.CategoryId;
                });

            mapperMock
                .Setup(m => m.Map<BlogDto>(It.IsAny<Blog>()))
                .Returns((Blog blog) => new BlogDto
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Content = blog.Content,
                    ImageUrl = blog.ImageUrl,
                    IsPublished = blog.IsPublished,
                });

            var handler = new UpdateBlogCommandHandler(
                unitOfWorkMock.Object,
                mapperMock.Object,
                currentUserMock.Object);

            var command = new UpdateBlogCommand(
                1,
                "Updated Blog",
                "This is an updated test blog.",
                "updated-image.png",
                false,
                3);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Blog", result.Title);

            blogRepoMock.Verify(
                x => x.GetAsync(1),
                Times.Once);

            blogRepoMock.Verify(
                x => x.Update(
                    It.Is<Blog>(b =>
                        b.Title == "Updated Blog" &&
                        b.Content == "This is an updated test blog." &&
                        b.ImageUrl == "updated-image.png" &&
                        b.IsPublished == false &&
                        b.CategoryId == 3)),
                Times.Once);

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            mapperMock.Verify(
                x => x.Map(
                    It.IsAny<UpdateBlogCommand>(),
                    It.IsAny<Blog>()),
                Times.Once);

            mapperMock.Verify(
                x => x.Map<BlogDto>(It.IsAny<Blog>()),
                Times.Once);
        }
    }
}
