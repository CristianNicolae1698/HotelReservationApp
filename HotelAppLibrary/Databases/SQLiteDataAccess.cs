using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace HotelAppLibrary.Databases
{
    public class SQLiteDataAccess : ISQLiteDataAccess
    {
        private readonly IConfiguration _config;

        public SQLiteDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName,
            bool isStoredProcedure = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;

            if (isStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }


            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                //Query in Dapper means it returns data
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);


            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
                //Query in Dapper means it returns data

            }
        }
    }
}
