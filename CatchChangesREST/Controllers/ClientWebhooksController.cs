using System;
using System.Text.Json;
using System.Threading.Tasks;
using CatchChangesREST.Clients;
using CatchChangesREST.Clients.Telegram;
using CatchChangesREST.DataSources;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CatchChangesREST.Controllers
{
    [Route("{clientName}/webhook")]
    public class ClientWebhooksController : Controller
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly SubscriptionService _subscriptionService;

        public ClientWebhooksController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Receiving updates from a client. (e.g., new messages, new actions ...) 
        /// </summary>
        [Route("receive/{telegramToken}")] // todo client level is not supposed to know about telegram
        [HttpPost]
        public async Task<IActionResult> ReceiveUpdate(string clientName, [FromBody] JsonElement jsonElement,
            string telegramToken)
        {
            try
            {
                RequestBuilder.InitData(out _, out _, out var realTelegramToken);
                if (realTelegramToken != telegramToken) throw new Exception("Received token is expired");
                var client = _subscriptionService.ClientByName[clientName];
                _logger.Trace($"Received update from client: Client:{client.Name} Update: {jsonElement.ToString()}");
                var update = HttpHelper.GetModel<Update>(jsonElement.ToString());
                _logger.Trace(
                    $"Checking model of update. Message: {update?.Message} Chat id: {update?.Message?.Chat?.Id} Text: {update?.Message?.Text}");

                await client.SendAsync(update.Message.Chat.Id, jsonElement.ToString());
                if (update.Message.Text == Commands.Subscribe)
                    client.Subscribe(update.Message.From, update.Message.Chat);

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
    }
}