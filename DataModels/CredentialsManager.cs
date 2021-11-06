using System;
using System.IO;
using System.Text.Json;
using DataModels.Models;
using NLog;

namespace DataModels
{
    public static class CredentialsManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static Credentials Credentials;

        public static bool TryInit()
        { 
            try
            {
                var text = File.ReadAllText("credentials.json");
                var credentials = JsonSerializer.Deserialize<Credentials>(text);
                Logger.Info($"Key: {credentials?.Key} Token: {credentials?.Token}");
                Credentials = credentials ?? throw new Exception("Json deserialization returned null value");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
    }
}