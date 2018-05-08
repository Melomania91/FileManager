using NHibernate;
using NHibernate.Criterion;
using Pivkina.FileManager.Web.Interfaces;
using Pivkina.FileManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pivkina.FileManager.Web.Repositories
{
    public class NHBaseRepository<T> : IRepository<T>
        where T : IEntity
    {
        public bool Delete(T obj)
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            var queryString = string.Format("delete {0} where id = :id",
                                        typeof(T));
            session.CreateQuery(queryString)
                   .SetParameter("id", obj.Id)
                   .ExecuteUpdate();
            return true;
        }

        public virtual ICriteria CreateNewCriteria(ISession session)
        {
            return session.CreateCriteria<object>();
        }

        public IEnumerable<T> Find(string[,] condition)
        {
            ISession session = NHibernateHelper.GetCurrentSession();

            var criteria = CreateNewCriteria(session);

            if (condition != null && condition.Length != 0)
            {
                for (int i = 0; i < condition.Length/2; i++)
                {
                    criteria.Add(Restrictions.Eq(condition[i, 0], condition[i, 1]));
                }
            }

            return criteria.List<T>();
        }

        public T Load(int id)
        {
            ISession session = NHibernateHelper.GetCurrentSession();

            return session.Load<T>(id);
        }

        public virtual bool Save(T obj)
        {
            ISession session = NHibernateHelper.GetCurrentSession();

            using (ITransaction tx = session.BeginTransaction())
            {
                session.SaveOrUpdate(obj);
                tx.Commit();
            }

            return true;
        }
    }
}