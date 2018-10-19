using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public sealed class NormalizationService
{
    private static readonly Lazy<NormalizationService> _lazyInstance = new Lazy<NormalizationService>(() => new NormalizationService());

    public static NormalizationService Instance
    {
        get
        {
            return _lazyInstance.Value;
        }
    }

    private NormalizationService()
    {
        // place for instance initialization code
    }

    public object NormalizeDate(object data)
    {
        var json = JsonConvert.SerializeObject(data);
        JObject jsonObject = JObject.Parse(json);

        NormalizeDateTimeToMinutePrecision(jsonObject);

        return jsonObject.ToObject<object>();
    }

    private void NormalizeDateTimeToMinutePrecision(JObject jsonObject)
    {
        const string datetimeFieldName = "datetime";
        string updatedDateTimeValue = jsonObject.Value<string>(datetimeFieldName).Substring(0, 12);
        jsonObject[datetimeFieldName] = updatedDateTimeValue;
    }
}