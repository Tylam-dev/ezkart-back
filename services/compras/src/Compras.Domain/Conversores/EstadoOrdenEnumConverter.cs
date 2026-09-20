using System.Text.Json;
using System.Text.Json.Serialization;
using Compras.Domain.Enums;

namespace Compras.Domain.Utilidades;

public class EstadoOrdenEnumConverter : JsonConverter<EstadoOrdenEnum>
{
    public override EstadoOrdenEnum Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        switch (value)
        {
            case "C":
                return EstadoOrdenEnum.Confirmada;

            case "X":
                return EstadoOrdenEnum.Cancelada;

            default:
                throw new JsonException($"Valor inválido para EstadoOrdenEnum: {value}");
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        EstadoOrdenEnum value,
        JsonSerializerOptions options)
    {
        switch (value)
        {
            case EstadoOrdenEnum.Confirmada:
                writer.WriteStringValue("C");
                break;

            case EstadoOrdenEnum.Cancelada:
                writer.WriteStringValue("X");
                break;

            default:
                throw new JsonException(
                    $"Valor inválido para EstadoOrdenEnum: {value}");
        }
    }
}