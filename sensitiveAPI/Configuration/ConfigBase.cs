using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace sensitiveAPI.Configuration
{
    /// <summary>
    /// Base class for strongly typed configuration classes.
    ///
    /// The idea is to allow you to define a config class specific
    /// for your project, but gain easy access to reading config values
    /// from wherever.
    /// </summary>
    public abstract class ConfigBase
    {
        private readonly IConfiguration _configuration;

        protected ConfigBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected T Get<T>(string key, T defaultValue)
        {
            var value = _configuration[key];

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            if (TypeDescriptor.GetConverter(typeof(T)).IsValid(value))
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);

            return defaultValue;
        }
    }
}
