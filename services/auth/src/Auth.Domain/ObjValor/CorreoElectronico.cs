using System.Net.Mail;

namespace Auth.Domain.ObjValor;

public class CorreoElectronico
{
    public string Valor { get; set; } = null!;
    public bool ValidarFormato(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            return false;

        try
        {
            var direccion = new MailAddress(correo);

            return direccion.Address == correo.Trim();
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
