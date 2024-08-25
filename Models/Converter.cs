//using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace ActivityClubAPIs.JsonConverters // Replace 'YourNamespace' with your actual namespace
//{
//    public class DateOnlyConverter : JsonConverter<DateOnly>
//    {
//        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            var date = reader.GetString();
//            return DateOnly.Parse(date);
//        }

//        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
//        {
//            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
//        }
//    }
//}
