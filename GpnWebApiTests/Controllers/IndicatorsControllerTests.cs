using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using GpnWebApi.Controllers;
using GpnWebApi.EF;
using GpnWebApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace GpnWebApi.Controllers.Tests
{
    [TestFixture()]
    public class IndicatorsControllerTests
    {
        private GpnContext _fakeGpnContext;
        private IndicatorsController _fakeIndicatorsController;
        private Indicator _fakeIndicator;

        [SetUp]
        public void Setup()
        {
            _fakeGpnContext = new GpnContext(new DbContextOptionsBuilder<GpnContext>().UseSqlServer(@"Data Source=MICROBE\SMPPDB1F2017;Initial Catalog=GPN;Persist Security Info=True;Password=sms_mcc@0802121;User ID=SmsMessageCenter").Options);


            _fakeIndicatorsController = new IndicatorsController(_fakeGpnContext);
            _fakeIndicator = new Indicator()
            {
                Id = Guid.NewGuid(),
                Title = "New title",
                MinValue = 0,
                MaxValue = 5000,
                Value = 1000
            };
        }

        [Test(), Order(1)]
        public async Task PostIndicatorTest()
        {
            //Arrange

            //Act

            var result = await _fakeIndicatorsController.PostIndicator(_fakeIndicator);

            //Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            //            Assert.Fail();

        }


        [Test(), Order(2)]
        public async Task GetIndicatorsTest()
        {
            //Arrange

            //Act
            var result = await _fakeIndicatorsController.GetIndicators();

            //Assert
            Assert.IsInstanceOf<List<Indicator>>(result.Value);
            Assert.Greater(((List<Indicator>)result.Value).Count, 0);
        }

        [Test(), Order(3)]
        public async Task PutIndicatorTest()
        {
            //Arrange
            _fakeIndicator = await _fakeGpnContext.Indicators.FirstOrDefaultAsync(x => x.Title == _fakeIndicator.Title);
            _fakeIndicator.Title = _fakeIndicator.Title + "-put";
            _fakeIndicator.MinValue = 5000;
            _fakeIndicator.MaxValue = 10000;
            _fakeIndicator.Value = 7000;


            //Act
            var result = await _fakeIndicatorsController.PutIndicator(_fakeIndicator.Id, _fakeIndicator);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test(), Order(4)]
        public async Task GetIndicatorTest()
        {
            //Arrange
            _fakeIndicator.Title = _fakeIndicator.Title + "-put";
            _fakeIndicator = await _fakeGpnContext.Indicators.FirstOrDefaultAsync(x => x.Title == _fakeIndicator.Title);
            //Act
            var result = await _fakeIndicatorsController.GetIndicator(_fakeIndicator.Id);

            //Assert
            Assert.IsInstanceOf<ActionResult<Indicator>>(result);
            Assert.IsInstanceOf<Indicator>(result.Value);
        }

        [Test(), Order(5)]
        public async Task DeleteIndicatorTest()
        {
            //Arrange
            _fakeIndicator.Title = _fakeIndicator.Title + "-put";
            _fakeIndicator = await _fakeGpnContext.Indicators.FirstOrDefaultAsync(x => x.Title == _fakeIndicator.Title);
            //Act
            var result = await _fakeIndicatorsController.DeleteIndicator(_fakeIndicator.Id);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
   }
}