using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DataModels.Models
{
    public partial class NewCardReply
    {
        [JsonProperty("model")]
        public Model Model { get; set; }

        [JsonProperty("action")]
        public Action Action { get; set; }
    }

    public partial class Action
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("idMemberCreator")]
        public string IdMemberCreator { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("appCreator")]
        public object AppCreator { get; set; }

        [JsonProperty("limits")]
        public Limits Limits { get; set; }

        [JsonProperty("display")]
        public Display Display { get; set; }

        [JsonProperty("memberCreator")]
        public MemberCreator MemberCreator { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("card")]
        public Ard Card { get; set; }

        [JsonProperty("list")]
        public List List { get; set; }

        [JsonProperty("board")]
        public Ard Board { get; set; }
    }

    public partial class Ard
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shortLink")]
        public string ShortLink { get; set; }

        [JsonProperty("idShort", NullValueHandling = NullValueHandling.Ignore)]
        public long? IdShort { get; set; }
    }

    public partial class List
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Display
    {
        [JsonProperty("translationKey")]
        public string TranslationKey { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }
    }

    public partial class Entities
    {
        [JsonProperty("card")]
        public Card Card { get; set; }

        [JsonProperty("list")]
        public Card List { get; set; }

        [JsonProperty("memberCreator")]
        public Card MemberCreator { get; set; }
    }

    public partial class Card
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("shortLink", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortLink { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }
    }

    public partial class Limits
    {
    }

    public partial class MemberCreator
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("activityBlocked")]
        public bool ActivityBlocked { get; set; }

        [JsonProperty("avatarHash")]
        public string AvatarHash { get; set; }

        [JsonProperty("avatarUrl")]
        public Uri AvatarUrl { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("idMemberReferrer")]
        public string IdMemberReferrer { get; set; }

        [JsonProperty("initials")]
        public string Initials { get; set; }

        [JsonProperty("nonPublic")]
        public Limits NonPublic { get; set; }

        [JsonProperty("nonPublicAvailable")]
        public bool NonPublicAvailable { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public partial class Model
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("descData")]
        public object DescData { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        [JsonProperty("idOrganization")]
        public string IdOrganization { get; set; }

        [JsonProperty("idEnterprise")]
        public object IdEnterprise { get; set; }

        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("shortUrl")]
        public Uri ShortUrl { get; set; }

        [JsonProperty("labelNames")]
        public LabelNames LabelNames { get; set; }

        [JsonProperty("prefs")]
        public Prefs Prefs { get; set; }
    }

    public partial class LabelNames
    {
        [JsonProperty("green")]
        public string Green { get; set; }

        [JsonProperty("yellow")]
        public string Yellow { get; set; }

        [JsonProperty("orange")]
        public string Orange { get; set; }

        [JsonProperty("red")]
        public string Red { get; set; }

        [JsonProperty("purple")]
        public string Purple { get; set; }

        [JsonProperty("blue")]
        public string Blue { get; set; }

        [JsonProperty("sky")]
        public string Sky { get; set; }

        [JsonProperty("lime")]
        public string Lime { get; set; }

        [JsonProperty("pink")]
        public string Pink { get; set; }

        [JsonProperty("black")]
        public string Black { get; set; }
    }

    public partial class Prefs
    {
        [JsonProperty("permissionLevel")]
        public string PermissionLevel { get; set; }

        [JsonProperty("hideVotes")]
        public bool HideVotes { get; set; }

        [JsonProperty("voting")]
        public string Voting { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("invitations")]
        public string Invitations { get; set; }

        [JsonProperty("selfJoin")]
        public bool SelfJoin { get; set; }

        [JsonProperty("cardCovers")]
        public bool CardCovers { get; set; }

        [JsonProperty("isTemplate")]
        public bool IsTemplate { get; set; }

        [JsonProperty("cardAging")]
        public string CardAging { get; set; }

        [JsonProperty("calendarFeedEnabled")]
        public bool CalendarFeedEnabled { get; set; }

        [JsonProperty("isPluginHeaderShortcutsEnabled")]
        public bool IsPluginHeaderShortcutsEnabled { get; set; }

        [JsonProperty("enabledPluginBoardButtons")]
        public List<object> EnabledPluginBoardButtons { get; set; }

        [JsonProperty("background")]
        public string Background { get; set; }

        [JsonProperty("backgroundImage")]
        public Uri BackgroundImage { get; set; }

        [JsonProperty("backgroundImageScaled")]
        public List<BackgroundImageScaled> BackgroundImageScaled { get; set; }

        [JsonProperty("backgroundTile")]
        public bool BackgroundTile { get; set; }

        [JsonProperty("backgroundBrightness")]
        public string BackgroundBrightness { get; set; }

        [JsonProperty("backgroundBottomColor")]
        public string BackgroundBottomColor { get; set; }

        [JsonProperty("backgroundTopColor")]
        public string BackgroundTopColor { get; set; }

        [JsonProperty("canBePublic")]
        public bool CanBePublic { get; set; }

        [JsonProperty("canBeEnterprise")]
        public bool CanBeEnterprise { get; set; }

        [JsonProperty("canBeOrg")]
        public bool CanBeOrg { get; set; }

        [JsonProperty("canBePrivate")]
        public bool CanBePrivate { get; set; }

        [JsonProperty("canInvite")]
        public bool CanInvite { get; set; }
    }

    public partial class BackgroundImageScaled
    {
        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class NewCardReply
    {
        public static NewCardReply FromJson(string json) => JsonConvert.DeserializeObject<NewCardReply>(json, DataModels.Models.NewCardConverter.Settings);
    }

    public static class SerializeNewCard
    {
        public static string ToJson(this NewCardReply self) => JsonConvert.SerializeObject(self, DataModels.Models.NewCardConverter.Settings);
    }

    internal static class NewCardConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
