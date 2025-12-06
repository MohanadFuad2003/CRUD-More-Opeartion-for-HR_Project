using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
namespace HRSYS.Infrastructure.Helpers
{

    public class SqlHelper
    {
        private readonly string _connectionString;

        public SqlHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("Connection string 'DefaultConnection' not found.");
        }

        public DataTable ExecuteDataTable(string storedProcedure, List<SqlParameter>? parameters = null)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(storedProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters.ToArray());

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public int ExecuteNonQuery(string storedProcedure, List<SqlParameter>? parameters = null)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(storedProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters.ToArray());

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string storedProcedure, List<SqlParameter>? parameters = null)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(storedProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters.ToArray());

            conn.Open();
            return cmd.ExecuteScalar();
        }
    }

}
