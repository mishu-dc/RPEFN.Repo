using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using RPEFN.Data.Entities;
using RPEFN.WebService.Dtos;
using RPEFN.WebService.Infrastructure.Implementations;

namespace RPEFN.WebService.Controllers
{
    [Authorize]
    public class PrescriptionController : ApiController
    {
        private readonly UnitOfWork _unitOfWork = null;
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PrescriptionController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetPrescriptionAsync(int prescriptionId)
        {
            try
            {
                var rxAwait = _unitOfWork.Prescriptions.GetAsync(prescriptionId);
                Prescription rx = await rxAwait;
                if (rx == null)
                {
                    return NotFound();
                }

                Drug drug = await _unitOfWork.Drugs.GetAsync(rx.DrugId);
                Patient patient = await _unitOfWork.Patients.GetAsync(rx.PatientId);

                return
                    Ok(new 
                    {
                        Id = rx.Id,
                        Drug = new { Id = drug.Id, BrandName = drug.BrandName, Genericname = drug.GenericName, NdcId = drug.NdcId, Price = drug.Price, Strength = drug.Strength },
                        rx.Dose,
                        WrittenDate = rx.WrittenDate.ToString("mm-dd-yyyy"),
                        rx.Duration,
                        Patient = new { Id = patient.Id, FirstName = patient.FirstName, LastName = patient.LastName, Gender = patient.Gender, DateOfBirth = patient.DateOfBirth.ToString("MM-dd-yyyy")  }
                    });
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
        public async Task<IHttpActionResult> GetPrescriptionsAsync()
        {
            try
            {
                return
                    Ok((await _unitOfWork.Prescriptions.GetAllPrescriptionsWithDrugAndPatientAsync()).Select(rx => new 
                    {
                        Id = rx.Id,
                        Drug = new { Id = rx.Drug.Id, BrandName = rx.Drug.BrandName, Genericname = rx.Drug.GenericName, NdcId = rx.Drug.NdcId, Price = rx.Drug.Price, Strength = rx.Drug.Strength },
                        Dose = rx.Dose,
                        WrittenDate = rx.WrittenDate,
                        Duration = rx.Duration,
                        Patient = new { Id = rx.Patient.Id, FirstName = rx.Patient.FirstName, LastName = rx.Patient.LastName, Gender = rx.Patient.Gender, DateOfBirth = rx.Patient.DateOfBirth.ToString("MM-dd-yyyy") }
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

        [HttpPost]
        public async Task<IHttpActionResult> CreatePrescriptionAsync(PrescriptionDto rx)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Drug dbDrug = await _unitOfWork.Drugs.GetAsync(rx.DrugId);

                    if (dbDrug == null)
                    {
                        _logger.Warn($"Invalid Drug Id {rx.DrugId}");
                        return BadRequest("Invalid drugId");
                    }

                    Patient dbPatient = await _unitOfWork.Patients.GetAsync(rx.PatientId);
                    if (dbPatient == null)
                    {
                        _logger.Warn($"Invalid Patient Id {rx.PatientId}");
                        return BadRequest("Invalid patientId");
                    }

                    Prescription dbRx = new Prescription()
                    {
                        Drug = dbDrug,
                        Patient = dbPatient,
                        Dose = rx.Dose,
                        Duration = rx.Duration,
                        WrittenDate = rx.WrittenDate
                    };
                    
                    _unitOfWork.Prescriptions.Add(dbRx);
                    await _unitOfWork.CompleteAsync();

                    rx.Id = dbRx.Id;

                    return Created(new Uri(Request.RequestUri + "/" + rx.Id), rx);
                }

                //If model state is not valid then log all the validation warnings
                _logger.Warn(string.Join(" | ", ModelState.Values.SelectMany(e => e.Errors.Take(1)).Select(e => e.ErrorMessage)));

                //If model state is not valid then return all the validation messages 
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
        public async Task<IHttpActionResult> UpdatePrescriptionAsync(PrescriptionDto rx)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Drug dbDrug = await _unitOfWork.Drugs.GetAsync(rx.DrugId);

                    if (dbDrug == null)
                    {
                        _logger.Warn($"Invalid Drug Id {rx.DrugId}");
                        return BadRequest("Invalid drugId");
                    }

                    Patient dbPatient = await _unitOfWork.Patients.GetAsync(rx.PatientId);
                    if (dbPatient == null)
                    {
                        _logger.Warn($"Invalid Patient Id {rx.PatientId}");
                        return BadRequest("Invalid patientId");
                    }
                    
                    Prescription dbRx = await _unitOfWork.Prescriptions.GetAsync(rx.Id);
                    if (dbRx == null)
                    {
                        _logger.Warn($"Invalid Rx Id {rx.Id}");
                        return BadRequest("Invalid Rx Id");
                    }

                    dbRx.Drug = dbDrug;
                    dbRx.Dose = rx.Dose;
                    dbRx.Duration = rx.Duration;
                    dbRx.WrittenDate = rx.WrittenDate;
                    dbRx.Patient = dbPatient;

                    await _unitOfWork.CompleteAsync();
                    
                    return Ok(rx);
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
        public async Task<IHttpActionResult> DeletePrescriptionAsync(int prescriptionId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    Prescription dbRx = await _unitOfWork.Prescriptions.GetAsync(prescriptionId);
                    if (dbRx == null)
                    {
                        _logger.Warn($"Invalid Rx Id {prescriptionId}");
                        return BadRequest("Invalid Rx Id");
                    }

                    _unitOfWork.Prescriptions.Remove(dbRx);
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
