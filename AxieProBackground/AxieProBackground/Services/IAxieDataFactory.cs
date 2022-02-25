using AxiePro.Models.Payment;
using AxieProBackground;
using AxieProBackground.Models.Axies;
using AxieProBackground.Models.Battles;
using AxieProBackground.Models.Leaderboard;
using AxieProBackground.Models.Logs;
using AxieProBackground.Models.Payment;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace AxieProBackround.Services
{
    public interface IAxieDataFactory
    {
        public Task ProcessPvpBattle(BattlePvpRequest battlePvpRequest, string roninUid, bool leaderboard = false);
        public Task<int> GetTransactionOffset();
        public Task AddPaymentTransaction(PaymentResult payment, int offset);
        //   public Task AddAllAxies();
    }

    public class AxieDataFactory : IAxieDataFactory
    {
     //  private readonly DataContext _dataContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GraphQL.Client.Http.GraphQLHttpClient _graphqlClient =
     new GraphQLHttpClient("https://graphql-gateway.axieinfinity.com/graphql", new NewtonsoftJsonSerializer());
        private readonly IServiceScopeFactory _scopeFactory;
        public AxieDataFactory( IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory)
        {
          //  _dataContext = dataContext;
            _httpClientFactory = httpClientFactory;
            _scopeFactory = scopeFactory;
        }
        public async Task ProcessPvpBattle(BattlePvpRequest battlePvpRequest, string roninUid, bool leaderboard = false)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();



                var battlesString = battlePvpRequest.battles.Select(a => a.battle_uuid).ToList();

                var nonExistentBattles = await _dataContext.BattleHistory.Where(a => battlesString.Contains(a.BattleUid)).Select(A => A.BattleUid).ToListAsync();
                var addTeamHashes = new List<string>();

                // var first = false;
                foreach (var battle in battlePvpRequest.battles)
                {

                    var hasBattle = await _dataContext.BattleHistory.Where(a => a.BattleUid == battle.battle_uuid).FirstOrDefaultAsync();
                    if (battle.winner == "draw")
                    {
                        var winnerTeamHash = GetHashString(battle.first_team_fighters[0] + battle.first_team_fighters[1] + battle.first_team_fighters[2] + "");
                        var loserTeamHash = GetHashString(battle.second_team_fighters[0] + battle.second_team_fighters[1] + battle.second_team_fighters[2] + "");
                        var winnerTeamList = battle.first_team_fighters;
                        var loserTeamList = battle.second_team_fighters;
                        if (!nonExistentBattles.Contains(battle.battle_uuid))
                        {
                            var newBattleHistory = new BattleHistory
                            {
                                BattleUid = battle.battle_uuid,
                                WinnerUserUid = battle.first_client_id,
                                LoserUserUid = battle.second_client_id,
                                StartDateTime = battle.game_started,
                                EndDateTime = battle.game_ended,
                                WinnerTeamHash = winnerTeamHash,
                                LoserTeamHash = loserTeamHash,
                                Draw = true,
                                LeaderboardGame = leaderboard



                            };
                            var winnerTeam = new AxieTeam
                            {
                                TeamHash = winnerTeamHash,
                                FirstFighter = winnerTeamList[0],
                                SecondFighter = winnerTeamList[1],
                                ThirdFighter = winnerTeamList[2]
                            };

                            var loserTeam = new AxieTeam
                            {
                                TeamHash = loserTeamHash,
                                FirstFighter = loserTeamList[0],
                                SecondFighter = loserTeamList[1],
                                ThirdFighter = loserTeamList[2]
                            };

                            var hasTeams = await _dataContext.AxieTeam.Where(a => a.TeamHash == winnerTeamHash || a.TeamHash == loserTeamHash).ToListAsync();
                            if (!hasTeams.Any())
                            {
                                if (!addTeamHashes.Contains(winnerTeamHash))
                                {
                                    var winnerBuildHash = await GetBuildHash(winnerTeamList, _dataContext);
                                    winnerTeam.BuildHash = winnerBuildHash.BuildHash;
                                    winnerTeam.ClassHash = winnerBuildHash.ClassHash;
                                    await _dataContext.AddAsync(winnerTeam);
                                    addTeamHashes.Add(winnerTeamHash);
                                }
                                if (!addTeamHashes.Contains(loserTeamHash))
                                {
                                    var loserBuildHash = await GetBuildHash(loserTeamList, _dataContext);
                                    loserTeam.BuildHash = loserBuildHash.BuildHash;
                                    loserTeam.ClassHash = loserBuildHash.ClassHash;

                                    await _dataContext.AddAsync(loserTeam);
                                    addTeamHashes.Add(loserTeamHash);
                                }

                            }
                            else
                            {

                                if (!hasTeams.Where(a => a.TeamHash == winnerTeamHash).Any() && !addTeamHashes.Contains(winnerTeamHash))
                                {
                                    var winnerBuildHash = await GetBuildHash(winnerTeamList, _dataContext);
                                    winnerTeam.BuildHash = winnerBuildHash.BuildHash;
                                    winnerTeam.ClassHash = winnerBuildHash.ClassHash;
                                    await _dataContext.AddAsync(winnerTeam);
                                    addTeamHashes.Add(winnerTeamHash);
                                }
                                if (!hasTeams.Where(a => a.TeamHash == loserTeamHash).Any() && !addTeamHashes.Contains(loserTeamHash))
                                {
                                    var loserBuildHash = await GetBuildHash(loserTeamList, _dataContext);
                                    loserTeam.BuildHash = loserBuildHash.BuildHash;
                                    loserTeam.ClassHash = loserBuildHash.ClassHash;

                                    await _dataContext.AddAsync(loserTeam);
                                    addTeamHashes.Add(loserTeamHash);
                                }
                            }
                            await _dataContext.AddAsync(newBattleHistory);
                        }
                    }
                    else
                    {


                        var winnerTeamHash = battle.winner == battle.second_client_id ? GetHashString(battle.second_team_fighters[0] + battle.second_team_fighters[1] + battle.second_team_fighters[2] + "") : GetHashString(battle.first_team_fighters[0] + battle.first_team_fighters[1] + battle.first_team_fighters[2] + "");
                        var loserTeamHash = battle.winner == battle.first_client_id ? GetHashString(battle.second_team_fighters[0] + battle.second_team_fighters[1] + battle.second_team_fighters[2] + "") : GetHashString(battle.first_team_fighters[0] + battle.first_team_fighters[1] + battle.first_team_fighters[2] + "");
                        var winnerTeamList = battle.winner == battle.first_client_id ? battle.first_team_fighters : battle.second_team_fighters;
                        var loserTeamList = battle.winner == battle.second_client_id ? battle.first_team_fighters : battle.second_team_fighters;

                        if (!nonExistentBattles.Contains(battle.battle_uuid))
                        {
                            var newBattleHistory = new BattleHistory
                            {
                                BattleUid = battle.battle_uuid,
                                WinnerUserUid = battle.winner,
                                LoserUserUid = battle.winner == battle.first_client_id ? battle.second_client_id : battle.first_client_id,
                                StartDateTime = battle.game_started,
                                EndDateTime = battle.game_ended,
                                WinnerTeamHash = winnerTeamHash,
                                LoserTeamHash = loserTeamHash,
                                Draw = false,
                                LeaderboardGame = leaderboard



                            };
                            var winnerTeam = new AxieTeam
                            {
                                TeamHash = winnerTeamHash,
                                FirstFighter = winnerTeamList[0],
                                SecondFighter = winnerTeamList[1],
                                ThirdFighter = winnerTeamList[2]
                            };

                            var loserTeam = new AxieTeam
                            {
                                TeamHash = loserTeamHash,
                                FirstFighter = loserTeamList[0],
                                SecondFighter = loserTeamList[1],
                                ThirdFighter = loserTeamList[2]
                            };

                            var hasTeams = await _dataContext.AxieTeam.Where(a => a.TeamHash == winnerTeamHash || a.TeamHash == loserTeamHash).ToListAsync();
                            if (!hasTeams.Any())
                            {
                                if (!addTeamHashes.Contains(winnerTeamHash))
                                {
                                    var winnerBuildHash = await GetBuildHash(winnerTeamList, _dataContext);
                                    winnerTeam.BuildHash = winnerBuildHash.BuildHash;
                                    winnerTeam.ClassHash = winnerBuildHash.ClassHash;
                                    await _dataContext.AddAsync(winnerTeam);
                                    addTeamHashes.Add(winnerTeamHash);
                                }
                                if (!addTeamHashes.Contains(loserTeamHash))
                                {
                                    var loserBuildHash = await GetBuildHash(loserTeamList, _dataContext);
                                    loserTeam.BuildHash = loserBuildHash.BuildHash;
                                    loserTeam.ClassHash = loserBuildHash.ClassHash;

                                    await _dataContext.AddAsync(loserTeam);
                                    addTeamHashes.Add(loserTeamHash);
                                }

                            }
                            else
                            {

                                if (!hasTeams.Where(a => a.TeamHash == winnerTeamHash).Any() && !addTeamHashes.Contains(winnerTeamHash))
                                {
                                    var winnerBuildHash = await GetBuildHash(winnerTeamList, _dataContext);
                                    winnerTeam.BuildHash = winnerBuildHash.BuildHash;
                                    winnerTeam.ClassHash = winnerBuildHash.ClassHash;
                                    await _dataContext.AddAsync(winnerTeam);
                                    addTeamHashes.Add(winnerTeamHash);
                                }
                                if (!hasTeams.Where(a => a.TeamHash == loserTeamHash).Any() && !addTeamHashes.Contains(loserTeamHash))
                                {
                                    var loserBuildHash = await GetBuildHash(loserTeamList, _dataContext);
                                    loserTeam.BuildHash = loserBuildHash.BuildHash;
                                    loserTeam.ClassHash = loserBuildHash.ClassHash;

                                    await _dataContext.AddAsync(loserTeam);
                                    addTeamHashes.Add(loserTeamHash);
                                }
                            }
                            await _dataContext.AddAsync(newBattleHistory);
                        }

                    }



                    //if ((battle.first_client_id == roninUid && !first) || (battle.first_client_id != roninUid))
                    //    await ProcessFighterList(battle.first_team_fighters);
                    //if ((battle.second_client_id == roninUid && !first) || (battle.second_client_id != roninUid))
                    //    await ProcessFighterList(battle.second_team_fighters);
                    //// first = true;

                }
                await _dataContext.SaveChangesAsync();
                // return null;
            }
        }
        public async Task<HashCombination> GetBuildHash(List<int> fighters, DataContext _dataContext)
        {

                var axies = new List<Axie>();
            axies = await _dataContext.Axie.Where(a => fighters.Contains(a.AxieId)).OrderBy(a => a.Hash).ToListAsync();

            if (axies == null)
            {
                await ProcessFighterList(fighters, _dataContext);
            }
            else
            {
                var existantFighters = axies.Select(A => A.AxieId).ToList();
                var nonExistantFighters = new List<int>();
                foreach (var fighter in fighters)
                {
                    if (!existantFighters.Contains(fighter))
                        nonExistantFighters.Add(fighter);


                }
                if (nonExistantFighters.Any())
                    await ProcessFighterList(nonExistantFighters, _dataContext);
                axies = await _dataContext.Axie.Where(a => fighters.Contains(a.AxieId)).OrderBy(a => a.Hash).ToListAsync();
            }

            var hashCombination = "";
            var classCombination = new List<string>();
            var classList = new List<string>();
            foreach (var axie in axies)
            {
                var temp = "";
                hashCombination = hashCombination + axie.Hash;
                classList.Add(axie.Class);
            };
            classList = classList.OrderBy(a => a).ToList();
            var buildHash = GetHashString(hashCombination);
            var classHash = GetHashString(classList[0] + classList[1] + classList[2]);
            var hashes = new HashCombination
            {
                BuildHash = hashCombination,
                ClassHash = classHash
            };

            return hashes;

        }
        
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        public async Task ProcessFighterList(List<int> fighters ,DataContext _dataContext)
        {
            var hasdata = false;
            var model = new List<AxieRequest>();
            while (!hasdata)
            {
                model = await GetAxies(fighters);
                if (model != null)
                    hasdata = true;
            }
            var missingList = new List<int>();
            for (int x = 0; x < fighters.Count; x++)
            {

                if (model[x] == null)
                {
                    await _dataContext.MissingAxie.AddAsync(new MissingAxie
                    {
                        AxieId = fighters[x],
                        Retrieved = false
                    });
                    await _dataContext.SaveChangesAsync();
                    missingList.Add(fighters[x]);
                }
                else if (model[x].id == null)
                {
                    await _dataContext.MissingAxie.AddAsync(new MissingAxie
                    {
                        AxieId = fighters[x],
                        Retrieved = false
                    });
                    await _dataContext.SaveChangesAsync();
                    missingList.Add(fighters[x]);
                }
            }
            try
            {
                var nonExistentFighters = await _dataContext.Axie.Where(a => fighters.Contains(a.AxieId)).Select(A => A.AxieId).ToListAsync();
                //SHA256 sha256Hash = SHA256.Create();
                foreach (var fighter in fighters)
                {

                    if (!missingList.Contains(fighter))
                    {
                        if (!nonExistentFighters.Contains(fighter))
                        {
                            var newAxie = model.Where(a => a.id == fighter.ToString()).FirstOrDefault();

                            if (newAxie != null)
                                await AddAxie(newAxie, _dataContext);





                        }


                    }

                }
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {


            }

        }

        //public async Task AddAllAxies()
        //{
        //    int increment = 500;
        //    for (int x = 204356; x < 11000000; x += increment)
        //    {

        //        var fighters = new List<int>();

        //        for (int y = x; y < x + increment; y++)
        //        {
        //            fighters.Add(y);
        //        }
        //        await ProcessFighterList(fighters);
        //        await Task.Delay(2000);
        //    }


        //}
        public async Task<Axie> AddAxie(AxieRequest axieToAdd, DataContext _dataContext)
        {
            try
            {
                var newAxie = new Axie
                {
                    AxieId = Int32.Parse(axieToAdd.id),
                    Class = axieToAdd.Class,
                    Eyes = axieToAdd.parts[0].id,
                    Ears = axieToAdd.parts[1].id,
                    Back = axieToAdd.parts[2].id,

                    Mouth = axieToAdd.parts[3].id,
                    Horn = axieToAdd.parts[4].id,
                    Tail = axieToAdd.parts[5].id,
                    Hash = GetHashString(axieToAdd.Class + axieToAdd.parts[2].id + axieToAdd.parts[3].id + axieToAdd.parts[4].id + axieToAdd.parts[5].id)


                };
                await _dataContext.Axie.AddAsync(newAxie);
                //  await _dataContext.SaveChangesAsync();
                return newAxie;
            }
            catch (Exception ex)
            {
                return null;

            }

        }
        public async Task<Axie> AddAxie(Result axieToAdd, DataContext _dataContext)
        {
            try
            {
                var newAxie = new Axie
                {
                    AxieId = Int32.Parse(axieToAdd.id),
                    Class = axieToAdd.Class,
                    Eyes = axieToAdd.parts[0].id,
                    Ears = axieToAdd.parts[1].id,
                    Back = axieToAdd.parts[2].id,

                    Mouth = axieToAdd.parts[3].id,
                    Horn = axieToAdd.parts[4].id,
                    Tail = axieToAdd.parts[5].id,
                    Hash = GetHashString(axieToAdd.Class + axieToAdd.parts[2].id + axieToAdd.parts[3].id + axieToAdd.parts[4].id + axieToAdd.parts[5].id)


                };
                await _dataContext.Axie.AddAsync(newAxie);
                await _dataContext.SaveChangesAsync();
                return newAxie;
            }
            catch (Exception ex)
            {
                return null;

            }

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

        public async Task<int> GetTransactionOffset()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                var offset = await _dataContext.PaymentTransaction.OrderByDescending(a=>a.Id).Select(a=>a.offset).FirstOrDefaultAsync();
                return offset;
            }
        }
        public async Task AddPaymentTransaction(PaymentResult payment, int offset)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
            
                var _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var exists = await _dataContext.PaymentTransaction.Where(a => a.PaymentTransactionUid == payment.tx_hash).FirstOrDefaultAsync();
               if(exists == null) { 
                var paymentTransaction = new PaymentTransaction
                {
                    PayeeAddressUid = payment.from,
                    PaymentDateTime = payment.timestamp,
                    PaymentTransactionUid = payment.tx_hash,
                    offset = offset,
                    Amount = Int32.Parse(payment.value)
                };
                await _dataContext.AddAsync(paymentTransaction);
                await _dataContext.SaveChangesAsync();
                  await  SubscriptionIntialiser(payment);
                }
            }
        }
        public async Task SubscriptionIntialiser(PaymentResult payment)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var hasSuscribed = await _dataContext.Membership.Where(a => a.MemberUid == payment.from && a.MembershipEndDate >= DateTime.Now).FirstOrDefaultAsync();
                if(hasSuscribed == null)
                {
                    var membership = new Membership
                    {
                        MembershipStartDate = DateTime.Now,
                        MemberUid = payment.from,
                        MembershipEndDate = DateTime.Now.AddDays(31),
                        TransactionUid = payment.tx_hash
                    };

                    await _dataContext.AddAsync(membership);
                    await _dataContext.SaveChangesAsync();
                }
            }
        }
    }

}
