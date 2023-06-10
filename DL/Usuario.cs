using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }
    //esta sera la modificada para el casteo
    //public string? Contraseña { get; set; }

    // esta es la original, la que se crea cuando se hace el scaffold
    public byte[]? Contraseña { get; set; }
}
