using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPEFN.Data.Entities;
using RPEFN.WebService.Controllers;
using RPEFN.WebService.Dtos;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace RPEFN.UnitTests.Controllers
{
    [TestClass]
    public class DrugControllerTest: BaseTest
    {
        public DrugControllerTest()
        {
            base.MockControllerProperties(new DrugController(UnitOfWork), "Drug" );
        }

        private DrugController DrugController => Controller as DrugController;

        [TestMethod]
        public async Task GetDrugAsync_ShouldReturnCorrectDrug()
        {
            var result = await DrugController.GetDrugAsync(1) as OkNegotiatedContentResult<DrugDto>;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public async Task GetDrugAsync_ShouldNotFindDrug()
        {
            var result = await DrugController.GetDrugAsync(10000);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetDrugAsync_ShouldReturnCorrectNumberOfDrugs()
        {
            var result = await DrugController.GetDrugsAsync() as OkNegotiatedContentResult<IEnumerable<DrugDto>>;
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Content.ToList().Count>0);
            Assert.AreEqual(result.Content.ToList().Count, base.UnitOfWork.Drugs.Get().ToList().Count);
            Assert.AreEqual(result.Content.ToList()[0].BrandName, base.UnitOfWork.Drugs.Get(1).BrandName);
        }


        [TestMethod]
        public async Task CreateDrugAsync_ShouldReturnBadRequest()
        {
            int count = base.UnitOfWork.Drugs.Get().Count();

            DrugDto newDrug = new DrugDto();

            DrugController.Validate(newDrug);

            var result = await DrugController.CreateDrugAsync(newDrug);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }
        
        [TestMethod]
        public async Task CreateDrugAsync_ShouldAddNewDrug()
        {
            int count = base.UnitOfWork.Drugs.Get().Count();
            
            DrugDto newDrug = new DrugDto() { BrandName = "Advil", GenericName = "pain killer", NdcId = "1597845841", Price = 10, Strength = "500 mg"};

            DrugController.Validate(newDrug);

            var result = await DrugController.CreateDrugAsync(newDrug) as CreatedNegotiatedContentResult<DrugDto>;
            
            int newCount = base.UnitOfWork.Drugs.Get().Count();
            
            Assert.IsNotNull(result);
            Assert.AreEqual(count + 1 , newCount);
        }

        [TestMethod]
        public async Task UpdateDrugAsync_ShouldReturnBadRequest()
        {
            Drug drug = UnitOfWork.Drugs.Get(1);
            DrugDto updatedDrug = new DrugDto
            {
                BrandName = string.Empty,
                GenericName = string.Empty,
                Id = drug.Id,
                NdcId = drug.NdcId,
                Price = drug.Price,
                Strength = drug.Strength
            };

            //Validation happens when the posted data is bound to the view model. The view model is then passed into the controller. You are skipping part 1 and passing a view model straight into a controller.

            DrugController.Validate(updatedDrug);

            var result = await DrugController.UpdateDrugAsync(updatedDrug);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }


        [TestMethod]
        public async Task UpdateDrugAsync_ShouldReturnDrugNotFound()
        {
            Drug drug = UnitOfWork.Drugs.Get(1);
            DrugDto updatedDrug = new DrugDto
            {
                BrandName = drug.BrandName,
                GenericName = drug.GenericName,
                Id = -1,
                NdcId = drug.NdcId,
                Price = drug.Price,
                Strength = drug.Strength
            };

            DrugController.Validate(updatedDrug);
            var result = await DrugController.UpdateDrugAsync(updatedDrug);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public async Task UpdateDrugAsync_ShouldReturnUpdatedDrug()
        {
            Drug drug = UnitOfWork.Drugs.Get(1);
            DrugDto updatedDrug = new DrugDto
            {
                BrandName = "Updated brand name",
                GenericName = "Updated generic name",
                Id = drug.Id,
                NdcId = drug.NdcId,
                Price = drug.Price,
                Strength = drug.Strength
            };

            DrugController.Validate(updatedDrug);
            var result = await DrugController.UpdateDrugAsync(updatedDrug) as OkNegotiatedContentResult<DrugDto>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.BrandName, updatedDrug.BrandName);
            Assert.AreEqual(result.Content.GenericName, updatedDrug.GenericName);

        }

        [TestMethod]
        public async Task RemoveDrugAsync_ShouldReturnDrugNotFound()
        {
            var result = await DrugController.RemoveDrugAsync(1000000);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task RemoveDrugAsync_ShouldReturnOk()
        {
            Drug drug = base.UnitOfWork.Drugs.Get().ToList()[0];
            var result = await DrugController.RemoveDrugAsync(drug.Id) as OkResult;
            Assert.IsNotNull(result);
        }
    }
}
