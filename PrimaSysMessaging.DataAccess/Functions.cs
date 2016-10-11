using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using BusinessLogic;

namespace PrimaSysMessaging.BusinessLogic
{
    public class Functions
    {
        static Database  con = null;

        static Functions()
        {
            con = new DatabaseProviderFactory().Create (CookiesWrapper.thisConnectionName);
        }

        public static DataTable ExecuteSelectCommand(string CommandName)
        {
            System.Data.Common.DbCommand cmd = con.GetStoredProcCommand(CommandName);
            DataSet ds;

            try
            {
                
               ds = con.ExecuteDataSet(cmd);

            }
            catch (Exception ex)
            {
                throw;
            }

            return ds.Tables[0];
        }

        public static DataTable ExecuteParamerizedSelectCommand(string CommandName, SqlParameter[] param)
        {
            System.Data.Common.DbCommand cmd = con.GetStoredProcCommand(CommandName);
            DataTable table = new DataTable();

            cmd.Parameters.AddRange(param);

            try
            {

                table = con.ExecuteDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }

            return table;
        }

        public static bool ExecuteNonQuery(string CommandName, SqlParameter[] pars)
        {
            System.Data.Common.DbCommand cmd = con.GetStoredProcCommand(CommandName);
            int res = 0;

            cmd.Parameters.AddRange(pars);

            try
            {

                res = con.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw;
            }

            if (res >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
