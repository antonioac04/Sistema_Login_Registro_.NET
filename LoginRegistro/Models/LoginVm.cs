//Importamos DataAnnotations para poder usar validaciones como pueede ser dato obligatorio
using System.ComponentModel.DataAnnotations;
//Esta linea sirve para poder organizar el código dentro del poryecto LoginRegistro; evitando conflictos y poder mantener el orden del proyecto
namespace LoginRegistro.Models;
//Clase que va a recoger los datos del formulario de login; estos no se almacenan en la base de datos y solo se usa para verlo en el login.
public class LoginVm
{
    //Variable Username obligatoria.
    [Required]
    public string Username { get; set; } = string.Empty;
    //Variable Username obligatoria.
    [Required]
    public string Password { get; set; } = string.Empty;
    //Añadimos este campo el cual sirve para que el usuario pueda marcar si el inicio de sesión quiere que se recuerde
    public bool RememberMe { get; set; }
}