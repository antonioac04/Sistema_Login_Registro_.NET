//Importamos el DbContext que es la conexión a la base de datos.
using LoginRegistro.Data;
//Importamos los modelos del proyecto que son AppUser y RegisterVm
using LoginRegistro.Models;
//Importamos lo que es necesario para usar Controllers en ASP.NET MVC
using Microsoft.AspNetCore.Mvc;
//Importamos EntityFrameworkCore en el cual lo usaremos para los métodos AnyAsync o SaveChangesAsync
using Microsoft.EntityFrameworkCore;
//Importamos la librerías de criptografía para realizar el hash de contraseñas
using System.Security.Cryptography;
//Importamos la librerías de autenticación y también la de autenticación y cookies
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
//Importamos Claims; el cual sirve para guardar información del usuario logueado
using System.Security.Claims;

//Esta linea sirve para poder organizar el código dentro del poryecto LoginRegistro; evitando conflictos y poder mantener el orden del proyecto
namespace LoginRegistro.Controllers;

//Clase Controller que sera la encargada de las acciones que se realicen con las cuentas
public class AccountController : Controller
{
    //Variable privada en la cual almacena el acceso a la base de datos.
    private readonly AppDbContext _db;
    //Constructor que se ejecuta al crear el controlador; este recibe el AppDbContext para poder usar la base de datos
    public AccountController(AppDbContext db)
    {
        _db = db;
    }

    //Este método se muestra cuando indicamos /Account/Register y con el metodo Get lo que estamos haciendo es mostrar el formulario del registro
    [HttpGet]
    public IActionResult Register()
    {
        //Devuelve la vista Register.cshtml
        return View();
    }

    //Este método se ejecuta cuando se pulsa el botón del formulario y con el metodo Post; lo que realizamos es procesar el formulario de registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        //Primero validamos de manera automatica el formulario; se comprueba si el modelo cumple con los requisitos que hemos indicado.
        if (!ModelState.IsValid)
            return View(vm);

        //Segundo se comprueba si ya existe el usuario en la base de datos
        var exists = await _db.Users.AnyAsync(u => u.Username == vm.Username);
        //Si existe muestra un mensaje indicando "Ese nombre de usuario ya existe"
        if (exists)
        {
            ModelState.AddModelError(nameof(vm.Username), "Ese nombre de usuario ya existe.");
            return View(vm);
        }

        //Tercero creamos el usuario nuevo con los datos que se nos ha proporcionado en el formulario; en la cual hasheamos la contraseña en la 
        //base de datos, etc
        var user = new AppUser
        {
            Username = vm.Username,
            PasswordHash = HashPassword(vm.Password),
            CreatedAtUtc = DateTime.UtcNow
        };

        //Guardamos el usuario en la base de datos y guardamos los cambios
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        //Mostramos un mensaje indicando que el registro se ha completado y ya se puede iniciar sesión
        TempData["Msg"] = "Registro completado. Ya puedes iniciar sesión.";
        //Una vez que el usuario ya se encuentra creado redirigimos al usuario a la página de Login
        return RedirectToAction("Login");
    }

    //El metodo Get lo que estamos haciendo es mostrar la vista del login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    //El metodo POST procesa el formulario cuando el usuario procede a entrar con su cuenta
    [HttpPost]
    //Se protege el formulario contra ataques CSRF
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        //Primero comprobamos si el modelo del formulario es decir se han introducido todos los campos que son required
        if (!ModelState.IsValid)
            return View(vm);
        //Segundo se busca en la base de datos el usuario que hemos introducido para más adelante comprobar si existe
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == vm.Username);

        //Tercero; tal y como se ha comentado anteriormente se comprueba si el usuario existe y si la contraseña es correcta; en caso de
        //que no suceda muestra un mensaje de error diciendo que las credenciales son inválidas
        if (user == null || !VerifyPassword(vm.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
            return View(vm);
        }

        //Cuarto si el login es correcto, se crean los claims que son datos que identifican al usuario dentro de la sesión
        var claims = new List<Claim>
        {
            //Se guarda el Id del usuario como identificador principal
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //Se guarda el nombre de usuario
            new Claim(ClaimTypes.Name, user.Username)
        };
        //Quinto, se crea la identidad del usuario con el esquema de cookies
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //Sexto se crea la variable principal, que representa al usuario que se encuentra logueado en el sistema.
        var principal = new ClaimsPrincipal(identity);

        //Septimo se configura como se va a guardar la cookie de login
        var authProps = new AuthenticationProperties
        {
            IsPersistent = vm.RememberMe,
            ExpiresUtc = vm.RememberMe
                ? DateTimeOffset.UtcNow.AddDays(7)
                : DateTimeOffset.UtcNow.AddMinutes(30)
        };
        //Octavo se inicia la sesión y se crea la cookie de autenticación necesaria
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);
        //Si el login es correcto redirigimos al Home/Index
        return RedirectToAction("Index", "Home");
    }

    //Método el cual se ejecuta cuando el usuario cierra sesión
    [HttpPost]
    //Token de seguridad para evitar ataques CSRF
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        //Se cierra la sesión del usuario eliminado. SignOutAsync borra la cookie en la cual se creó cuando se hizo el login
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //Cuando se cierra la sesión redirigimos al usuario al inicio de la web
        return RedirectToAction("Index", "Home");
    }

    //Metodo privado en la cual se comprueba si la contraseña escrita en el login es la misma que hay en la base de datos
    private static bool VerifyPassword(string password, string storedHash)
    {
        //Lo primero que realizamos es separar el texto guardado usando el punto "."
        var parts = storedHash.Split('.');
        //En caso de que no tenga las dos partes, esto significa que se encuentra mál guardado
        if (parts.Length != 2) return false;
        //Segundo se convierte la salt de Base64 a bytes
        var salt = Convert.FromBase64String(parts[0]);
        //Tercero se convierte el hash guardadode Base64 a bytes
        var hashStored = Convert.FromBase64String(parts[1]);
        //Cuarto calculamos el hash de la contraseñaque el usuario ha escrito; a parte usamos el mismo salt y el mismo método PBKDF2
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        var hashComputed = pbkdf2.GetBytes(32);
        //Por ultimo se compara el hash guardado con el hash que se ha calculado
        return CryptographicOperations.FixedTimeEquals(hashStored, hashComputed);
    }

    //Método privado el cual se usa para crear un hash seguro de la contraseña, haciendo que no se almacene en texto plano.
    private static string HashPassword(string password)
    {
        //Generamos una cadena aleatoria que se añade a la contraseña; haciendo que si dos usuarios tienen la misma contraseña, el hash sea diferente
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        //Generamos el hash usando PBKDF2, este es un método seguro y recomendado
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        //Guardamos la contraseña hasheada en formato salt.hash
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
}