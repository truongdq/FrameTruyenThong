using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Ecotek.Common
{
    public static class SqlManager
    {
        private static Database _db;

        static SqlManager()
        {
            try
            {
                string connectionStr = Utils.Decode(ConfigurationManager.ConnectionStrings["SQL_CONNECTION"].ConnectionString);
                _db = new SqlDatabase(connectionStr);
            }
            catch (Exception ex)
            {
                FileHelper.WriteLogError("", ex.Message + Environment.NewLine + ex.StackTrace);
                throw new Exception("CreateDatabase : " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách đối tượng
        /// </summary>
        public static List<T> ReadEntityList<T>(string sql, CommandType typeCommand = CommandType.StoredProcedure, IDataParameter[] parms = null) where T : new()
        {
            using (DbCommand command = typeCommand == CommandType.StoredProcedure ? _db.GetStoredProcCommand(sql) : _db.GetSqlStringCommand(sql))
            {
                command.SetParameters(parms);
                var reader = _db.ExecuteReader(command);
                return ReadEntityListByReader<T>(reader);
            }
        }

        /// <summary>
        /// Lấy một đối tượng
        /// </summary>
        public static object GetScalar(string sql, CommandType typeCommand = CommandType.StoredProcedure, IDataParameter[] parms = null)
        {
            using (DbCommand command = typeCommand == CommandType.StoredProcedure ? _db.GetStoredProcCommand(sql) : _db.GetSqlStringCommand(sql))
            {
                command.SetParameters(parms);
                return _db.ExecuteScalar(command);
            }
        }

        public static int ExcuteCommand(string sql, IDataParameter[] parms = null, bool output = false)
        {
            int result = 0;
            using (DbCommand command = _db.GetSqlStringCommand(sql))
            {
                try
                {
                    command.SetParameters(parms);
                    result = _db.ExecuteNonQuery(command);
                    if (output)
                        for (int i = 0; i < parms.Length; i++)
                        {
                            if (parms[i].Direction == ParameterDirection.Output)
                                parms[i].Value = _db.GetParameterValue(command, parms[i].ParameterName);
                        }
                }
                catch (Exception ex)
                {
                    FileHelper.WriteLogError("", ex.Message + Environment.NewLine + ex.StackTrace);
                    return result;
                }
            }

            return result;
        }

        private static void SetParameters(this DbCommand command, IDataParameter[] parms)
        {
            if (parms != null && parms.Length > 0)
            {
                foreach (IDataParameter pa in parms)
                {
                    object value = pa.Value ?? DBNull.Value;

                    if (pa.Direction == ParameterDirection.Output)
                        _db.AddOutParameter(command, pa.ParameterName, pa.DbType, Int32.MaxValue);
                    else
                    {
                        if (value.GetType().Name == "DataTable")
                        {
                            command.Parameters.Add(pa);
                        }
                        else
                        {
                            _db.AddInParameter(command, pa.ParameterName, pa.DbType, value);
                        }
                    }
                }
            }
        }

        private static List<T> ReadEntityListByReader<T>(IDataReader reader) where T : new()
        {
            var listT = new List<T>();
            using (reader)
            {
                while (reader.Read())
                {
                    var fileNames = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        fileNames.Add(reader.GetName(i));
                    }
                    var inst = new T();
                    foreach (var pi in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (!fileNames.Exists(fileName => string.Compare(fileName, pi.Name, StringComparison.OrdinalIgnoreCase) == 0))
                            continue;
                        object obj;
                        try
                        {
                            obj = reader[pi.Name];
                        }
                        catch
                        {
                            continue;
                        }

                        if (obj == DBNull.Value || obj == null)
                            continue;
                        var si = pi.GetSetMethod();
                        if (si == null)
                            continue;
                        if (pi.PropertyType == typeof(bool?))
                            pi.SetValue(inst, Convert.ToBoolean(obj), null);
                        else if (pi.PropertyType == typeof(string))
                            pi.SetValue(inst, obj.ToString(), null);
                        else if (pi.PropertyType == typeof(Int32))
                            pi.SetValue(inst, Convert.ToInt32(obj), null);
                        else if (pi.PropertyType == typeof(Int64))
                            pi.SetValue(inst, Convert.ToInt64(obj), null);
                        else if (pi.PropertyType == typeof(decimal))
                            pi.SetValue(inst, Convert.ToDecimal(obj), null);
                        else
                            pi.SetValue(inst, obj, null);
                    }
                    listT.Add(inst);
                }
            }
            return listT;
        }
    }
}
