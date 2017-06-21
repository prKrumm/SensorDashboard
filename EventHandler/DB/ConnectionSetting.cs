using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventHandler.DB
{
    public static class ConnectionSetting
    {
        private static string mongoDBClusterIP
        {
            get
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGODB_SERVICE_HOST")))
                {
                    return Environment.GetEnvironmentVariable("MONGODB_SERVICE_HOST");
                }

                return string.Empty;
            }
        }

        internal static string CONNECTION_STRING
        {
            get
            {
                if (!(string.IsNullOrEmpty(MONGODB_USER) || string.IsNullOrEmpty(MONGODB_PASSWORD)
                || string.IsNullOrEmpty(mongoDBClusterIP) || string.IsNullOrEmpty(MONGODB_DATABASE)))
                {
                    string _connectionString = string.Format("mongodb://{0}:{1}@{2}:{3}/{4}", MONGODB_USER, MONGODB_PASSWORD,
                    mongoDBClusterIP, "27017", MONGODB_DATABASE);

                    return _connectionString;
                }
                else { throw new Exception("MongoDB Cluster IP is not set."); }
            }
        }

        private static string MONGODB_USER
        {
            get
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGODB_USER")))
                {
                    return Environment.GetEnvironmentVariable("MONGODB_USER");
                }

                return string.Empty;
            }
        }

        private static string MONGODB_PASSWORD
        {
            get
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGODB_PASSWORD")))
                {
                    return Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
                }

                return string.Empty;
            }
        }

        internal static string MONGODB_DATABASE
        {
            get
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGODB_DATABASE")))
                {
                    return Environment.GetEnvironmentVariable("MONGODB_DATABASE");
                }

                return string.Empty;
            }
        }
    }
}

