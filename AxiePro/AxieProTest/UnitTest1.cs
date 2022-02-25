using AxiePro.Models.Reports;
using AxiePro.Pages.Build;
using AxiePro.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AxieProTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Mock<IBattlesReportsService> _battlesReportsService;
        private readonly Mock<IAxieDataFactory> _axieDataFactory;
        private readonly Mock<IAxieApiService> _axieApiService;
        private List<BattleHistoryReport> _battlesReports;
        public UnitTest1()
        {
            _battlesReportsService = new Mock<IBattlesReportsService>();
            _battlesReports = new List<BattleHistoryReport>();
            _battlesReports.Add(new BattleHistoryReport());
        }
       
        [Fact]

        public async Task TestAgeofRecords()
        {

            //arrange
         
             _battlesReportsService.Setup(a => a.GetBattleHistoryByBuild("25016A041AE31A90F10374949EA012D3BA5C509276E7A729CB1D683C0F93B577BC0BBBD7DC9ED87993C23B12D09D8E90F4B8814AF69AFA0304617D074C29CE61E0DDFCBE6CCA935E2904944B5523D8CF63B047CAF5C0439D1A5CC0033D53981B")
                ).ReturnsAsync(_battlesReports);

            //Arrest
            var listQuery = (_battlesReportsService.Object);
            var list = await listQuery.GetBattleHistoryByBuild("25016A041AE31A90F10374949EA012D3BA5C509276E7A729CB1D683C0F93B577BC0BBBD7DC9ED87993C23B12D09D8E90F4B8814AF69AFA0304617D074C29CE61E0DDFCBE6CCA935E2904944B5523D8CF63B047CAF5C0439D1A5CC0033D53981B");
            Assert.IsTrue(list.Count > 1);
        }
    }
}