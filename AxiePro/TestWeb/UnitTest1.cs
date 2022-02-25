using AxiePro;
using AxiePro.Models.Axies;
using AxiePro.Models.Battles;
using AxiePro.Models.Leaderboard;
using AxiePro.Services;
using AxieProBackground.Models.Payment;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static AxiePro.Models.Battles.MmrRequest;

namespace TestWeb
{
    public class UnitTest1
    {
        private readonly IUtilityService _utilityService;
      //  private readonly IAxieApiService _axieApiService;
   
        public UnitTest1()
        {
            _utilityService = new UtilityService();

        }
        [Fact]

        public void TestHashAxieBuild()


        {
        //    var hasValue = false;
            var hash = _utilityService.GetHashString("Plantback-sandalmouth-godahorn-beechtail-cattail");
          
           // if(!String.IsNullOrEmpty(hash))
          //      hasValue = true;

            Assert.Equal("DA6494FCD67827523F8D6796394A90224903998EB4868A336280FC1526BE0D21", hash);

        }
        [Fact]
        public async Task TestGetAxieApi()


        {
            // Arrange  
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mmrPlayer = new MmrPlayer
            {
                clientID = "ronin:666bcd8766bcce45e1b1611f5bf9b7d68d4437f2",
                elo = 1200,
                name = "UserName",
                rank = 1
            };
            var mmrPlayerList = new List<MmrPlayer>();
            mmrPlayerList.Add(mmrPlayer);
            var axieMockList = new List<AxieRequest>();
            var axieMock = new AxieRequest
            {
                id="1",
                breedCount = 0,
                


            };
            axieMockList.Add(axieMock);
            var mockAxieList = new List<int>();
            mockAxieList.Add(555555);
                


            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(axieMockList), Encoding.UTF8, "application/json")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
      
            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act  
           var _axieApiService = new AxieApiIsolatedService(httpClientFactory.Object);
            var result = await _axieApiService.GetAxies(mockAxieList);

            // Assert  
            Assert.NotNull(result);
            
        }
        [Fact]
        public async Task TestLeaderBoardApi()


        {
            // Arrange  
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mmrPlayer = new MmrPlayer
            {
                clientID = "ronin:666bcd8766bcce45e1b1611f5bf9b7d68d4437f2",
                elo = 1200,
                name = "UserName",
                rank = 1
            };
            var mmrPlayerList = new List<MmrPlayer>();
            mmrPlayerList.Add(mmrPlayer);
            var leaderBoardMock = new LeaderboardRequestProxy
            {
              items = new List<LeaderboardRequestProxy.LeaderboardPlayer>(),
              limit = 100,
              offset = 0,
              success = true
              
            };


            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(leaderBoardMock), Encoding.UTF8, "application/json")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act  
            var _axieApiService = new AxieApiIsolatedService(httpClientFactory.Object);
            var result = await _axieApiService.GetLeaderboard();

            // Assert  
            Assert.NotNull(result);

        }
        [Fact]
        public async Task TestMmrApi()


        {
            // Arrange  
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mmrPlayer = new MmrPlayer
            {
                clientID = "ronin:666bcd8766bcce45e1b1611f5bf9b7d68d4437f2",
                elo = 1200,
                name = "UserName",
                rank = 1
            };
            var mmrPlayerList = new List<MmrPlayer>();
            mmrPlayerList.Add(mmrPlayer);
            var leaderBoardMock = new LeaderBoardRequest
            {
                _etag = "test",
                _items = mmrPlayerList

            };


            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(leaderBoardMock), Encoding.UTF8, "application/json")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act  
            var _axieApiService = new AxieApiIsolatedService(httpClientFactory.Object);
            var result = await _axieApiService.GetMmr("ronin:666bcd8766bcce45e1b1611f5bf9b7d68d4437f2");

            // Assert  
            Assert.NotNull(result);

        }
        [Fact]
        public async Task TestPaymentSuccess()


        {   // Arrange  
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

          var  paymentListMock = new List<PaymentResult>();

            var paymentResultMock = new PaymentResult
            {
                token_address = "ronin:666bcd8766bcce45e1b1611f5bf9b7d68d4437f2",
                to = "ronin:666bcd8766bcce45e1b1611f5bf9b7d68d4437f2",
                value = "4",
                timestamp = DateTime.Now
            };
            paymentListMock.Add(paymentResultMock);
            var paymentRequestMock = new PaymentRequest
            {
                total = 100,
                results = paymentListMock

            };


            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(paymentRequestMock), Encoding.UTF8, "application/json")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act  
            var _axieApiService = new AxieApiIsolatedService(httpClientFactory.Object);
            var result = await _axieApiService.GetPaymentTransaction();

            // Assert  
            Assert.True(result);

        }
    }
}