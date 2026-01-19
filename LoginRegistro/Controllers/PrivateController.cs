//Importamos Authorization que sirve para proteger las páginas y que solo entren usuarios que han sido logueados
using Microsoft.AspNetCore.Authorization;
//Importamos lo necesario para usar Controllers y devolver la vista
using Microsoft.AspNetCore.Mvc;

//Esta linea sirve para poder organizar el código dentro del poryecto LoginRegistro; evitando conflictos y poder mantener el orden del proyecto
namespace LoginRegistro.Controllers;

//Authorize significa que este controlador se encuentra protegido y solo podrán acceder los usuarios que hayan iniciado sesión
[Authorize]
public class PrivateController : Controller
{
    //Se devuelve la vista del Index.cshtml que se encuentra dentro de Views/Private
    public IActionResult Index()
    {
        return View();
    }
}