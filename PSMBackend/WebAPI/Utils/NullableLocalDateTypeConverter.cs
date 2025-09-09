using System.ComponentModel;
using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace PSMWebAPI.Utils
{
    public class NullableLocalDateTypeConverter : TypeConverter
    {
        private static readonly LocalDatePattern Pattern = LocalDatePattern.Iso;

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                // Handle null, empty, or "null" string values
                if (string.IsNullOrWhiteSpace(stringValue) || stringValue.Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                var parseResult = Pattern.Parse(stringValue);
                if (parseResult.Success)
                {
                    return parseResult.Value;
                }

                // If parsing fails, return null for nullable types
                return null;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value is LocalDate localDate)
                {
                    return Pattern.Format(localDate);
                }
                return string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
