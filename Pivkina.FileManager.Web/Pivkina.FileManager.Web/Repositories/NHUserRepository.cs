using NHibernate;
using NHibernate.Criterion;
using Pivkina.FileManager.Web.Interfaces;
using Pivkina.FileManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Security;

namespace Pivkina.FileManager.Web.Repositories
{
    public class NHUserRepository : NHBaseRepository<User>
    {
        public override ICriteria CreateNewCriteria(ISession session)
        {
            return session.CreateCriteria<User>();
        }

        public User LoadByLogin(string login)
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            var lst1 = Find(null);
            var lst = session.QueryOver<User>()
                    .And(op => op.Login == login).List();
            return session.CreateCriteria<User>()
                .Add(Restrictions.Eq("Login", $"{login}"))
                .UniqueResult<User>();
        }

        public bool Create(User obj)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["FileBase"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                String query = @"INSERT INTO [User] (Login, Name, Password) VALUES (@Login, @Name, @Password)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddRange(InverseMap(obj));

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }

        }

        private SqlParameter[] InverseMap(User obj)
        {
            var parameters = new List<SqlParameter>();

            var properties = typeof(User).GetProperties();

            foreach (var property in properties)
            {
                var val = property.GetValue(obj);
                parameters.Add(new SqlParameter($"@{property.Name}", val));
            }

            return parameters.ToArray();
        }

       

    }
}