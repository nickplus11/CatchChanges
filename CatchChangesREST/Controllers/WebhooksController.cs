using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CatchChangesREST.DataSources;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CatchChangesREST.Controllers
{
    [Route("{dataSourceName}/webhook")]
    public class WebhooksController : Controller
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IReadOnlyList<IDataSource> _dataSources;

        public WebhooksController(IEnumerable<IDataSource> dataSources)
        {
            _dataSources = dataSources.ToList();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateWebhooks(string dataSourceName)
        {
            try
            {
                var dataSource = _dataSources.First(s => s.Name == dataSourceName);
                var tables = await dataSource.GetAllTablesAsync();
                _logger.Trace($"{tables.Count} tables have been received.");
                var table = tables.First(t => t.Name == "KeepWorking");
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
        public async Task<IActionResult> ReceiveWebhooks(string dataSourceName)
        {
            try
            {
                _logger.Trace("Started receiving a webhook.");
                var dataSource = _dataSources.First(s => s.Name == dataSourceName);
                using var reader = new StreamReader(ControllerContext.HttpContext.Request.Body);
                var requestBody = await reader.ReadToEndAsync();
                _logger.Trace($"Receiving new webhook. Request body: {requestBody} Response body: not available now");
                await dataSource.ReceiveWebhookAsync(requestBody);
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