using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Woorkie.Core.Nhibernate
{
    public class NhWorkEntryQueryable : IQueryable<IWorkEntry>
    {
        public Type ElementType { get; private set; }
        public Expression Expression { get; private set; }
        public IQueryProvider Provider { get; private set; }

        public NhWorkEntryQueryable(IQueryProvider provider, Expression expression)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            ElementType = typeof(IWorkEntry);
            Provider = provider;
            Expression = expression ?? Expression.Constant(this);
        }

        public IEnumerator<IWorkEntry> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<IWorkEntry>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
