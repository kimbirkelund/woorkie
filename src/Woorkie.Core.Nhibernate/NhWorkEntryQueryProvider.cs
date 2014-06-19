using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace Woorkie.Core.Nhibernate
{
    public class NhWorkEntryQueryProvider : IQueryProvider
    {
        private readonly ISession _session;

        public NhWorkEntryQueryProvider(ISession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _session = session;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new NhWorkEntryQueryable(this, expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return (IQueryable<TElement>)CreateQuery(expression);
        }

        public object Execute(Expression expression)
        {
            return CreateQuery(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var rewrittenExpression = new ModelToNhExpressionRewriter(_session).Visit(expression);

            var result = _session.Query<WorkEntryEntity>().Provider.Execute(rewrittenExpression);

            return (TResult)TranslateResult(typeof(TResult), result);
        }

        private object TranslateResult(Type resultType, object result)
        {
            var nhProfile = result as ProfileEntity;
            if (nhProfile != null)
                return nhProfile.ToModelProfile(_session);

            var nhWorkEntry = result as WorkEntryEntity;
            if (nhWorkEntry != null)
                return nhWorkEntry.ToModelWOrkEntry(_session);

            if (resultType == typeof(string))
                return result;

            if (!resultType.IsGenericType)
                return result;

            if (resultType.GetGenericTypeDefinition() != typeof(IEnumerable<>))
                return result;

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(resultType.GetGenericArguments()[0]));
            foreach (var item in ((IEnumerable)result).Cast<object>()
                                                      .Select(v => TranslateResult(resultType.GetGenericArguments()[0], v)))
                list.Add(item);

            return list;
        }
    }
}
