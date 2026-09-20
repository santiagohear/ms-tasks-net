namespace Infrastructure.DataSource.StoreProcedures
{
    public record StoreProcedureResult<T>
    {
        public IEnumerable<T> Items { get; }
        public Dictionary<string, object> OutputValues { get; }

        public StoreProcedureResult(IEnumerable<T> items, Dictionary<string, object> outputValues)
        {
            Items = items;
            OutputValues = outputValues;
        }

        public StoreProcedureResult(IEnumerable<T> items)
        {
            Items = items;
            OutputValues = [];
        }
    }

    public record StoreProcedureResult(Dictionary<string, object> OutputValues);
}
