using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate;
using NHibernate.Linq;

namespace Woorkie.Core.Nhibernate
{
    public class ModelToNhExpressionRewriter : ExpressionVisitor
    {
        private readonly ISession _session;

        public ModelToNhExpressionRewriter(ISession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _session = session;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var profile = node.Value as IProfile;
            if (profile != null)
                return Expression.Constant(GetNhProfile(profile));
            var workEntry = node.Value as IWorkEntry;
            if (workEntry != null)
                return Expression.Constant(GetNhWorkEntry(workEntry));
            if (node.Value is NhWorkEntryQueryable)
                return Expression.Constant(_session.Query<NhWorkEntry>());

            return base.VisitConstant(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var body = Visit(node.Body);
            var parameters = Visit(node.Parameters, pe => (ParameterExpression)Visit(pe));

            return Expression.Lambda(body, parameters);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var operand = Visit(node.Expression);

            MemberInfo member;
            if (operand.Type == typeof(NhWorkEntry) || operand.Type == typeof(NhProfile))
                member = operand.Type.GetMember(node.Member.Name).Single();
            else
                member = node.Member;

            var newNode = Expression.MakeMemberAccess(operand, member);

            if (newNode.Expression.NodeType != ExpressionType.Constant)
                return newNode;

            ConstantExpression newConstant;

            var value = ((ConstantExpression)newNode.Expression).Value;
            switch (newNode.Member.MemberType)
            {
                case MemberTypes.Field:
                    var fieldInfo = (FieldInfo)newNode.Member;
                    newConstant = Expression.Constant(fieldInfo.GetValue(value));
                    break;
                case MemberTypes.Property:
                    var propertynfo = (PropertyInfo)newNode.Member;
                    newConstant = Expression.Constant(propertynfo.GetValue(value));
                    break;
                default:
                    return newNode;
            }

            return Visit(newConstant);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var obj = Visit(node.Object);
            var arguments = Visit(node.Arguments);
            var method = node.Method;

            if (method.IsGenericMethod)
            {
                method = method.GetGenericMethodDefinition()
                               .MakeGenericMethod(method.GetGenericArguments()
                                                        .Select(TranslateType)
                                                        .ToArray());
            }
            if (method.Name == "Equals")
                return Expression.Equal(obj, arguments.Single());

            return Expression.Call(obj, method, arguments);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == typeof(IWorkEntry))
                return Expression.Parameter(typeof(NhWorkEntry), node.Name);
            if (node.Type == typeof(IProfile))
                return Expression.Parameter(typeof(NhProfile), node.Name);

            return node;
        }

        private NhProfile GetNhProfile(IProfile profile)
        {
            using (_session.BeginTransaction())
            {
                return _session.Query<NhProfile>()
                               .Single(p => p.Name == profile.Name);
            }
        }

        private NhWorkEntry GetNhWorkEntry(IWorkEntry workEntry)
        {
            using (_session.BeginTransaction())
            {
                return _session.Query<NhWorkEntry>()
                               .Single(p => p.Id == workEntry.Id);
            }
        }

        private Type TranslateType(Type type)
        {
            if (type == typeof(IProfile))
                return typeof(NhProfile);
            if (type == typeof(IWorkEntry))
                return typeof(NhWorkEntry);

            if (type.IsGenericType)
            {
                return type.GetGenericTypeDefinition()
                           .MakeGenericType(type.GetGenericArguments()
                                                .Select(TranslateType)
                                                .ToArray());
            }

            return type;
        }
    }
}
