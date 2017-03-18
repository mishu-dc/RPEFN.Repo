using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPEFN.WebService.Controllers;
using RPEFN.WebService.Dtos;

namespace RPEFN.UnitTests.Controllers
{
    [TestClass]
    public class PrescriptionControllerTest : BaseTest
    {
        public PrescriptionControllerTest()
        {
            base.MockControllerProperties(new PrescriptionController(UnitOfWork), "Prescription");
        }

        private PrescriptionController PrescriptionController => Controller as PrescriptionController;

        [TestMethod]
        public async Task GetPrescriptionAsync_ShouldReturnRxs()
        {
            var result = await PrescriptionController.GetPrescriptionsAsync() as OkNegotiatedContentResult<IEnumerable<PrescriptionDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Count(), UnitOfWork.Prescriptions.Get().Count());
        }

        [TestMethod]
        public async Task GetPrescriptionAsync_ShouldReturnOneRx()
        {
            var rx = UnitOfWork.Prescriptions.Get(1);
            var result = await PrescriptionController.GetPrescriptionAsync(1) as OkNegotiatedContentResult<PrescriptionDto>;

            Assert.IsNotNull(result);

            Assert.AreEqual(rx.Drug.BrandName, result.Content.Drug.BrandName);
            Assert.AreEqual(rx.Drug.GenericName, result.Content.Drug.GenericName);
            Assert.AreEqual(rx.Drug.NdcId, result.Content.Drug.NdcId);
            Assert.AreEqual(rx.Drug.Price, result.Content.Drug.Price);

            Assert.AreEqual(rx.Patient.DateOfBirth, result.Content.Patient.DateOfBirth);
            Assert.AreEqual(rx.Patient.FirstName, result.Content.Patient.FirstName);
            Assert.AreEqual(rx.Patient.LastName, result.Content.Patient.LastName);

            Assert.AreEqual(rx.Dose, result.Content.Dose);
            Assert.AreEqual(rx.Duration, result.Content.Duration);
            Assert.AreEqual(rx.WrittenDate, result.Content.WrittenDate);
        }

        [TestMethod]
        public async Task GetPrescriptionAsync_ShouldReturnResultNotFound()
        {
            var result = await PrescriptionController.GetPrescriptionAsync(10000);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdatePrescriptionAsync_ShouldReturnNotFound()
        {
            var rx = UnitOfWork.Prescriptions.Get(1);
            var rxDto = new PrescriptionDto()
            {
                Dose = rx.Dose,
                Duration = rx.Duration,
                WrittenDate = rx.WrittenDate,
                Drug = new DrugDto() { Id = rx.Drug.Id, BrandName = rx.Drug.BrandName, GenericName = rx.Drug.GenericName },
                Patient = new PatientDto() { Id = rx.Patient.Id, FirstName = rx.Patient.FirstName, LastName = rx.Patient.LastName }
            };

            var result = await PrescriptionController.UpdatePrescriptionAsync(rxDto);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdatePrescriptionAsync_ShouldReturnOkResult()
        {
            var rx = UnitOfWork.Prescriptions.Get(1);
            var rxDto = new PrescriptionDto()
            {
                Id = rx.Id,
                Dose = rx.Dose,
                Duration = 90,
                WrittenDate = DateTime.Now.AddDays(-30),
                DrugId = rx.DrugId,
                PatientId = rx.PatientId
                //Drug = new DrugDto() { Id = rx.Drug.Id, BrandName = rx.Drug.BrandName, GenericName = rx.Drug.GenericName },
                //Patient = new PatientDto() { Id = rx.Patient.Id, FirstName = rx.Patient.FirstName, LastName = rx.Patient.LastName }
            };

            var result = await PrescriptionController.UpdatePrescriptionAsync(rxDto) as OkNegotiatedContentResult<PrescriptionDto>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Duration, rxDto.Duration);
            Assert.AreEqual(result.Content.WrittenDate, rxDto.WrittenDate);
        }

        [TestMethod]
        public async Task CreatePrescriptionAsync_ShouldReturnBadRequest()
        {
            var rxDto = new PrescriptionDto()
            {
                Dose = "Once a day",
                Duration = 30,
                WrittenDate = DateTime.Now
            };

            PrescriptionController.Validate(rxDto);

            var result = await PrescriptionController.CreatePrescriptionAsync(rxDto);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task CreatePrescriptionAsync_ShouldReturnCreatedResult()
        {
            int count = UnitOfWork.Prescriptions.Get().ToList().Count;

            var drug = UnitOfWork.Drugs.Get(1);
            var patient = UnitOfWork.Patients.Get(1);

            var rxDto = new PrescriptionDto()
            {
                Dose = "Once a day",
                Duration = 90,
                DrugId = drug.Id,
                PatientId = patient.Id,
                WrittenDate = DateTime.Now.AddDays(-30),
                //Drug = new DrugDto() { Id = drug.Id, BrandName = drug.BrandName, GenericName = drug.GenericName, NdcId = drug.NdcId, Price = drug.Price, Strength = drug.Strength},
                //Patient = new PatientDto() { Id = patient.Id, FirstName = patient.FirstName, LastName = patient.LastName, DateOfBirth = patient.DateOfBirth, Gender = patient.Gender}
            };

            PrescriptionController.Validate(rxDto);

            var result = await PrescriptionController.CreatePrescriptionAsync(rxDto) as CreatedNegotiatedContentResult<PrescriptionDto>;

            Assert.IsNotNull(result);
            Assert.AreEqual(UnitOfWork.Prescriptions.Get().ToList().Count, count + 1);
            
        }

        [TestMethod]
        public async Task DeletePrescription_ShouldReturnNotFoundResult()
        {
            var result = await PrescriptionController.DeletePrescriptionAsync(10000);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeletePrescription_ShouldReturnOkResult()
        {
            int count = UnitOfWork.Patients.Get().ToList().Count;
            var result = await PrescriptionController.DeletePrescriptionAsync(1);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
