using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Interfaces;

namespace UnitTest.TravelExplorer
{
    public class BlogQueryHandlerTests
    {
        [Fact]
        public async Task Handler_Should_GetAllBlogs()
        {
            var blogRepoMock = new Mock<IGenericRepository<Blog>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var currentUserMock = new Mock<ICurrentUserService>();
        }
    }
}
