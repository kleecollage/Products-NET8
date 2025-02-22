// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var paypalToken = PaypalToken.FromJson(jsonString);

namespace Web.Models
{
  using System;
  using System.Collections.Generic;

  using System.Globalization;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Converters;

  public partial class PaypalToken
  {
    [JsonProperty("scope")]
    public Uri Scope { get; set; }

    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("app_id")]
    public string AppId { get; set; }

    [JsonProperty("expires_in")]
    public long ExpiresIn { get; set; }

    [JsonProperty("nonce")]
    public string Nonce { get; set; }
  }

  public partial class PaypalToken
  {
    public static PaypalToken FromJson(string json) => JsonConvert.DeserializeObject<PaypalToken>(json, Web.Models.Converter.Settings);
  }

  public static class Serialize
  {
    public static string ToJson(this PaypalToken self) => JsonConvert.SerializeObject(self, Web.Models.Converter.Settings);
  }

  internal static class Converter
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
