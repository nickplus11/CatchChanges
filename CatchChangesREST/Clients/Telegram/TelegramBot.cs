﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataModels;
using DataModels.Models;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NLog;
using Action = DataModels.Models.Action;

namespace CatchChangesREST.Clients.Telegram
{
    [UsedImplicitly]
    public class TelegramBot : IClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public string Name => "telegram";
        private readonly HashSet<int> _subscribersId = new();
        private readonly Dictionary<int, int> _chatIdByUserId = new();

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
            try
            {
                _subscribersId.Select(async sub =>
                {
                    var (chatId, info) = (_chatIdByUserId[sub], args.NewTable.Name);
                    Logger.Trace($"Sending to telegram. Chat: {chatId}");
                    await SendAsync(chatId, ParseInfoForUser(info));
                }).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
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

        public void Subscribe(object userObj, object chatObj)
        {
            try
            {
                if (userObj is not User user || chatObj is not Chat chat)
                    throw new Exception($"Subscription parameters are not valid. User: {userObj} Chat: {chatObj}");

                _subscribersId.Add(user.Id);
                _chatIdByUserId[user.Id] = chat.Id;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private string ParseInfoForUser(string input)
        {
            string toUser = input;
            try
            {
                JObject reply = JObject.Parse(input);

                DataModels.Models.Action action = Action.FromJson(reply.Last.ToString());

                if (action != null)
                {
                    toUser = "New event: " + action.Display.TranslationKey + " with " + action.Data.ToString() +
                             " by " +
                             action.MemberCreator.FullName;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            return toUser;
        }
    }
}