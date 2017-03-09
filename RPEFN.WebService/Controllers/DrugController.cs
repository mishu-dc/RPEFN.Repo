using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.UI;
using RPEFN.Data.Entities;
using RPEFN.WebService.Dtos;
using RPEFN.WebService.Infrastructure.Implementations;

namespace RPEFN.WebService.Controllers
{
    [System.Web.Http.Authorize]
    public class DrugController : ApiController
    {
        private readonly UnitOfWork _unitOfWork = null;
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DrugCachekey = "DRUG";

        public DrugController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger.Info("Drug Controller Initialized successfully");
        }

        private void InvalidateCache()
        {
            MemoryCache.Default.Remove(DrugCachekey);
        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetDrugsAsync()
        {
            try
            {
                IEnumerable<Drug> drugs = null;

                if (MemoryCache.Default[DrugCachekey] != null)
                {
                    drugs = (IEnumerable<Drug>) MemoryCache.Default[DrugCachekey];
                }
                else
                {
                    drugs = await _unitOfWork.Drugs.GetAsync();
                    MemoryCache.Default.Add(DrugCachekey, drugs, DateTimeOffset.UtcNow.AddMinutes(10));
                }

                return Ok(drugs.Select(d => new DrugDto()
                {
                    Id = d.Id,
                    BrandName = d.BrandName,
                    GenericName = d.GenericName,
                    Strength = d.Strength,
                    Price = d.Price,
                    NdcId = d.NdcId
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

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetDrugAsync(int drugId)
        {
            try
            {
                Drug dbDrug = await _unitOfWork.Drugs.GetAsync(drugId);
                if (dbDrug == null)
                {
                    _logger.Warn($"Invalid drugId: {drugId}");
                    return NotFound();
                }
                   
                return Ok(new DrugDto()
                {
                    Id = dbDrug.Id,
                    BrandName = dbDrug.BrandName,
                    GenericName = dbDrug.GenericName,
                    Strength = dbDrug.Strength,
                    Price = dbDrug.Price,
                    NdcId = dbDrug.NdcId
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

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CreateDrugAsync(DrugDto drug)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Drug dbDrug = new Drug
                    {
                        BrandName = drug.BrandName,
                        GenericName = drug.GenericName,
                        NdcId = drug.NdcId,
                        Price = drug.Price,
                        Strength = drug.Strength,
                        CreatedBy = User.Identity.Name
                    };


                    _unitOfWork.Drugs.Add(dbDrug);
                    await _unitOfWork.CompleteAsync();
 
                    drug.Id = dbDrug.Id;

                    _logger.Info($"New drug created with id {drug.Id}");

                    InvalidateCache();
                    
                    return Created(new Uri(Request.RequestUri + "/" + drug.Id), drug);
                    
                }
                else
                {
                    _logger.Warn(string.Join(" | ", ModelState.Values.SelectMany(e => e.Errors.Take(1)).Select(e => e.ErrorMessage)));
                    return BadRequest(ModelState);
                }
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

        [System.Web.Http.HttpPut]
        public async Task<IHttpActionResult> UpdateDrugAsync(DrugDto drug)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Drug dbDrug = await _unitOfWork.Drugs.GetAsync(drug.Id);
                    if (dbDrug == null)
                    {
                        _logger.Warn($"Invalid drugId: {drug.Id}");
                        return NotFound();
                    }

                    dbDrug.BrandName = drug.BrandName;
                    dbDrug.GenericName = drug.GenericName;
                    dbDrug.NdcId = drug.NdcId;
                    dbDrug.Price = drug.Price;
                    dbDrug.Strength = drug.Strength;
                    dbDrug.UpdatedBy = User.Identity.Name;

                    await _unitOfWork.CompleteAsync();

                    InvalidateCache();

                    return Ok(drug);
                }
                else
                {
                    _logger.Warn(string.Join(" | ",ModelState.Values.SelectMany(e => e.Errors.Take(1)).Select(e=>e.ErrorMessage)));
                    return BadRequest(ModelState);
                }
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

        [System.Web.Http.HttpDelete]
        public async Task<IHttpActionResult> RemoveDrugAsync(int id)
        {
            try
            {
                Drug dbDrug = await _unitOfWork.Drugs.GetAsync(id);
                if (dbDrug == null)
                {
                    _logger.Warn($"Invalid drugId: {id}");
                    return NotFound();
                }
                _unitOfWork.Drugs.Remove(dbDrug);
                await _unitOfWork.CompleteAsync();

                InvalidateCache();

                return Ok();
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
