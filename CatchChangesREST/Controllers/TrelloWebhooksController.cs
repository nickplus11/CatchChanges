using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CatchChangesREST.DataSources;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CatchChangesREST.Controllers
{
    [Route("trello/webhook")]
    public class WebhooksController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateWebhooks()
        {
            try
            {
                var tables = await RequestBuilder.GetAllTablesAsync();
                Logger.Trace($"{tables.Count} tables have been received.");
                var table = tables.First(t => t.Name == "KeepWorking");
                Logger.Trace($"Creating new webhook for board: {table.Name}");
                await RequestBuilder.CreateWebhookAsync(table.Id);
                return new ContentResult
                {
                    Content = "Webhook creation executed",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new EmptyResult();
            }
        }

        [Route("receive")]
        [HttpPost]
        public async Task<IActionResult> ReceiveWebhooks()
        {
            try
            {
                Logger.Trace("Started receiving a webhook.");
                using var reader = new StreamReader(ControllerContext.HttpContext.Request.Body);
                var requestBody = await reader.ReadToEndAsync();
                Logger.Trace($"Receiving new webhook. Request body: {requestBody} Response body: not available now");
                await RequestBuilder.ReceiveWebhookAsync(requestBody);
                return new ContentResult
                {
                    Content = "Webhook receiving executed",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Before creating new webhooks TrelloAPI sends HEAD request to callback url
        /// and continues only in case of returned 200 status code
        /// </summary>
        [Route("receive")]
        [HttpHead]
        public IActionResult SubmitSupport()
        {
            return new ContentResult
            {
                Content = "Webhook support submitted successfully",
                StatusCode = 200
            };
        }

        [Route("get")]
        [HttpGet]
        public IReadOnlyList<string> GetReceivedWebhooks() => RequestBuilder.GetReceivedChangesAsync();
    }
}