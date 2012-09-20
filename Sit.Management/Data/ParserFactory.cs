using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Sit.Management.Data.Attributes;

namespace Sit.Management.Data
{
    public class ParserFactory
    {
        public static ParseIntoIDbCommand<TEntityContract> GetParserForIDbCommand<TEntityContract>()
        {
            var type = typeof (TEntityContract);

            if (!type.IsInterface)
            {
                throw new ArgumentException("Invalid Type passed as Generic. Must be an interface.");
            }

            var props = type.GetProperties();

            var commandExpr = Expression.Parameter(typeof (IDbCommand), "command");
            var commandExprProperty = Expression.Property(commandExpr, "Parameters");
            var dataExpr = Expression.Parameter(type, "data");

            var parametersExpressions = props
                .Select(prop =>
                            {
                                var attributes = prop.GetCustomAttributes(true);

                                var name = prop.Name;

                                var nameAttribute = attributes.OfType<DbFieldNameAttribute>().FirstOrDefault();

                                if (nameAttribute != null)
                                {
                                    name = nameAttribute.FieldName;
                                }

                                DbType dbType = prop.PropertyType.GetDbType();

                                int? size = null;

                                if (dbType == DbType.String)
                                {
                                    var length = attributes.OfType<MaxLengthAttribute>().FirstOrDefault();

                                    if (length != null)
                                    {
                                        size = length.MaxLength;
                                    }
                                }

                                var nameExpr = Expression.Constant(name);
                                var dbTypeExpr = Expression.Constant(dbType);
                                var sizeExpr = Expression.Constant(size);

                                var paramType = typeof (IDataParameter);

                                var propInfoName = paramType.GetProperty("ParameterName");
                                var propInfoValue = paramType.GetProperty("Value");
                                var propInfoDbType = paramType.GetProperty("DbType");


                                var paramExpr = Expression.Variable(typeof (IDbDataParameter), "parameter");

                                IEnumerable<Expression> expressions =
                                    new Expression[]
                                        {
                                            Expression.Assign(paramExpr, Expression.Call(commandExpr, "CreateParameter", null, null)),
                                            Expression.Call(Expression.TypeAs(commandExprProperty, typeof (IList)), "Add", null, paramExpr),
                                            Expression.Assign(Expression.Property(paramExpr, propInfoName), nameExpr),
                                            Expression.Assign(Expression.Property(paramExpr, propInfoDbType), dbTypeExpr),
                                            Expression.Assign(Expression.Property(paramExpr, propInfoValue), Expression.TypeAs(Expression.Property(dataExpr, prop.Name, null), typeof (Object)))
                                        };

                                if (size != null)
                                {
                                    expressions = expressions.Concat(new[]
                                                                         {
                                                                             Expression.Assign(Expression.Property(paramExpr, "Size", null), sizeExpr)
                                                                         });
                                }

                                return Expression.Block(new[] {paramExpr}, expressions);
                            });

            var code = Expression.Block(parametersExpressions);

            var compiled = Expression.Lambda<ParseIntoIDbCommand<TEntityContract>>(code, commandExpr, dataExpr).Compile();

            return compiled;
        }
    }
}