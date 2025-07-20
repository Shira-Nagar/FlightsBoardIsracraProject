using FlightsDl.Interfaces;
using FlightsEntity;
using FlightsEntity.Dto;
using Microsoft.Extensions.Logging;
using Moq;
using FlightsBlClass = FlightsBl.Services.FlightsBl;

namespace FlightUnitTest
{
    public class FlightsBlTests
    {
        private readonly Mock<IflightsDl> _mockFlightsDl;

        
        private readonly Mock<ILogger<FlightsBlClass>> _mockLogger;
        private readonly FlightsBlClass _flightsBl;

        public FlightsBlTests()
        {
            _mockFlightsDl = new Mock<IflightsDl>();
            _mockLogger = new Mock<ILogger<FlightsBlClass>>();
            var mockMapper = new Mock<AutoMapper.IMapper>();
            _flightsBl = new FlightsBlClass(_mockFlightsDl.Object, _mockLogger.Object, mockMapper.Object);
        }

        // ... (שאר הטסטים לא דורשים שינוי)

        [Fact]
        public void GetAllFlights_ReturnsFlights()
        {
            var flights = new List<FlightEntity> { new FlightEntity { Id = 1, FlightNumber = "FN1" } };
            _mockFlightsDl.Setup(dl => dl.GetAllFlights()).Returns(flights);
            var result = _flightsBl.GetAllFlights();
            Assert.Equal(flights, result);
        }

        [Fact]
        public void GetFlightById_ReturnsFlightId()
        {
            var flightId = "1";
            _mockFlightsDl.Setup(dl => dl.GetFlightById(flightId)).Returns(flightId);
            var result = _flightsBl.GetFlightById(flightId);
            Assert.Equal(flightId, result);
        }

        [Fact]
        public void DeleteFlight_CallsDlDelete()
        {
            var flightId = "1";
            _flightsBl.DeleteFlight(flightId);
            _mockFlightsDl.Verify(dl => dl.DeleteFlight(flightId), Times.Once);
        }

        [Fact]
        public void ScheduleNewFlight_CallsDlSchedule()
        {
            var flight = new CreateFlightRequestDto { Id = 1, FlightNumber = "FN1" };
            var mappedFlight = new FlightEntity { Id = 1, FlightNumber = "FN1" };
            var mockMapper = new Mock<AutoMapper.IMapper>();
            mockMapper.Setup(m => m.Map<FlightEntity>(flight)).Returns(mappedFlight);
            var flightsBl = new FlightsBl.Services.FlightsBl(_mockFlightsDl.Object, _mockLogger.Object, mockMapper.Object);
            _mockFlightsDl.Setup(dl => dl.GetAllFlights()).Returns(new List<FlightEntity>());
            flightsBl.ScheduleNewFlight(flight);
            _mockFlightsDl.Verify(dl => dl.ScheduleNewFlight(It.IsAny<FlightEntity>()), Times.Once);
        }

        [Fact]
        public async Task SearchFlightsAsync_ReturnsFilteredFlights()
        {
            var now = DateTime.Now;
            var flights = new List<FlightEntity>
            {
                new FlightEntity { Id = 1, FlightNumber = "FN1", Destination = "NYC", DepartureTime = now.AddMinutes(40), Gate = "A1" },
                new FlightEntity { Id = 2, FlightNumber = "FN2", Destination = "LAX", DepartureTime = now.AddMinutes(10), Gate = "B2" }
            };
            _mockFlightsDl.Setup(dl => dl.GetAllAsync()).ReturnsAsync(flights);
            var result = await _flightsBl.SearchFlightsAsync(null, "NYC");
            Assert.Single(result);
            Assert.Equal("NYC", result.First().Destination);
        }

        [Fact]
        public void FlightValidator_Throws_When_Destination_Is_Missing()
        {
            var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "", Gate = "A1", DepartureTime = DateTime.Now.AddHours(1) };
            var existing = new List<FlightEntity>();
            Assert.Throws<ArgumentException>(() => Flights.BL.FlightValidator.Validate(flight, existing));
        }

        [Fact]
        public void FlightValidator_Throws_When_Gate_Is_Missing()
        {
            var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "", DepartureTime = DateTime.Now.AddHours(1) };
            var existing = new List<FlightEntity>();
            Assert.Throws<ArgumentException>(() => Flights.BL.FlightValidator.Validate(flight, existing));
        }

        [Fact]
        public void FlightValidator_Throws_When_DepartureTime_Is_Past()
        {
            var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "A1", DepartureTime = DateTime.Now.AddMinutes(-1) };
            var existing = new List<FlightEntity>();
            Assert.Throws<ArgumentException>(() => Flights.BL.FlightValidator.Validate(flight, existing));
        }

        [Fact]
        public void FlightValidator_DoesNotThrow_When_Valid()
        {
            var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "A1", DepartureTime = DateTime.Now.AddHours(1) };
            var existing = new List<FlightEntity> { new FlightEntity { FlightNumber = "999" } };
            Flights.BL.FlightValidator.Validate(flight, existing); // Should not throw
        }

        [Fact]
        public void FlightValidator_Throws_When_FlightNumber_Is_Whitespace()
        {
            var flight = new CreateFlightRequestDto { FlightNumber = "   ", Destination = "TLV", Gate = "A1", DepartureTime = DateTime.Now.AddHours(1) };
            var existing = new List<FlightEntity>();
            Assert.Throws<ArgumentException>(() => Flights.BL.FlightValidator.Validate(flight, existing));
        }

        [Fact]
        public void FlightValidator_Throws_When_Destination_Is_Whitespace()
        {
            var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "   ", Gate = "A1", DepartureTime = DateTime.Now.AddHours(1) };
            var existing = new List<FlightEntity>();
            Assert.Throws<ArgumentException>(() => Flights.BL.FlightValidator.Validate(flight, existing));
        }

        [Fact]
        public void FlightValidator_Throws_When_Gate_Is_Whitespace()
        {
            var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "   ", DepartureTime = DateTime.Now.AddHours(1) };
            var existing = new List<FlightEntity>();
            Assert.Throws<ArgumentException>(() => Flights.BL.FlightValidator.Validate(flight, existing));
        }

        [Fact]
        public void FlightValidator_Throws_When_DepartureTime_Is_TooFarInPast()
        {
            var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "A1", DepartureTime = DateTime.Now.AddYears(-10) };
            var existing = new List<FlightEntity>();
            Assert.Throws<ArgumentException>(() => Flights.BL.FlightValidator.Validate(flight, existing));
        }

        [Fact]
        public void FlightValidator_DoesNotThrow_When_DepartureTime_Is_FarInFuture()
        {
            var flight = new CreateFlightRequestDto { FlightNumber = "123", Destination = "TLV", Gate = "A1", DepartureTime = DateTime.Now.AddYears(10) };
            var existing = new List<FlightEntity>();
            Flights.BL.FlightValidator.Validate(flight, existing); // Should not throw
        }

        [Theory]
        [InlineData(40, 0, "Scheduled")]
        [InlineData(20, 0, "Boarding")]
        [InlineData(-10, 0, "Departed")]
        [InlineData(-70, 0, "Landed")]
        public void FlightStatusCalculator_Calculates_Correct_Status(int minutesFromNow, int nowOffset, string expectedStatus)
        {
            var now = DateTime.Now.AddMinutes(nowOffset);
            var departure = now.AddMinutes(minutesFromNow);
            var status = Flights.BL.FlightStatusCalculator.CalculateStatus(departure, now);
            Assert.Equal(expectedStatus, status.ToString());
        }
    }
} 