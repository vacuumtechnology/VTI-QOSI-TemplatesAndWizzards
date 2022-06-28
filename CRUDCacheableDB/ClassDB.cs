using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Hosting;
using WebApplicationQOSI.API.Interfaces;
using $modelnamespace$;
using WebApplicationQOSI.Libraries;

namespace $rootnamespace$
{
	public class $safeitemrootname$ : ICacheable<$keytype$, $valuetype$>
	{
        private static Dictionary<$keytype$, $valuetype$> $valuetype$Cache = new Dictionary<$keytype$, $valuetype$>();
        private readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisUrl"]);

        public static void Refresh()
        {
            DateTime start = DateTime.Now;
            $valuetype$Cache = QueryAll();
            $safeitemrootname$.RedisSet($valuetype$Cache);
            Logger.LogMetric($"$safeitemrootname$.QueryAll FROM DB.Qosi", start);
        }

        public static Dictionary<$keytype$, $valuetype$> Cached()
        {
            if ($valuetype$Cache.Count == 0)
            {
                Dictionary <$keytype$, $valuetype$> data = RedisGet();
                if (data == null) Refresh();
                else
                {
                            $valuetype$Cache = data;
                    Logger.log.Info("$safeitemrootname$ loaded from redis");
                }
            }
            return $valuetype$Cache;
        }

        public static void Bust()
        {
            Logger.log.Warn("$safeitemrootname$.Bust()");
            try
            {
                var db = (new $safeitemrootname$()).redis.GetDatabase();
                db.KeyDelete(typeof($safeitemrootname$).FullName);
            }
            catch (Exception ex) { Logger.log.Error("Failed to connect to redis for Bust()", ex); }
            $valuetype$Cache.Clear();
        }

        public static Dictionary<$keytype$, $valuetype$> QueryAll()
        {
            Dictionary <$keytype$, $valuetype$> results = new Dictionary<$keytype$, $valuetype$>();
            using (HostingEnvironment.Impersonate())
            {
                using (SqlConnection conn = DB.Qosi)
                {
                    try
                    {
                        using (SqlCommand fetch = new SqlCommand
                        {
                            Connection = conn,
                            CommandType = CommandType.Text,
                            CommandText = "$sqlstring$"
                        })
                        {
                            conn.Open();
                            SqlDataReader reader = fetch.ExecuteReader();
                            while (reader.Read())
                            {
                                        $valuetype$ next = new $valuetype$(reader);
                                results.Add(next.ID, next);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex);
                        throw ex;
                    }
                }
            }
            return results;
        }

        public static $valuetype$ Get($keytype$ id)
        {
            if (Cached().ContainsKey(id))
                return Cached()[id];
            return null;
        }

        public static bool RedisSet(Dictionary<$keytype$, $valuetype$> data)
        {
            bool ret = Redis.Set(typeof($safeitemrootname$).FullName, JsonConvert.SerializeObject(data));
            return ret;
        }

        public static Dictionary<$keytype$, $valuetype$> RedisGet()
        {
            Dictionary <$keytype$, $valuetype$> ret = null;
            var result = Redis.Get(typeof($safeitemrootname$).FullName);
            if (result.IsNullOrEmpty)
                return ret;
            ret = JsonConvert.DeserializeObject < Dictionary <$keytype$, $valuetype$>> (result);
            return ret;
        }

        Dictionary<$keytype$, $valuetype$> ICacheable<$keytype$, $valuetype$>.QueryAll()
        {
            return $safeitemrootname$.QueryAll();
        }

        void ICacheable<$keytype$, $valuetype$>.Bust()
        {
            $safeitemrootname$.Bust();
        }

        void ICacheable<$keytype$, $valuetype$>.Refresh()
        {
            $safeitemrootname$.Refresh();
        }

        Dictionary<$keytype$, $valuetype$> ICacheable<$keytype$, $valuetype$>.RedisGet()
        {
            return $safeitemrootname$.RedisGet();
        }

        bool ICacheable<$keytype$, $valuetype$>.RedisSet(Dictionary<$keytype$, $valuetype$> data)
        {
            return $safeitemrootname$.RedisSet(data);
        }

        Dictionary<$keytype$, $valuetype$> ICacheable<$keytype$, $valuetype$>.Cached()
        {
            return $safeitemrootname$.Cached();
        }

        public static void CheckPermission()
        {
            /*if (!QosiAuth.Is("SuperAdmin"))
            {
                throw new PermissionDeniedException("Only Authorized accounts may administrate $valuetype$.");
            }*/
        }

        public static $keytype$ Create($valuetype$ input)
        {
            CheckPermission();
            $keytype$ newId = -1;

            using (SqlConnection conn = DB.Qosi)
            {
                try
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        Connection = conn,
                        CommandType = System.Data.CommandType.Text,
                        CommandText = "INSERT INTO tbl_$valuetype$($valuetype$) " +
                        "VALUES(@$valuetype$);SELECT SCOPE_IDENTITY();"
                    })
                    {
                        command.Parameters.Add("@$valuetype$", System.Data.SqlDbType.DateTime).Value = input.Date;

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader(); // creations, updates, deletes do not get read, run as "non query"
                        if (reader.HasRows)
                        {
                            reader.Read();
                            var raw = reader.GetValue(0);
                            $keytype$.TryParse(raw.ToString(), out newId);
                            input.ID = newId;
                            Logger.Create(input);
                            Bust();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw ex;
                }
            }

            return newId;
        }

        public static void Update($keytype$ id, $valuetype$ input)
        {
            CheckPermission();
            $valuetype$ before = Get(id);
            if (before == null)
            {
                throw new RecordNotFoundException($"No $valuetype$ found with the supplied ID {id}.");
            }
            using (SqlConnection conn = DB.Qosi)
            {
                try
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        Connection = conn,
                        CommandType = System.Data.CommandType.Text,
                        CommandText = "UPDATE tbl_$valuetype$" +
                        " SET $valuetype$ = @$valuetype$ " +
                        " WHERE ID = @ID;"
                    })
                    {
                        command.Parameters.Add("@ID", System.Data.SqlDbType.$keytype$).Value = id;
                        command.Parameters.Add("@$valuetype$", System.Data.SqlDbType.DateTime).Value = input.Date;

                        conn.Open();
                        command.ExecuteNonQuery();
                        Logger.Change(before, input);
                        Bust();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw ex;
                }
            }
        }

        public static void Delete($keytype$ id)
        {
            CheckPermission();
            $valuetype$ before = Get(id);
            if (before == null)
            {
                throw new RecordNotFoundException($"No $valuetype$ found with the supplied ID {id}.");
            }

            using (SqlConnection conn = DB.Qosi)
            {
                try
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        Connection = conn,
                        CommandType = System.Data.CommandType.Text,
                        CommandText = "DELETE FROM tbl_$valuetype$" +
                        " WHERE ID = @ID;"
                    })
                    {
                        command.Parameters.Add("@ID", System.Data.SqlDbType.$keytype$).Value = id;

                        conn.Open();
                        command.ExecuteNonQuery();
                        Logger.Delete(before);
                        Bust();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    throw ex;
                }
            }
        }
}
}
