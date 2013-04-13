using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Utils
{
    public static class SqlHelper
    {
        public static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["SpotiChelasCon"].ConnectionString;


        public static DataSet ExecSp(SqlCommand cmd)
        {
            var da = new SqlDataAdapter {SelectCommand = cmd};
            var ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
    }
}