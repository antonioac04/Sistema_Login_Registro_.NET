//Importamos DataAnnotations; esto sirve para validar los campos que indiquemos
using System.ComponentModel.DataAnnotations;

//Esta linea sirve para poder organizar el código dentro del poryecto LoginRegistro; evitando conflictos y poder mantener el orden del proyecto
namespace LoginRegistro.Models;

//Clase en la cual representa los datos del formulario de registro; esto solo sirve para el formulario
public class RegisterVm
{
    //La variable Username es obligatoria y tiene un mínimo 3 caracteres y un máximo de 50 caracteres.
    [Required, StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    //La variable Password es obligatoria y tiene un mínimo 8 caracteres y un máximo de 100 caracteres; para así poner contraseñas más seguras
    [Required, StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    //La variable ConfirmPassword es obligatoria y tiene un mínimo 8 caracteres y un máximo de 100 caracteres; para así poner contraseñas más seguras
    //Esta variable se pone para validar la contraseña si la contraseña de arriba con esta es igual son validas
    [Required, Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}