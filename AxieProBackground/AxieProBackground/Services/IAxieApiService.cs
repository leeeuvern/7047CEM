

using AxieProBackground.Models.Battles;
using AxieProBackground.Models.Leaderboard;
using AxieProBackground.Models.Payment;
using AxieProBackround.Services;
using System.Net.Http.Json;

namespace AxiePro.Services
{
    public interface IAxieApiService
    {
        public Task<LeaderboardRequestProxy> GetLeaderboard(int offset = 0, int limit = 100);
        public Task<BattlePvpRequest> GetBattlePvp(String RoninId, bool leaderboard);
        public Task GetPaymentTransaction();
        //public Task<MmrPlayer> GetMmr(String RoninId);
        //public Task<ProxyMmr> GetMmrProxy(string RoninId);
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
        public async Task<BattlePvpRequest> GetBattlePvp(string RoninId, bool leaderboard)
        {


            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://tracking.skymavis.com/battle-history?type=pvp&player_id={RoninId}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var model = await response.Content.ReadFromJsonAsync<BattlePvpRequest>();
                    //responseStream = responseStream.Replace("-", "");
                    //   var model = System.Text.Json.JsonSerializer.Deserialize<List<BattlePvpRequest>>(responseStream);

                    await _axieDataFactory.ProcessPvpBattle(model, RoninId, leaderboard);

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

                    foreach(var item in model.items)
                    {
                       await GetBattlePvp(item.client_id, true);


                    }

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

        public async Task GetPaymentTransaction()
        {
            var offset = 0;
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://explorer.roninchain.com/api/tokentxs?addr=0x666bcd8766bcce45e1b1611f5bf9b7d68d4437f2&from={offset}&size=100&token=ERC20");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var model = await response.Content.ReadFromJsonAsync<PaymentRequest>();
                    //responseStream = responseStream.Replace("-", "");
                    //   var model = System.Text.Json.JsonSerializer.Deserialize<List<BattlePvpRequest>>(responseStream);

                    foreach (var item in model.results)
                    {
                        if(item.to == "0x666bcd8766bcce45e1b1611f5bf9b7d68d4437f2" && item.token_address == "0xa8754b9fa15fc18bb59458815510e40a12cd2014" && Int32.Parse(item.value) > 3)
                        await _axieDataFactory.AddPaymentTransaction(item, (offset + model.results.Count));


                    }

                    return ;
                }
                catch (Exception ex)
                {

                    return ;
                }
            }
            else
                return ;
        }

        //public async Task<MmrPlayer> GetMmr(string RoninId)
        //{


        //    var client = _httpClientFactory.CreateClient();
        //    var response = await client.GetAsync($"https://game-api-pre.skymavis.com/v1/leaderboards?clientID={RoninId}&offset=0&limit=1");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        try
        //        {
        //            var model = await response.Content.ReadFromJsonAsync<LeaderBoardRequest>();
        //            //responseStream = responseStream.Replace("-", "");
        //            //   var model = System.Text.Json.JsonSerializer.Deserialize<List<BattlePvpRequest>>(responseStream);



        //            var mmrPlayer = model._items.Where(a => a.clientID == RoninId).FirstOrDefault();
        //            return mmrPlayer;
        //        }
        //        catch (Exception ex)
        //        {

        //            return null;
        //        }
        //    }
        //    else
        //        return null;

        //}
        //public async Task<ProxyMmr> GetMmrProxy(string RoninId)
        //{

        //    var client = _httpClientFactory.CreateClient();
        //    var response = await client.GetAsync($"https://game-api.axie.technology/api/v1/{RoninId}");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        try
        //        {
        //            var model = await response.Content.ReadFromJsonAsync<ProxyMmr>();
        //            //responseStream = responseStream.Replace("-", "");
        //            //   var model = System.Text.Json.JsonSerializer.Deserialize<List<BattlePvpRequest>>(responseStream);




        //            return model;
        //        }
        //        catch (Exception ex)
        //        {

        //            return null;
        //        }
        //    }
        //    else
        //        return null;

        //}
    }
}
