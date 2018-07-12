using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FourSolid.Cqrs.Anagrafiche.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class ArticoliController : CommonController
    {
        private readonly IArticoloOrchestrator _articoloOrchestrator;
        private readonly ILogger<ArticoliController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articoloOrchestrator"></param>
        /// <param name="logger"></param>
        public ArticoliController(IArticoloOrchestrator articoloOrchestrator, ILogger<ArticoliController> logger)
        {
            this._articoloOrchestrator = articoloOrchestrator;
            this._logger = logger;
        }

        /// <summary>
        /// Create a new Articolo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateArticolo([FromBody] ArticoloJson articoloToCreate)
        {
            try
            {
                var commandInfo = this.DecodeJwtToken();
                await this._articoloOrchestrator.CreateArticoloAsync(articoloToCreate, commandInfo.Who, commandInfo.When);

                return this.Created("articoli", new PostResult("/", ""));
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ArticoliController.CreateArticolo] - {CommonServices.GetErrorMessage(ex)}");
                return this.BadRequest($"[ArticoliController.CreateArticolo] - {CommonServices.GetErrorMessage(ex)}");
            }
        }

        /// <summary>
        /// Gets a list of Articoli
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<ArticoloJson>> GetArticoli()
        {
            try
            {
                return await this._articoloOrchestrator.GetArticoliAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ArticoliController.GetArticoli] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ArticoliController.GetArticoli] - {CommonServices.GetErrorMessage(ex)}");
            }
        }

        /// <summary>
        /// Gets details of Articolo by ArticoloId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{articoloId}")]
        public async Task<ArticoloJson> GetArticoloDetailsById(string articoloId)
        {
            try
            {
                return await this._articoloOrchestrator.GetArticoloByIdAsync(articoloId);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ArticoliController.GetArticoloDetailsById] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ArticoliController.GetArticoloDetailsById] - {CommonServices.GetErrorMessage(ex)}");
            }
        }

        /// <summary>
        /// Modifiy Description by ArticoloId
        /// </summary>
        /// <param name="articolo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{articoloId}")]
        public async Task<IActionResult> ModificaDescrizione([FromBody] ArticoloJson articolo)
        {
            try
            {
                var commandInfo = this.DecodeJwtToken();
                await this._articoloOrchestrator.ModificaDescrizioneArticoloAsync(articolo, commandInfo.Who,
                    commandInfo.When);

                return this.NoContent();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ArticoliController.GetArticoloDetailsById] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ArticoliController.GetArticoloDetailsById] - {CommonServices.GetErrorMessage(ex)}");
                throw;
            }
        }
    }
}