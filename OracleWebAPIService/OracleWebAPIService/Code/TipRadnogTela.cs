using System.Text.Json.Serialization;

namespace WebAPI.Code;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TipOdeljenja
{
    StalniOdbori,
    AnketniOdbori,
    Komisije,
    PrivremeniOdbori
}
