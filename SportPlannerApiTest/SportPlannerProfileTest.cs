using AutoFixture;
using AutoMapper;
using AutoMapper.Configuration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelsCore;
using SportPlannerApi.DataLayer.Profiles;
using SportPlannerIngestion.DataLayer.Models;

namespace SportPlannerApiTest
{
    [TestClass]
    public class SportPlannerProfileTest
    {
        private Fixture _fixture = new Fixture();
        private MapperConfiguration _mapperConf;
        private IMapper _mapper;

        public SportPlannerProfileTest()
        {
            var confExpression = new MapperConfigurationExpression();
            confExpression.AddProfile<SportPlannerProfile>();
            _mapperConf = new MapperConfiguration(confExpression);
            _mapper = _mapperConf.CreateMapper();
        }

        [TestMethod]
        public void AssertConfigurationIsValid()
        {
            _mapperConf.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void EventDto_to_Event()
        {
            var eventDto = _fixture.Create<EventDto>();
            var @event = new Event();

            _mapper.Map(eventDto, @event);

            foreach (var eventUser in @event.Users)
            {
                eventUser.EventId.Should().Be(eventDto.Id);
            }
        }
    }
}
