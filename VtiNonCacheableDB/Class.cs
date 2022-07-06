using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using WebApplicationQOSI.Libraries;
using $modelnamespace$;
using System.Web.Hosting;
using WebApplicationQOSI.Libraries.Exceptions;

namespace $rootnamespace$
{
	internal class $safeitemrootname$
	{
	    public static Dictionary<$keytype$, $valuetype$> Query(List<string> filters, string orderBy = "$orderbyvar$ DESC", int length = 50, int start = 0)
        {
            Dictionary<$keytype$, $valuetype$> results = new Dictionary<$keytype$, $valuetype$>();
            // replace this with the database connection you need, see the DB class
            SqlConnection conn = DB.$databasename$;

            string where = "";
            if (filters.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", filters);
            }

            // replace this with query that selects all RunSheet items
            string query = $"$selectsqlstring$";

            using (HostingEnvironment.Impersonate())
            {
                try
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandType = CommandType.Text,
                        Connection = conn,
                        CommandText = query
                    })
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            // passing reader to constructor will map the data to the object, this happens in ModelBase using reflection
                            $valuetype$ next = new $valuetype$(reader);
                            results.Add(next.$keyname$, next);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return results;
        }

        public static int GetFilteredCount(List<string> filters)
        {
            int result = 0;
            SqlConnection conn = DB.$databasename$;

            string where = "";
            if (filters.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", filters);
            }

            string query = $"$filteredsqlstring$";

            using (HostingEnvironment.Impersonate())
            {
                try
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandType = CommandType.Text,
                        Connection = conn,
                        CommandText = query
                    })
                    {
                        conn.Open();
                        result = (int)command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
                finally
                {
                    conn.Close();
                }
            }

            return result;
        }

        public static int GetTotal()
        {
            int result = 0;
            SqlConnection conn = DB.$databasename$;
            string query = "$totalsqlstring$";
            using (HostingEnvironment.Impersonate())
            {
                try
                {
                    using (SqlCommand command = new SqlCommand
                    {
                        CommandType = CommandType.Text,
                        Connection = conn,
                        CommandText = query
                    })
                    {
                        conn.Open();
                        result = (int)command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        public static $valuetype$ Get($keytype$ id)
        {
            Dictionary<$keytype$, $valuetype$> results = Query(new List<string>
                    {
                        $"x.$keyname$ = {id}",
                    });

            if (results.Count == 0)
            {
                throw new RecordNotFoundException("No $valuetype$ with the supplied Id.");
            }

            return results.First().Value;
        }
    }
}