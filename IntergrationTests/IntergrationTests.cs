using FluentAssertions;
using Flurl;
using Flurl.Http;
using FluentAssertions.Json;
using IntergrationTests.JsonObject;

namespace IntergrationTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task WeatherForcast()
        {
            var response = await Settings.GetSUT()
                    .AppendPathSegment("WeatherForecast")
                    .AllowAnyHttpStatus()
                    .GetAsync();

            int status = response.StatusCode;   
            status.Should().Be(200);
            var weatherResponse = response.GetJsonAsync<List<WeatherResponse>>().Result;
            weatherResponse.Should().NotBeNull();
            weatherResponse.Should().HaveCount(5);
            weatherResponse[0].Should().BeJsonSerializable();
            weatherResponse[0].date.Should().NotBeNull();
            weatherResponse[0].temperatureC.Should().BeOfType(typeof(int));
            weatherResponse[0].temperatureF.Should().BeOfType(typeof(int));
            weatherResponse[0].summary.Should().NotBeNull();

        }

        [Test]
        public async Task WeatherForcastDateCheck()
        {
            var response = await Settings.GetSUT()
                    .AppendPathSegment("WeatherForecast")
                    .AllowAnyHttpStatus()
                    .GetJsonAsync<List<WeatherResponse>>();
            
            //Set initial date counter
            int dateCounter = 0;
            foreach (var weatherResponse in response) {
                //DateTime dateTime = DateTime.Now.AddDays(dateCounter);
                //DateTime localDateTime = DateTime.SpecifyKind(DateTime.Now.AddDays(dateCounter), DateTimeKind.Local);
                DateOnly date = DateOnly.FromDateTime(DateTime.SpecifyKind(DateTime.Now.AddDays(dateCounter), DateTimeKind.Local));
                System.Console.WriteLine("Date: " + date);
                //string formatedDate = date.ToString("yyyy-MM-dd");
                weatherResponse.date.Should().Be(date.ToString());
                weatherResponse.temperatureC.Should().BeOfType(typeof(int));
                weatherResponse.temperatureF.Should().BeOfType(typeof(int));
                weatherResponse.summary.Should().NotBeNull();
                //Increment the date counter
                dateCounter++;
            }

        }
    }
}