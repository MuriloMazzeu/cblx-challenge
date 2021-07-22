using AutoMapper;
using CblxChallenge.Domain.Application;
using CblxChallenge.Domain.Entities;
using CblxChallenge.Domain.Infrastructure;
using CblxChallenge.Domain.ViewModels;
using FluentAssertions;
using Google.Cloud.Firestore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class FreighterCommandTests
    {

        [Fact]
        public async Task CheckinFail_IfAmountIsGreather()
        {
            var start = DateTime.UtcNow;
            var end = start.AddDays(1);

            var repo = new Mock<IFreighterRepository>();
            repo.Setup(x => x.GetTotalKilogramsInWeekAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(10);

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<FreighterTransportEntity>(It.IsAny<FreighterCheckinCommand>()))
                .Returns(new FreighterTransportEntity
                {
                    Id = "123",
                    Amount = 2000,
                    Mineral = "A",
                    StartAt = Timestamp.FromDateTime(start),
                    EndAt = Timestamp.FromDateTime(end)
                });

            var api = new Mock<ISmk186Service>();
            api.Setup(x => x.GetMineralsAsync(It.IsAny<string>()))
                .ReturnsAsync(new Smk186Result
                {
                    AMineralInTon = 1d,
                    BMineralInTon = 1d,
                    CMineralInTon = 1d,
                    DMineralInTon = 1d,
                });

            var service = new FreighterCommandService(repo.Object, mapper.Object, api.Object);
            var result = await service.ExecuteAsync(new FreighterCheckinCommand()
            {
                Id = "123",
                Amount = 10,
                Mineral = "A",
                RequestId = "123",
                StartAt = start,
                EndAt = end
            });

            result.Success.Should().BeFalse();
        }
    }
}
