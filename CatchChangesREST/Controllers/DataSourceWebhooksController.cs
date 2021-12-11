using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CatchChangesREST.Controllers
{
    [Route("{dataSourceName}/webhook")]
    public class DataSourceWebhooksController : Controller
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SubscriptionService _subscriptionService;

        public DataSourceWebhooksController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateWebhooks(string dataSourceName, [FromBody] CreateWebhookParams webhookParams)
        {
            try
            {
                var dataSource = _subscriptionService.DataSourceByName[dataSourceName];
                var id = webhookParams.IdModel;
                var table = await dataSource.GetTableAsync(id);
                _logger.Trace($"Creating new webhook for board: {table.Name}");
                await dataSource.CreateWebhookAsync(table.Id);
                return new ContentResult
                {
                    Content = "Webhook creation executed",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new EmptyResult();
            }
        }

        [Route("receive")]
        [HttpPost]
        public async Task<IActionResult> ReceiveWebhooks(string dataSourceName, [FromBody] JsonElement jsonElement)
        {
            try
            {
                _logger.Trace("Started receiving a webhook.");
                var dataSource = _subscriptionService.DataSourceByName[dataSourceName];
                _logger.Trace($"Receiving new webhook. Request body: {jsonElement.ToString()}");
                await dataSource.ReceiveWebhookAsync(jsonElement.ToString());
                return new ContentResult
                {
                    Content = "Webhook receiving executed",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Before creating new webhooks TrelloAPI sends HEAD request to callback url
        /// and continues only in case of returned 200 status code
        /// </summary>
        [Route("receive")]
        [HttpHead]
        public IActionResult SubmitSupport(string dataSourceName)
        {
            return new ContentResult
            {
                Content = "Webhook support submitted successfully",
                StatusCode = 200
            };
        }
    }
}