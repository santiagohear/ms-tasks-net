using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.DataSource
{
    public static class JsonDbFunctions
    {
        [DbFunction("JSON_VALUE", IsBuiltIn = true)]
        public static string? JsonValue(string? expression, [NotParameterized] string path)
        {
            throw new NotSupportedException();
        }
    }
}
