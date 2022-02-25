using AxiePro.Models.Battles;
using AxiePro.Models.Leaderboard;
using AxieProBackground.Models.Payment;
using static AxiePro.Models.Battles.MmrRequest;

namespace AxiePro.Services
{
    public interface IAxieApiIsolatedService
    {
        public Task<LeaderboardRequestProxy> GetLeaderboard(int offset=0, int limit = 100);
   
        public Task<MmrPlayer> GetMmr(String RoninId);
        public  Task<ProxyMmr> GetMmrProxy(string RoninId);
        public Task<bool> GetPaymentTransaction();
    }

    public class AxieApiIsolatedService : IAxieApiIsolatedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
   
        public AxieApiIsolatedService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
         
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
        public async Task<bool> GetPaymentTransaction()
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
                        if (item.to == "0x666bcd8766bcce45e1b1611f5bf9b7d68d4437f2" && item.token_address == "0xa8754b9fa15fc18bb59458815510e40a12cd2014" && Int32.Parse(item.value) > 3) ;
                       //insert transaction into db

                    }

                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
            else
                return false;
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
        public async Task<List<AxieRequest>> GetAxies(List<int> axies)
        {

            var axiesString = "";


            foreach (var axie in axies)
            {
                axiesString = axiesString + axie.ToString() + ",";
            }

            var retry = 0;
            var notcompleted = true;
            while (retry < 5 && notcompleted)
            {
                var hasMissingAxie = false;
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://api.axie.technology/getaxies/{axiesString}");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        //   var model = await response.Content.ReadFromJsonAsync<List<AxieRequest>>();
                        //  //responseStream = responseStream.Replace("-", "");
                        var model = System.Text.Json.JsonSerializer.Deserialize<List<AxieRequest>>(responseBody);


                        if (model != null)
                        {
                            model.RemoveAt(model.Count - 1);

                            if (model.Any(a => a == null))
                                hasMissingAxie = true;
                            if (model.Any(a => a.id == null))
                                hasMissingAxie = true;
                            //var count = 0;
                            //foreach(var axie in model)
                            //{
                            //    count++;
                            //    if (count != 301) { 
                            //     if (axie == null)
                            //     hasMissingAxie = true;
                            //    else
                            //    if(axie.id == null)
                            //        hasMissingAxie = true;
                            //    }
                            //   // retry++;
                            //}
                            if (!hasMissingAxie || retry == 4)
                                return model;
                            else
                                retry++;

                        }
                        else
                            retry++;



                    }
                    catch (Exception ex)
                    {
                        retry++;
                        // return null;
                    }
                }
                else
                {
                    retry++;
                    //   model = null;
                    //  return null;
                }
            }
            //else
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
