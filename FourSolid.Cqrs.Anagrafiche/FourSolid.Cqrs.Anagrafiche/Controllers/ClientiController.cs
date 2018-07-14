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
    public class ClientiController : CommonController
    {
        private readonly IClienteOrchestrator _clienteOrchestrator;
        private readonly ILogger<ClientiController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteOrchestrator"></param>
        /// <param name="logger"></param>
        public ClientiController(IClienteOrchestrator clienteOrchestrator, ILogger<ClientiController> logger)
        {
            this._clienteOrchestrator = clienteOrchestrator;
            this._logger = logger;
        }

        /// <summary>
        /// Create a New Cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteJson cliente)
        {
            try
            {
                await this._clienteOrchestrator.CreateClienteAsync(cliente, this.CommandInfo.Who, this.CommandInfo.When);

                var clienteUri = $"{GetUri(this.Request)}";
                return this.Created("clienti", new PostResult(clienteUri, ""));
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ClientiController.CreateCliente] - {CommonServices.GetErrorMessage(ex)}");
                return this.BadRequest($"[ClientiController.CreateCliente] - {CommonServices.GetErrorMessage(ex)}");
            }
        }

        /// <summary>
        /// Gets a list of Clienti
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<ClienteJson>> GetClienti()
        {
            try
            {
                return await this._clienteOrchestrator.GetClientiAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ClientiController.CreateCliente] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ClientiController.CreateCliente] - {CommonServices.GetErrorMessage(ex)}");
            }
        }

        /// <summary>
        /// Gets a details of Cliente by clienteId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{clienteId}")]
        public async Task<ClienteJson> GetClienteDetailsById(string clienteId)
        {
            try
            {
                return await this._clienteOrchestrator.GetClienteDetailsByIdAsync(clienteId);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"[ClientiController.CreateCliente] - {CommonServices.GetErrorMessage(ex)}");
                throw new Exception($"[ClientiController.CreateCliente] - {CommonServices.GetErrorMessage(ex)}");
            }
        }
    }
}