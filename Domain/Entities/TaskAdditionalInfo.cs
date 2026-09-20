using System.Text.Json;
using System.Text.Json.Nodes;
using Domain.Exceptions;

namespace Domain.Entities
{
    public sealed record TaskAdditionalInfoSnapshot(string? Priority, IReadOnlyList<string> Tags);

    public static class TaskAdditionalInfo
    {
        public const string PriorityPropertyName = "prioridad";
        public const string TagsPropertyName = "etiquetas";

        public static void EnsureValidJson(string? additionalInfoJson)
        {
            if (string.IsNullOrWhiteSpace(additionalInfoJson))
            {
                return;
            }

            _ = ParseObject(additionalInfoJson);
        }

        public static TaskAdditionalInfoSnapshot Parse(string? additionalInfoJson)
        {
            if (string.IsNullOrWhiteSpace(additionalInfoJson))
            {
                return new TaskAdditionalInfoSnapshot(null, []);
            }

            var jsonObject = ParseObject(additionalInfoJson);
            var priority = jsonObject[PriorityPropertyName]?.GetValue<string?>();
            var tags = jsonObject[TagsPropertyName] is JsonArray jsonArray
                ? jsonArray
                    .Select(node => node?.GetValue<string>())
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Select(value => value!)
                    .ToArray()
                : [];

            return new TaskAdditionalInfoSnapshot(priority, tags);
        }

        public static string? MergePriorityAndTags(string? additionalInfoJson, string? priority, IEnumerable<string>? tags)
        {
            if (string.IsNullOrWhiteSpace(additionalInfoJson) && priority is null && tags is null)
            {
                return null;
            }

            var jsonObject = string.IsNullOrWhiteSpace(additionalInfoJson)
                ? []
                : ParseObject(additionalInfoJson);

            if (priority is not null)
            {
                jsonObject[PriorityPropertyName] = priority;
            }

            if (tags is not null)
            {
                jsonObject[TagsPropertyName] = new JsonArray([.. tags.Select(tag => JsonValue.Create(tag))]);
            }

            return jsonObject.ToJsonString();
        }

        public static string SetPriority(string? additionalInfoJson, string priority)
        {
            var jsonObject = string.IsNullOrWhiteSpace(additionalInfoJson)
                ? []
                : ParseObject(additionalInfoJson);

            jsonObject[PriorityPropertyName] = priority;

            return jsonObject.ToJsonString();
        }

        private static JsonObject ParseObject(string additionalInfoJson)
        {
            try
            {
                return JsonNode.Parse(additionalInfoJson) as JsonObject
                    ?? throw new ValidationException("La información adicional de la tarea debe ser un objeto JSON válido");
            }
            catch (JsonException)
            {
                throw new ValidationException("La información adicional de la tarea debe ser un JSON válido");
            }
        }
    }
}
