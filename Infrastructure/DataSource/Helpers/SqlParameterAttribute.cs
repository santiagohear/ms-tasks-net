using System.Data;

namespace Infrastructure.DataSource.Helpers
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    internal sealed class SqlParameterAttribute : Attribute
    {
        public string Name { get; }

        public SqlDbType? Type { get; }

        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

        public SqlParameterAttribute(string name)
        {
            Name = name;
        }

        public SqlParameterAttribute(string name, SqlDbType type)
        {
            Name = name;
            Type = type;
        }
    }
}
