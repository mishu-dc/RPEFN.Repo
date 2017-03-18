using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPEFN.Data.Entities;
using RPEFN.WebService.Controllers;
using RPEFN.WebService.Dtos;

namespace RPEFN.UnitTests.Controllers
{
    [TestClass]
    public class PatientControllerTest:BaseTest
    {
        public PatientControllerTest()
        {
            base.MockControllerProperties(new PatientController(UnitOfWork), "Patient" );    
        }

        private PatientController PatientController => Controller as PatientController;

        [TestMethod]
        public async Task PatientsAsync_ShouldReturnPatients()
        {
            var result = await PatientController.PatientsAsync() as OkNegotiatedContentResult<IEnumerable<PatientDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Count(), UnitOfWork.Patients.Get().Count());
        }

        [TestMethod]
        public async Task PatientAsync_ShouldReturnOnePatient()
        {
            var patient = UnitOfWork.Patients.Get(1);
            var result = await PatientController.PatientAsync(1) as OkNegotiatedContentResult<PatientDto>;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(patient.FirstName, result.Content.FirstName);
            Assert.AreEqual(patient.LastName, result.Content.LastName);
            Assert.AreEqual(patient.DateOfBirth, result.Content.DateOfBirth);
            Assert.AreEqual(patient.Gender, result.Content.Gender);
        }

        [TestMethod]
        public async Task PatientAsync_ShouldReturnResultNotFound()
        {
            var result = await PatientController.PatientAsync(10000);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdatePatientAsync_ShouldReturnBadRequest()
        {
            var patient = new PatientDto();
            PatientController.Validate(patient);
            var result = await PatientController.UpdatePatientAsync(patient);

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task UpdatePatientAsync_ShouldReturnNotFound()
        {
            var patient = UnitOfWork.Patients.Get(1);
            var patientDto = new PatientDto()
            {
                FirstName = patient.FirstName, LastName = patient.LastName, DateOfBirth =  patient.DateOfBirth, Gender = patient.Gender
            };

            var result = await PatientController.UpdatePatientAsync(patientDto);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdatePatientAsync_ShouldReturnOkResult()
        {
            var patient = UnitOfWork.Patients.Get(1);
            var patientDto = new PatientDto()
            {
                Id= patient.Id,
                FirstName = "Updated First Name",
                LastName = "Updated Last Name",
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender
            };

            var result = await PatientController.UpdatePatientAsync(patientDto) as OkNegotiatedContentResult<PatientDto>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.FirstName, patientDto.FirstName);
            Assert.AreEqual(result.Content.LastName, patientDto.LastName);
        }

        [TestMethod]
        public async Task PatientAsync_ShouldReturnBadRequest()
        {
            var patientDto = new PatientDto() { DateOfBirth = DateTime.Now, FirstName = "Test"};

            PatientController.Validate(patientDto);

            var result = await PatientController.PatientAsync(patientDto);
            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task PatientAsync_ShouldReturnCreatedResult()
        {
            int count = UnitOfWork.Patients.Get().ToList().Count;
            var patientDto = new PatientDto() { DateOfBirth = DateTime.Now, FirstName = "Automation", LastName = "Patient", Gender = "M"};

            PatientController.Validate(patientDto);

            var result = await PatientController.PatientAsync(patientDto) as CreatedNegotiatedContentResult<PatientDto>;

            Assert.IsNotNull(result);
            Assert.AreEqual(UnitOfWork.Patients.Get().ToList().Count, count+1);
            Assert.AreEqual(result.Content.FirstName, patientDto.FirstName);
            Assert.AreEqual(result.Content.LastName, patientDto.LastName);
            Assert.AreEqual(result.Content.DateOfBirth, patientDto.DateOfBirth);
        }

        [TestMethod]
        public async Task DeletePatientAsync_ShouldReturnNotFoundResult()
        {
            var result = await PatientController.DeletePatientAsync(10000);
            Assert.IsInstanceOfType(result,typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeletePatientAsync_ShouldReturnOkResult()
        {
            int count = UnitOfWork.Patients.Get().ToList().Count;
            var result = await PatientController.DeletePatientAsync(1);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
