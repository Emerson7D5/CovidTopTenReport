using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DomainLayer;
using EntitiesLayer;
using System.Collections.Generic;
using System.Linq;

namespace CovidTopTenReportUnitTest
{
    [TestClass]
    public class TestApiRequests
    {
        [TestMethod]
        public void TestApiRequestNotFound()
        {
            IEnumerable<totalRegions> callApi = new Provinces().GetTotals("2022-03-19");
            var result = (from ca in callApi select ca).ToList();
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void TestApiRequestFound()
        {
            IEnumerable<totalRegions> callApi = new Provinces().GetTotals("2022-03-11");
            var result = (from ca in callApi select ca).ToList();
            Assert.AreNotEqual(result.Count, 0);
        }

    }
}
