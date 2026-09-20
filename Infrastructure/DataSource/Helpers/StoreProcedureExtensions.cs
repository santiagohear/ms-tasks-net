using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace Infrastructure.DataSource.Helpers
{
    internal static class StoreProcedureExtensions
    {
        internal static Tuple<string, List<SqlParameter>> AsStoreProcedure<T>(this object instance, string storeProcedureName)
        {
            var definition = Activator.CreateInstance<T>();
            var definitionType = typeof(T);
            var definitionProperties = definitionType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var instanceType = instance.GetType();
            var instanceProperties = instanceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var parameters = new List<SqlParameter>();

            foreach (var definitionProperty in definitionProperties)
            {
                var attribute = definitionProperty.GetCustomAttribute<SqlParameterAttribute>();

                if (attribute is null)
                {
                    continue;
                }

                var instanceProperty = instanceProperties.FirstOrDefault(x => x.Name == definitionProperty.Name);
                var value = definitionProperty.GetValue(definition);

                if (instanceProperty is not null)
                {
                    value = instanceProperty.GetValue(instance);
                }

                SqlParameter parameter;

                if (attribute.Type.HasValue)
                {
                    parameter = new SqlParameter($"@{attribute.Name}", attribute.Type.Value)
                    {
                        Direction = attribute.Direction,
                        Value = value ?? DBNull.Value
                    };
                }
                else
                {
                    parameter = new SqlParameter($"@{attribute.Name}", value ?? DBNull.Value)
                    {
                        Direction = attribute.Direction
                    };
                }

                parameters.Add(parameter);
            }

            var query = $"{storeProcedureName} {string.Join(", ", parameters.Select(x => $"{x.ParameterName} {x.DirectionToString()}"))}";

            return new Tuple<string, List<SqlParameter>>(query, parameters);
        }

        internal static Tuple<string, List<SqlParameter>> AsStoreProcedure<T>(this T instance, string storeProcedureName)
        {
            var definitionType = typeof(T);
            var definitionProperties = definitionType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var parameters = new List<SqlParameter>();

            foreach (var definitionProperty in definitionProperties)
            {
                var value = definitionProperty.GetValue(instance);
                var attribute = definitionProperty.GetCustomAttribute<SqlParameterAttribute>();

                if (attribute is null)
                {
                    continue;
                }

                SqlParameter parameter;

                if (attribute.Type.HasValue)
                {
                    parameter = new SqlParameter($"@{attribute.Name}", attribute.Type.Value)
                    {
                        Direction = attribute.Direction,
                        Value = value ?? DBNull.Value
                    };
                }
                else
                {
                    parameter = new SqlParameter($"@{attribute.Name}", value ?? DBNull.Value)
                    {
                        Direction = attribute.Direction
                    };
                }

                parameters.Add(parameter);
            }

            var query = $"{storeProcedureName} {string.Join(", ", parameters.Select(x => $"{x.ParameterName} {x.DirectionToString()}"))}";

            return new Tuple<string, List<SqlParameter>>(query, parameters);
        }

        internal static string DirectionToString(this SqlParameter parameter)
        {
            return parameter?.Direction == ParameterDirection.Output ? parameter.Direction.ToString() : string.Empty;
        }

        internal static Dictionary<string, object> GetOutputValues<T>(this List<SqlParameter> parameters)
        {
            var definitionType = typeof(T);
            var definitionProperties = definitionType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var outputValues = parameters.Where(p => p.Direction == ParameterDirection.Output)
                .ToDictionary(p => GetPropertyNameByParameterName(p.ParameterName), p => p.Value);

            string GetPropertyNameByParameterName(string parameterName)
            {
                var fixedParameterName = parameterName.Replace("@", string.Empty);
                return definitionProperties!.FirstOrDefault(p => p.GetCustomAttribute<SqlParameterAttribute>()?.Name == fixedParameterName)?.Name ?? fixedParameterName;
            }

            return outputValues;
        }

        internal static T? GetValue<T>(this Dictionary<string, object> outputValues, string key)
        {
            return outputValues.TryGetValue(key, out var value) ? value is T newValue ? newValue : default : default;
        }
    }
}
