using AxiePro.Services;
using AxieProBackround.Services;

namespace AxieProBackground
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IAxieDataFactory  _axieDataFacotory;
        private readonly IAxieApiService _axieApiService;
        public Worker(ILogger<Worker> logger, IAxieDataFactory axieDataFactory, IAxieApiService axieApiService)
        {
            _logger = logger;
            _axieDataFacotory = axieDataFactory;
            _axieApiService = axieApiService;
        }

   
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               await _axieApiService.GetPaymentTransaction();
               await _axieApiService.GetLeaderboard();
                Thread.Sleep(12000);
                //3 second delay to abide by the terms and conditions
            }
        }
    }
}