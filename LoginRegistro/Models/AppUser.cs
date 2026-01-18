//Importamos el espacio de nombre en el cual contiene los atributos de validación.
using System.ComponentModel.DataAnnotations;

//Esta linea sirve para poder organizar el código dentro del poryecto LoginRegistro; evitando conflictos y poder mantener el orden del proyecto
namespace LoginRegistro.Models;

//Creamos las AppUser la cual va a presentar a un usuario dentro del sistema. Con esta clase hacemos que Entity Framework se convierta en una tabla MySQL
public class AppUser
{
    //El ID será la clave primaria del usuario; en la base de datos este valor será autoincremental
    public int Id { get; set; }

    //El username (usuario) será un campo obligatorio y tendrá un máximo de 50 caracteres.
    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    //El PasswordHash almacenara los hash de las contraseñas cifrado; es obligatorio que siempre debe de existir un hash y no se permitan contraseñas vacías
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    //El DateTime almacenara la fecha de la creación de los usuarios; este dato se pondrá de manera automatica para crear el usuario
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}