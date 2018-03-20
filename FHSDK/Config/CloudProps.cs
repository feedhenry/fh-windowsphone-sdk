using FHSDK.Config;
using Newtonsoft.Json.Linq;

namespace FHSDK
{
    /// <summary>
    ///     Class represents the cloud app instance (MBAAS service) the app should be communication with.
    /// </summary>
    public class CloudProps
    {
        private readonly JObject _cloudPropsJson;
        private string _env;
        private string _hostUrl;
        private readonly FHConfig _config;

        // Optional modifier to help with doing reverse-proxy
        public delegate string HostUrlModifier(string srcUrl);
        // The default Host URL modifier is null. Clients may set it if required.
        public static HostUrlModifier UrlModifier;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="props">The json format of the cloud host info</param>
        public CloudProps(JObject props)
        {
            _cloudPropsJson = props;
            _config = FHConfig.GetInstance();
        }
        /// <summary>
        /// Constructor used for unit testing and injecting mock.
        /// </summary>
        /// <param name="props">list of properties returned by FH.init</param>
        /// <param name="config">config retrievd from fhconfig.json file.</param>
        public CloudProps(JObject props, FHConfig config)
        {
            _cloudPropsJson = props;
            _config = config;
        }
        /// <summary>
        ///     Return the cloud host info as URL
        /// </summary>
        /// <returns>the cloud host url</returns>
        public string GetCloudHost()
        {
            if (null != _hostUrl) return _hostUrl;
            if (null != _cloudPropsJson["url"])
            {
                _hostUrl = (string) _cloudPropsJson["url"];
            }
            else
            {
                var hosts = (JObject) _cloudPropsJson["hosts"];
                if (null != hosts["url"])
                {
                    _hostUrl = (string) hosts["url"];
                }
                else
                {
                    var appMode = _config.GetMode();
                    if ("dev" == appMode)
                    {
                        _hostUrl = (string) hosts["debugCloudUrl"];
                    }
                    else
                    {
                        _hostUrl = (string) hosts["releaseCloudUrl"];
                    }
                }
            }
            _hostUrl = _hostUrl.EndsWith("/") ? _hostUrl.Substring(0, _hostUrl.Length - 1) : _hostUrl;
            if (UrlModifier != null)
                _hostUrl = UrlModifier(_hostUrl);
            return _hostUrl;
        }

        public string GetEnv()
        {
            if (null != _env) return _env;
            var hosts = (JObject) _cloudPropsJson["hosts"];
            if (null != hosts["environment"])
            {
                _env = (string) hosts["environment"];
            }
            return _env;
        }
    }
}

