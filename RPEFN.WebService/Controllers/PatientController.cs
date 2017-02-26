using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RPEFN.Data.Entities;
using RPEFN.WebService.Dtos;
using RPEFN.WebService.Infrastructure.Implementations;

namespace RPEFN.WebService.Controllers
{
    [Authorize]
    public class PatientController : ApiController
    {

        private readonly UnitOfWork _unitOfWork;

        private readonly log4net.ILog _logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PatientController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger.Info("Patient Controlled is initialized.");
        }

        [HttpGet]
        public async Task<IHttpActionResult> PatientsAsync()
        {
            try
            {
                return Ok((await _unitOfWork.Patients.GetAsync()).Select(x => new PatientDto()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth
                }));
            }
            catch (Exception ex)
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(ex);
                    return InternalServerError(ex);
                }
                return InternalServerError();
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> PatientAsync(int patientId)
        {
            try
            {
                Patient patient = await _unitOfWork.Patients.GetAsync(patientId);
                if (patient != null)
                    return Ok(new PatientDto()
                    {
                        Id = patient.Id,
                        FirstName = patient.FirstName,
                        LastName = patient.LastName,
                        Gender = patient.Gender,
                        DateOfBirth = patient.DateOfBirth
                    });

                return NotFound();
            }
            catch (Exception ex)
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(ex);
                    return InternalServerError(ex);
                }
                return InternalServerError();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> PatientAsync(PatientDto patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Patient dbPatient = new Patient
                    {
                        DateOfBirth = patient.DateOfBirth,
                        FirstName = patient.FirstName,
                        LastName = patient.LastName,
                        Gender = patient.Gender
                    };

                    _unitOfWork.Patients.Add(dbPatient);

                    await _unitOfWork.CompleteAsync();

                    patient.Id = dbPatient.Id;

                    return Created(new Uri(Request.RequestUri + "/" + patient.Id), patient);
                }
                _logger.Warn(string.Join(" | ", ModelState.Values.SelectMany(e => e.Errors.Take(1)).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(ex);
                    return InternalServerError(ex);
                }
                return InternalServerError();
            }
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdatePatientAsync(PatientDto patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Patient dbPatient = await _unitOfWork.Patients.GetAsync(patient.Id);
                    if (dbPatient == null)
                    {
                        return NotFound();
                    }

                    dbPatient.DateOfBirth = patient.DateOfBirth;
                    dbPatient.FirstName = patient.FirstName;
                    dbPatient.LastName = patient.LastName;
                    dbPatient.Gender = patient.Gender;

                    await _unitOfWork.CompleteAsync();

                    return Ok(patient);
                }
                _logger.Warn(string.Join(" | ", ModelState.Values.SelectMany(e => e.Errors.Take(1)).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(ex);
                    return InternalServerError(ex);
                }
                return InternalServerError();
            }

        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeletePatientAsync(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Patient dbPatient = await _unitOfWork.Patients.GetAsync(id);
                    if (dbPatient == null)
                    {
                        return NotFound();
                    }

                    _unitOfWork.Patients.Remove(dbPatient);
                    await _unitOfWork.CompleteAsync();

                    return Ok();
                }
                _logger.Warn(string.Join(" | ", ModelState.Values.SelectMany(e => e.Errors.Take(1)).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(ex);
                    return InternalServerError(ex);
                }
                return InternalServerError();
            }
        }
    }
}
