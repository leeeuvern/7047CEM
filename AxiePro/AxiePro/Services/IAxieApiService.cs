using AxiePro.Models.Battles;
using AxiePro.Models.Leaderboard;
using static AxiePro.Models.Battles.MmrRequest;

namespace AxiePro.Services
{
    public interface IAxieApiService
    {
        public Task<LeaderboardRequestProxy> GetLeaderboard(int offset=0, int limit = 100);
        public Task<BattlePvpRequest> GetBattlePvp(String RoninId);
   
        public Task<MmrPlayer> GetMmr(String RoninId);
        public  Task<ProxyMmr> GetMmrProxy(string RoninId);
    }

    public class AxieApiService : IAxieApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAxieDataFactory _axieDataFactory;
        public AxieApiService(IHttpClientFactory httpClientFactory, IAxieDataFactory axieDataFactory)
        {
            _httpClientFactory = httpClientFactory;
            _axieDataFactory = axieDataFactory;
        }
        public async Task<BattlePvpRequest> GetBattlePvp(string RoninId)
        {
            
          
            var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://tracking.skymavis.com/battle-history?type=pvp&player_id={RoninId}");
            if (response.IsSuccessStatusCode)
            {
                    try { 
                var model = await response.Content.ReadFromJsonAsync<BattlePvpRequest>();
                    //responseStream = responseStream.Replace("-", "");
                    //   var model = System.Text.Json.JsonSerializer.Deserialize<List<BattlePvpRequest>>(responseStream);
                    
                    await _axieDataFactory.ProcessPvpBattle(model, RoninId);

                return model;

                    }
                    catch(Exception ex) {
                    
                    return null;
                    }
            }
            else
                return null;

            
        }

 

        public async Task<LeaderboardRequestProxy> GetLeaderboard(int offset = 0, int limit = 100)
        {
        
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://game-api.skymavis.com/game-api/leaderboard?offset={offset}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var model = await response.Content.ReadFromJsonAsync<LeaderboardRequestProxy>();
                    //responseStream = responseStream.Replace("-", "");
                    //   var model = System.Text.Json.JsonSerializer.Deserialize<List<BattlePvpRequest>>(responseStream);


                    return model;
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
            else
                return null;
        }

        public async Task<MmrPlayer> GetMmr(string RoninId)
        {
           
  
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://game-api-pre.skymavis.com/v1/leaderboards?clientID={RoninId}&offset=0&limit=1");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var model = await response.Content.ReadFromJsonAsync<LeaderBoardRequest > ();
                    //responseStream = responseStream.Replace("-", "");
                    //   var model = System.Text.Json.JsonSerializer.Deserialize<List<BattlePvpRequest>>(responseStream);



                    var mmrPlayer = model._items.Where(a => a.clientID == RoninId).FirstOrDefault();
                    return mmrPlayer;
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
            else
                return null;

        }
        public async Task<ProxyMmr> GetMmrProxy(string RoninId)
        {
    
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            var response = await client.GetAsync($"https://game-api.axie.technology/api/v1/{RoninId}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var model = await response.Content.ReadFromJsonAsync<ProxyMmr>();
                    //responseStream = responseStream.Replace("-", "");
                    //   var model = System.Text.Json.JsonSerializer.Deserialize<List<BattlePvpRequest>>(responseStream);



                   
                    return model;
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
            else
                return null;

        }
    }
}
