using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MFITest
{
    public class DAL
    {
        SqlConnection m_sqlCon;
        SqlCommand m_sqlCom;
        SqlTransaction m_sqlTran;
        /// <summary>
        /// Constructer with StrConnetion string 
        /// </summary>
        public DAL(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                m_sqlCon = new SqlConnection(connectionString);
                m_sqlCom = new SqlCommand();
            }
            else
            {
                throw new Exception("In DAL Constructor No Auguments were found");
            }
        }
        /// <summary>
        /// Function Executes the DML Query and returns the no of rows affected 
        /// </summary>
        /// <param name="queryString"> Query </param>
        /// <returns> No. of rows affected </returns>
        public int ExecuteNonQuery(string queryString)
        {
            m_sqlCom.CommandText = queryString;
            m_sqlCom.Connection = m_sqlCon;
            int iReturnValue = 0;
            try
            {
                m_sqlCon.Open();
                iReturnValue = m_sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (m_sqlCon.State == ConnectionState.Open)
                {
                    m_sqlCon.Close();
                }
            }
            return iReturnValue;
        }

        /// <summary>
        /// Function Executes the DML Stored Procedure and returns the no of rows affected 
        /// </summary>
        /// <param name="command"> Command </param>
        /// <returns> No. of rows affected </returns>
        public int ExecuteNonQuery(SqlCommand command)
        {
            command.Connection = m_sqlCon;
            int iReturnValue = 0;

            try
            {
                m_sqlCon.Open();
                iReturnValue = command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (m_sqlCon.State == ConnectionState.Open)
                {
                    m_sqlCon.Close();
                }
                command.Dispose();
            }
            return iReturnValue;
        }

        /// <summary>
        ///		Start Transaction for the Connection 
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                m_sqlCon.Open();
                m_sqlTran = m_sqlCon.BeginTransaction("DALTransaction");
                m_sqlCom.Transaction = m_sqlTran;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        ///		Start Transaction for the Connection with command 
        /// </summary>
        public void BeginTransaction(SqlCommand command)
        {
            try
            {
                m_sqlCon.Open();
                m_sqlTran = m_sqlCon.BeginTransaction("DALTransaction");
                command.Transaction = m_sqlTran;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Function Executes the DML Stored Procedure and returns the no of rows affected 
        /// </summary>
        /// <param name="command"> Command </param>
        /// <returns> No. of rows affected </returns>
        public int ExecuteNonQueryWithTransaction(SqlCommand command)
        {
            command.Connection = m_sqlCon;
            int iReturnValue = 0;
            try
            {
                iReturnValue = command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }

            return iReturnValue;
        }

        /// <summary>
        ///		Function Executes the DML Query with Transaction which already Begain and returns the no of rows affected 
        /// </summary>
        /// <param name="queryString">Query</param>
        /// <returns>no of rows affected </returns>
        public int ExecuteNonQueryWithTransaction(string queryString)
        {
            m_sqlCom.CommandText = queryString;
            m_sqlCom.Connection = m_sqlCon;
            int iReturnValue = 0;

            try
            {
                iReturnValue = m_sqlCom.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            return iReturnValue;
        }

        /// <summary>
        ///		Commit the Transaction 
        /// </summary>
        public void CommitTransaction()
        {
            if (m_sqlTran != null)
            {
                m_sqlTran.Commit();
                m_sqlTran.Dispose();
            }
            m_sqlTran = null;
            m_sqlCon.Close();
        }

        /// <summary>
        ///		Rollback the transaction 
        /// </summary>
        public void RollbackTransaction()
        {
            if (m_sqlTran != null)
            {
                m_sqlTran.Rollback();
                m_sqlTran.Dispose();
            }
            m_sqlTran = null;
            if (m_sqlCon.State == ConnectionState.Open)
                m_sqlCon.Close();
        }

        /// <summary>
        ///		Returns the dataset as per Query or Stored Procedure 
        /// </summary>
        /// <param name="queryStringOrProcedure">Query Or Stored Procedure Name</param>
        /// <param name="commandType">Mention the Query Or Stored Procedure</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string queryStringOrProcedure, CommandType commandType)
        {
            DataSet ds = null;
            if (!string.IsNullOrEmpty(queryStringOrProcedure))
            {
                if (m_sqlCon.State != ConnectionState.Open)
                {
                    using (SqlDataAdapter sqlDa = new SqlDataAdapter(queryStringOrProcedure, m_sqlCon))
                    {
                        sqlDa.SelectCommand.CommandType = commandType;
                        try
                        {
                            ds = new DataSet();
                            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;
                            sqlDa.Fill(ds);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("In DAL GetDataSet No Auguments were found");
            }
            return ds;
        }

        /// <summary>
        ///		Returns the dataset as per Command object Pass 
        /// </summary>	
        /// <param name="command"></param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(SqlCommand command)
        {
            DataSet ds = null;
            if (m_sqlCon.State != ConnectionState.Open)
            {
                command.Connection = m_sqlCon;
                command.CommandTimeout = 900;
                using (SqlDataAdapter sqlDa = new SqlDataAdapter(command))
                {
                    try
                    {
                        ds = new DataSet();
                        ds.Locale = System.Globalization.CultureInfo.InvariantCulture;
                        sqlDa.Fill(ds);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return ds;
        }
        /// <summary>
        /// Get Dataset in Transaction
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet GetDataSet(SqlCommand command, bool trans)
        {
            DataSet ds = null;

            command.Connection = m_sqlCon;
            command.CommandTimeout = 900;
            using (SqlDataAdapter sqlDa = new SqlDataAdapter(command))
            {
                try
                {
                    ds = new DataSet();
                    ds.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    sqlDa.Fill(ds);
                }
                catch
                {
                    throw;
                }
            }


            return ds;
        }
        /// <summary>
        ///		Returns the dataset as per Query 
        /// </summary>
        /// <param name="queryString">Query</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string queryString)
        {
            return this.GetDataSet(queryString, CommandType.Text);
        }

        /// <summary>
        ///		Returns the DataReader as per Query Close the DataReader after using that
        /// </summary>
        /// <param name="queryStringOrProcedure">Query Or Stored Procedure Name</param>
        /// <param name="commandType">Mention the Query Or Stored Procedure</param>
        /// <returns>DataReder</returns>
        public SqlDataReader ExecuteReader(string queryStringOrProcedure, CommandType commandType)
        {
            SqlDataReader dr;
            m_sqlCom.Connection = m_sqlCon;
            if (!string.IsNullOrEmpty(queryStringOrProcedure))
            {
                m_sqlCom.CommandText = queryStringOrProcedure;
                m_sqlCom.CommandType = commandType;
                m_sqlCom.CommandTimeout = 900;
                try
                {
                    m_sqlCon.Open();
                    // Connection closes When Sql reader will Close the Connection 
                    dr = m_sqlCom.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {
                    m_sqlCon.Close();
                    throw;
                }
            }
            else
            {
                throw new Exception("In DAL ExecuteReader No Auguments were found");
            }
            return dr;

        }

        /// <summary>
        ///		Returns the DataReader as per Query
        ///		Close the DataReader after using that
        /// </summary>
        /// <param name="queryString"> Query </param>
        /// <returns>DataReder</returns>
        public SqlDataReader ExecuteReader(string queryString)
        {
            return (this.ExecuteReader(queryString, CommandType.Text));
        }

        /// <summary>
        ///		Returns the DataReader as per Command object pass 
        ///		Close the DataReader after using that
        /// </summary>
        /// <param name="command"> Query </param>
        /// <returns>DataReder</returns>
        public SqlDataReader ExecuteReader(SqlCommand command)
        {
            SqlDataReader dr;
            command.Connection = m_sqlCon;
            try
            {
                m_sqlCon.Open();
                dr = m_sqlCom.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                m_sqlCon.Close();
                throw;
            }
            return dr;
        }

        /// <summary>
        ///		Returns true if Transaction is Begin
        /// </summary>
        public bool IsInTransaction
        {
            get
            {
                if (m_sqlTran == null)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        ///		Returns the Single value of first row and first column  
        /// </summary>
        /// <param name="command">strQuery</param>
        /// <returns>object</returns>
        public object ExecuteScalar(SqlCommand command)
        {
            object returnValue = 0;
            try
            {
                command.Connection = m_sqlCon;
                if (m_sqlCon.State == ConnectionState.Closed || m_sqlCon.State == ConnectionState.Broken)
                    m_sqlCon.Open();
                returnValue = command.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (m_sqlCon.State != ConnectionState.Closed && !IsInTransaction)
                    m_sqlCon.Close();
            }
            return returnValue;

        }

        /// <summary>
        ///		Returns the Single value of first row and first column  
        /// </summary>
        /// <param name="queryString">strQuery</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string queryString)
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                m_sqlCom.CommandText = queryString;
                m_sqlCom.Connection = m_sqlCon;

                object returnValue = 0;
                try
                {
                    if (m_sqlCon.State == ConnectionState.Closed || m_sqlCon.State == ConnectionState.Broken)
                        m_sqlCon.Open();
                    returnValue = m_sqlCom.ExecuteScalar();
                    return returnValue;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (m_sqlCon.State != ConnectionState.Closed && !IsInTransaction)
                        m_sqlCon.Close();
                }
            }
            else
            {
                throw new Exception("In DAL ExecuteScalar No Auguments were found");
            }
        }
        /// <summary>
        ///  Destructor
        /// </summary>
        ~DAL()
        {
            if (m_sqlCon != null)
            {
                if (m_sqlCon.State == ConnectionState.Open)
                    m_sqlCon.Close();
                m_sqlCon.Dispose();
            }
            if (m_sqlCom != null)
                m_sqlCom.Dispose();
            if (m_sqlTran != null)
                m_sqlTran.Dispose();

        }
    }
}
