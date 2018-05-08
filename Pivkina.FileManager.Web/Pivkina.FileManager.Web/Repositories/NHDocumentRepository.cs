using NHibernate;
using Pivkina.FileManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Pivkina.FileManager.Web.Repositories
{
    public class NHDocumentRepository : NHBaseRepository<Document>
    {
        public override ICriteria CreateNewCriteria(ISession session)
        {
            return session.CreateCriteria<Document>();
        }

        public override bool Save(Document obj)
        {
            if (obj.Id != 0)
            {
               return base.Save(obj);
            }
            else
            {
                var connectionString = ConfigurationManager.ConnectionStrings["FileBase"].ConnectionString;
                string sqlExpression = "CreateDoc";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    // указываем, что команда представляет хранимую процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = obj.Name ?? "Test"
                    };
                    // добавляем параметр
                    command.Parameters.Add(nameParam);

                    SqlParameter dateParam = new SqlParameter
                    {
                        ParameterName = "@creationDate",
                        Value = obj.CreationDate
                    };
                    command.Parameters.Add(dateParam);

                    SqlParameter authorParam = new SqlParameter
                    {
                        ParameterName = "@creationAuthor",
                        Value = obj.CreationAuthor != null ? obj.CreationAuthor.Id : 1
                    };
                    command.Parameters.Add(authorParam);

                    SqlParameter fileParam = new SqlParameter
                    {
                        ParameterName = "@binaryFile",
                        Value = obj.BinaryFile
                    };
                    command.Parameters.Add(fileParam);

                    return command.ExecuteNonQuery() != 0;
                }
            }
        }
    }
}