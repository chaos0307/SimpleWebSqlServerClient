using System.Data;
using System.Data.SqlClient;

namespace SimpleWebSqlServerClient.Data
{
    public class AdoNetExec
    {
        private int _timeout = 30;
        private CommandType _commandType = CommandType.Text;
        public virtual DataSet GetDataSet(string connectionName, string query, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(connectionName))
            {
                using (var command = new SqlCommand(query, conn))
                {
                    command.CommandType = _commandType;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = _timeout;

                    var result = new DataSet();
                    var dataAdapter = new SqlDataAdapter(command);

                    dataAdapter.Fill(result);

                    return result;
                }
            }
        }

        public virtual void SetTimeOut(int timeout)
        {
            _timeout = timeout;
        }

        public virtual void SetCommandType(CommandType commandType)
        {
            _commandType = commandType;
        }
    }
}