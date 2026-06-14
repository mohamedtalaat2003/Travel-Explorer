using AutoMapper;
using Moq;
using System.Reflection.Metadata;
using Travel_Explorer.Application.Features.Activities;
using Travel_Explorer.Application.Features.Activities.Queries.GetActivityById;
using Travel_Explorer.Application.Features.Activities.Queries.GetAllActivities;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Interfaces;

namespace UnitTest.TravelExplorer.ActivityTests
{
    public class ActivityCommandHandlerTestsBase
    {
        [Theory]
        [InlineData(2)]
        public async Task Handle_Shoud_GetActivityById(int id)
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var activityRepoMock = new Mock<IGenericRepository<Activity>>();
            var mapperMock = new Mock<IMapper>();

            unitOfWorkMock
                .Setup(uow => uow.Repository<Activity>())
                .Returns(activityRepoMock.Object);

            activityRepoMock.Setup(repo => repo.GenericEntitiesWithSpec(It.IsAny<ActivitySpecification>()))
                .ReturnsAsync(new Activity { Id = id });

            var handler = new GetActivityByIdQueryHandler(unitOfWorkMock.Object,mapperMock.Object);
            var query = new GetActivityByIdQuery(id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            activityRepoMock.Verify(x => x.GenericEntitiesWithSpec(It.IsAny<ActivitySpecification>()), Times.Once);
        }


        [Fact]
        public async Task Handle_Should_GetAllActivities()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var activitiesRepoMock = new Mock<IGenericRepository<Activity>>();
            var mapperMock = new Mock<IMapper>();

            unitOfWorkMock.Setup(uow => uow.Repository<Activity>()).Returns(activitiesRepoMock.Object);
            activitiesRepoMock.Setup(repo=>repo.ListSpecAsync(It.IsAny<ActivitySpecification>())).ReturnsAsync(new List<Activity>());
            activitiesRepoMock.Setup(repo => repo.CountAsync(It.IsAny<ActivitySpecification>())).ReturnsAsync(97);

            var handler = new GetAllActivitiesQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var command = new GetAllActivitiesQuery(1, 10);

            var result = await handler.Handle(command, CancellationToken.None);

            activitiesRepoMock.Verify(x => x.ListSpecAsync(It.IsAny<ActivitySpecification>()), Times.Once);
        }
    }
}