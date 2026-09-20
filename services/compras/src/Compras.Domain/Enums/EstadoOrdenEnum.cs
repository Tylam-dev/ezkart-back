using System.Text.Json.Serialization;
using Compras.Domain.Utilidades;

namespace Compras.Domain.Enums;
[JsonConverter(typeof(EstadoOrdenEnumConverter))]
public enum EstadoOrdenEnum
{
    Confirmada = 'C',
    Cancelada = 'X'
}
