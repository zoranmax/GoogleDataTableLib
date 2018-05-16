using System;
using System.Runtime.Serialization;

namespace Google.DataTable.Net.Wrapper
{
    public static class SerializationInfoExtensions
    {
        public static T TryGetValue<T>(this SerializationInfo info, string property, T defaultValue)// where T : class
        {
            try
            {
                var value = info.GetValue(property, typeof(T));
                if (value != null)
                    return (T)value;
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}