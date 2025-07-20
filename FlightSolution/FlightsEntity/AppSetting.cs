using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEntity
{
 
        /// <summary>
        /// Application settings root.
        /// </summary>
        public class AppSetting
        {
            /// <summary>Database connection strings.</summary>
            public ConnectionString ConnectionString { get; set; }
            /// <summary>JWT settings.</summary>
            public JwtSetting Jwt { get; set; }
        }
        /// <summary>
        /// Database connection string settings.
        /// </summary>
        public class ConnectionString
        {
            /// <summary>Flights database connection string.</summary>
            public string Flights { get; set; }
        }


        /// <summary>
        /// JWT authentication settings.
        /// </summary>
        public class JwtSetting
        {
            /// <summary>Secret key for JWT.</summary>
            public string SecretKey { get; set; }
            /// <summary>JWT issuer.</summary>
            public string Issuer { get; set; }
            /// <summary>JWT audience.</summary>
            public string Audience { get; set; }
            /// <summary>Token expiration in minutes.</summary>
            public int ExpireMinute { get; set; }
        }
    }


