﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Nmsjson;
//
//    var nms = Nms.FromJson(jsonString);

namespace Nmsjson
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Nms
    {
        [JsonProperty("F2P", NullValueHandling = NullValueHandling.Ignore)]
        public long? F2P { get; set; }

        [JsonProperty("8>q", NullValueHandling = NullValueHandling.Ignore)]
        public string The8Q { get; set; }

        [JsonProperty("6f=", NullValueHandling = NullValueHandling.Ignore)]
        public The6F The6F { get; set; }

        [JsonProperty("rnc", NullValueHandling = NullValueHandling.Ignore)]
        public Rnc Rnc { get; set; }

        [JsonProperty("VuQ", NullValueHandling = NullValueHandling.Ignore)]
        public VuQ VuQ { get; set; }

        [JsonProperty("fDu", NullValueHandling = NullValueHandling.Ignore)]
        public FDu FDu { get; set; }
    }

    public partial class FDu
    {
        [JsonProperty("ETO", NullValueHandling = NullValueHandling.Ignore)]
        public Eto Eto { get; set; }
    }

    public partial class Eto
    {
        [JsonProperty("fgt", NullValueHandling = NullValueHandling.Ignore)]
        public long? Fgt { get; set; }

        [JsonProperty("xxK", NullValueHandling = NullValueHandling.Ignore)]
        public long? XxK { get; set; }

        [JsonProperty("OsQ", NullValueHandling = NullValueHandling.Ignore)]
        public OsQ OsQ { get; set; }
    }

    public partial class OsQ
    {
        [JsonProperty("?fB", NullValueHandling = NullValueHandling.Ignore)]
        public FB[] FB { get; set; }
    }

    public partial class FB
    {
        [JsonProperty("8P3", NullValueHandling = NullValueHandling.Ignore)]
        public The8P3 The8P3 { get; set; }

        [JsonProperty("q9a", NullValueHandling = NullValueHandling.Ignore)]
        public VuQ Q9A { get; set; }

        [JsonProperty("ksu", NullValueHandling = NullValueHandling.Ignore)]
        public The3_K Ksu { get; set; }

        [JsonProperty("=wD", NullValueHandling = NullValueHandling.Ignore)]
        public WD WD { get; set; }

        [JsonProperty("B2h", NullValueHandling = NullValueHandling.Ignore)]
        public string B2H { get; set; }

        [JsonProperty("D6b", NullValueHandling = NullValueHandling.Ignore)]
        public string D6B { get; set; }
    }

    public partial class The3_K
    {
        [JsonProperty("f5Q", NullValueHandling = NullValueHandling.Ignore)]
        public string F5Q { get; set; }

        [JsonProperty("K7E", NullValueHandling = NullValueHandling.Ignore)]
        public string K7E { get; set; }

        [JsonProperty("V?:", NullValueHandling = NullValueHandling.Ignore)]
        public string V { get; set; }

        [JsonProperty("D6b", NullValueHandling = NullValueHandling.Ignore)]
        public string D6B { get; set; }

        [JsonProperty("3I1", NullValueHandling = NullValueHandling.Ignore)]
        public long? The3I1 { get; set; }
    }

    public partial class VuQ
    {
    }

    public partial class The8P3
    {
        [JsonProperty("5L6", NullValueHandling = NullValueHandling.Ignore)]
        //public The5_L6? The5L6 { get; set; }
        public string The5L6 { get; set; }

        [JsonProperty("<Dn", NullValueHandling = NullValueHandling.Ignore)]
        public string Dn { get; set; }

        [JsonProperty("bEr", NullValueHandling = NullValueHandling.Ignore)]
        //public The5_L6[] BEr { get; set; }
        public string[] BEr { get; set; }
    }

    public partial class WD
    {
        [JsonProperty("bLr", NullValueHandling = NullValueHandling.Ignore)]
        public long? BLr { get; set; }
    }

    public partial class Rnc
    {
        [JsonProperty("mEH", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] MEh { get; set; }

        [JsonProperty("l2U", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] L2U { get; set; }

        [JsonProperty("tnP", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] TnP { get; set; }

        [JsonProperty("l4H", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] L4H { get; set; }

        [JsonProperty("jk4", NullValueHandling = NullValueHandling.Ignore)]
        public string Jk4 { get; set; }

        [JsonProperty("NGn", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] NGn { get; set; }

        [JsonProperty("uAt", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] UAt { get; set; }

        [JsonProperty("5Sg", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] The5Sg { get; set; }
    }

    public partial class The6F
    {
        [JsonProperty("yhJ", NullValueHandling = NullValueHandling.Ignore)]
        public YhJ YhJ { get; set; }

        [JsonProperty("QQp", NullValueHandling = NullValueHandling.Ignore)]
        public long? QQp { get; set; }

        [JsonProperty("05J", NullValueHandling = NullValueHandling.Ignore)]
        public long? The05J { get; set; }

        [JsonProperty("8br", NullValueHandling = NullValueHandling.Ignore)]
        public long? The8Br { get; set; }

        [JsonProperty("8xx", NullValueHandling = NullValueHandling.Ignore)]
        public long? The8Xx { get; set; }

        [JsonProperty("F?0", NullValueHandling = NullValueHandling.Ignore)]
        public F0[] F0 { get; set; }

        [JsonProperty("nlG", NullValueHandling = NullValueHandling.Ignore)]
        public NlG[] NlG { get; set; }

        [JsonProperty("4hl", NullValueHandling = NullValueHandling.Ignore)]
        public bool[] The4Hl { get; set; }

        [JsonProperty("HbG", NullValueHandling = NullValueHandling.Ignore)]
        public object[] HbG { get; set; }

        [JsonProperty("NQJ", NullValueHandling = NullValueHandling.Ignore)]
        public Nqj Nqj { get; set; }

        [JsonProperty("vrS", NullValueHandling = NullValueHandling.Ignore)]
        public long? VrS { get; set; }

        [JsonProperty("DaC", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DaC { get; set; }

        [JsonProperty("30s", NullValueHandling = NullValueHandling.Ignore)]
        public The30S The30S { get; set; }
    }

    public partial class F0
    {
        [JsonProperty("h4X", NullValueHandling = NullValueHandling.Ignore)]
        public long? H4X { get; set; }

        [JsonProperty("BpT", NullValueHandling = NullValueHandling.Ignore)]
        public long? BpT { get; set; }

        [JsonProperty("oZw", NullValueHandling = NullValueHandling.Ignore)]
        public string OZw { get; set; }

        [JsonProperty("wMC", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] WMc { get; set; }

        [JsonProperty("oHw", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] OHw { get; set; }

        [JsonProperty("CVX", NullValueHandling = NullValueHandling.Ignore)]
        public long? Cvx { get; set; }

        [JsonProperty("wx7", NullValueHandling = NullValueHandling.Ignore)]
        public long? Wx7 { get; set; }

        [JsonProperty("@ZJ", NullValueHandling = NullValueHandling.Ignore)]
        public Zj[] Zj { get; set; }

        [JsonProperty("B2h", NullValueHandling = NullValueHandling.Ignore)]
        public string B2H { get; set; }

        [JsonProperty("3?K", NullValueHandling = NullValueHandling.Ignore)]
        public The3_K The3K { get; set; }

        [JsonProperty("NKm", NullValueHandling = NullValueHandling.Ignore)]
        public string NKm { get; set; }

        [JsonProperty("peI", NullValueHandling = NullValueHandling.Ignore)]
        public PeI PeI { get; set; }

        [JsonProperty("J=S", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(ParseStringConverter))]
        //public long? JS { get; set; }
        public string JS { get; set; }

        [JsonProperty("vyN", NullValueHandling = NullValueHandling.Ignore)]
        public string VyN { get; set; }

        [JsonProperty("D9@", NullValueHandling = NullValueHandling.Ignore)]
        public long[] D9 { get; set; }

        [JsonProperty("rIR", NullValueHandling = NullValueHandling.Ignore)]
        public long[] RIr { get; set; }

        [JsonProperty("idA", NullValueHandling = NullValueHandling.Ignore)]
        public IdA IdA { get; set; }

        [JsonProperty("C0j", NullValueHandling = NullValueHandling.Ignore)]
        public string C0J { get; set; }

        [JsonProperty("i4g", NullValueHandling = NullValueHandling.Ignore)]
        public bool? I4G { get; set; }

        [JsonProperty("tww", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tww { get; set; }
    }

    public partial class IdA
    {
        [JsonProperty("pwt", NullValueHandling = NullValueHandling.Ignore)]
        public string Pwt { get; set; }
    }

    public partial class PeI
    {
        [JsonProperty("DPp", NullValueHandling = NullValueHandling.Ignore)]
        public string DPp { get; set; }
    }

    public partial class Zj
    {
        [JsonProperty("b1:", NullValueHandling = NullValueHandling.Ignore)]
        public long? B1 { get; set; }

        [JsonProperty("r<7", NullValueHandling = NullValueHandling.Ignore)]
        public string R7 { get; set; }

        [JsonProperty("CVX", NullValueHandling = NullValueHandling.Ignore)]
        public long? Cvx { get; set; }

        [JsonProperty("wMC", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] WMc { get; set; }

        [JsonProperty("wJ0", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] WJ0 { get; set; }

        [JsonProperty("aNu", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] ANu { get; set; }
    }

    public partial class NlG
    {
        [JsonProperty("yhJ", NullValueHandling = NullValueHandling.Ignore)]
        public YhJ YhJ { get; set; }

        [JsonProperty("wMC", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] WMc { get; set; }

        [JsonProperty("gk4", NullValueHandling = NullValueHandling.Ignore)]
        public decimal[] Gk4 { get; set; }

        [JsonProperty("iAF", NullValueHandling = NullValueHandling.Ignore)]
        public string IAf { get; set; }

        [JsonProperty("NKm", NullValueHandling = NullValueHandling.Ignore)]
        public string NKm { get; set; }

        [JsonProperty("a>;", NullValueHandling = NullValueHandling.Ignore)]
        public bool? A { get; set; }

        [JsonProperty("tww", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tww { get; set; }
    }

    public partial class YhJ
    {
        [JsonProperty("Iis", NullValueHandling = NullValueHandling.Ignore)]
        public long? Iis { get; set; }

        [JsonProperty("oZw", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, long> OZw { get; set; }
    }

    public partial class Nqj
    {
        [JsonProperty("3fO", NullValueHandling = NullValueHandling.Ignore)]
        public The3FO[] The3FO { get; set; }

        [JsonProperty("K:U", NullValueHandling = NullValueHandling.Ignore)]
        public string KU { get; set; }
    }

    public partial class The30S
    {
        [JsonProperty("yhJ", NullValueHandling = NullValueHandling.Ignore)]
        public YhJ YhJ { get; set; }
    }
    /*
    public partial struct The5_L6
    {
        public long? Integer;
        public string String;

        public static implicit operator The5_L6(long Integer) => new The5_L6 { Integer = Integer };
        public static implicit operator The5_L6(string String) => new The5_L6 { String = String };
    }
    */
    public partial struct The3FO
    {
        public bool? Bool;
        public string String;

        public static implicit operator The3FO(bool Bool) => new The3FO { Bool = Bool };
        public static implicit operator The3FO(string String) => new The3FO { String = String };
    }

    public partial class Nms
    {
        public static Nms FromJson(string json) => JsonConvert.DeserializeObject<Nms>(json, Nmsjson.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Nms self) => JsonConvert.SerializeObject(self, Nmsjson.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                The3FoConverter.Singleton,
                //The5L6Converter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
    /*
    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
    */
    internal class The3FoConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(The3FO) || t == typeof(The3FO?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Boolean:
                    var boolValue = serializer.Deserialize<bool>(reader);
                    return new The3FO { Bool = boolValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new The3FO { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type The3FO");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (The3FO)untypedValue;
            if (value.Bool != null)
            {
                serializer.Serialize(writer, value.Bool.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type The3FO");
        }

        public static readonly The3FoConverter Singleton = new The3FoConverter();
    }
    /*
    internal class The5L6Converter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(The5_L6) || t == typeof(The5_L6?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    //return new The5_L6 { Integer = integerValue };
                    return new The5_L6 { String = Convert.ToString(integerValue) };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new The5_L6 { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type The5_L6");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (The5_L6)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type The5_L6");
        }

        public static readonly The5L6Converter Singleton = new The5L6Converter();
    }*/
}
