using System;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using JetBrains.Annotations;
using NLog;

namespace CatchChangesREST.Clients.Telegram
{
    [UsedImplicitly]
    public class TelegramBot : IClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public string Name => "telegram";
        private int _chatId;

        public TelegramBot()
        {
            Logger.Trace("Telegram bot client has been created");
            RequestBuilder.InitData(out var key, out var token, out var telegramToken);
            var callback = $"https://catchchangesrest.azurewebsites.net/telegram/webhook/receive/{telegramToken}";
            var url = $"https://api.telegram.org/bot{telegramToken}/setWebhook?url={callback}";
            RequestBuilder.GetAsync(url).Wait();
        }

        public void OnTableChanged(object sender, TableChangedEventArgs args)
        {
            SendAsync(_chatId, args.NewTable.Name).Wait();
        }

        public void OnListChanged(object sender, ListChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnCardChanged(object sender, CardChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnUpdateReceived(object sender, UpdateReceivedEventArgs args)
        {
            
        }

        public async Task SendAsync(int chatId, string info)
        {
            try
            {
                if (info.Length > 4096)
                {
                    info = info.Take(4000) + "to be continued...";
                }
                Logger.Trace($"Sending info to {Name} client. ChatId: {chatId}");
                _chatId = chatId;
                RequestBuilder.InitData(out var key, out var token, out var telegramToken);
                var url = $"https://api.telegram.org/bot{telegramToken}/sendMessage";
                var model = new SendMessageModel
                {
                    ChatId = chatId,
                    Text = info
                };
                await RequestBuilder.PostAsync(url, model);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}