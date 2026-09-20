namespace Inventario.Application.Utilidades;

public class Resultado<T>
{
    public bool Exitoso { get; set; }
    public T? Valor { get; set; }
    public string? MensajeError { get; set; }
    public Exception? Excepcion { get; set; }

    public static Resultado<T> Exito(T valor)
    {
        return new Resultado<T> { Exitoso = true, Valor = valor };
    }

    public static Resultado<T> Error(string mensajeError, Exception? ex)
    {
        return new Resultado<T> { Exitoso = false, MensajeError = mensajeError, Excepcion = ex};
    }
}