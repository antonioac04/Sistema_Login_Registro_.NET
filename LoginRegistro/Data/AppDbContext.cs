//Importamos la clase AppUser que se encuentra en la carpeta Models
using LoginRegistro.Models;
//Importamos Entity Framework Core para así poder usar la conexión con la base de datos, los set y el modelo constructor.
using Microsoft.EntityFrameworkCore;

//Esta linea sirve para poder organizar el código dentro del poryecto LoginRegistro; evitando conflictos y poder mantener el orden del proyecto
namespace LoginRegistro.Data;

//Clase Principal de Entity Framework en la cual se encarga de conectar la app con la base de datos
public class AppDbContext : DbContext
{
    //Creamos el constructor de la base de datos en el cual recibe la configuración necesaria para la conexión y se le pasa al constructor padre
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //Podemos acceder a la tabla lllamada Users con datos de tipo AppUser
    public DbSet<AppUser> Users => Set<AppUser>();

    //Con este metodo vamos a ajustar las configuraciónes extras de la base de datos.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Dejamos la configuración que viene por defecto de Entity Framework
        base.OnModelCreating(modelBuilder);

        //En esta línea indicamos que el campo Username debe ser único haciendo que no se puedan registrar dos usuarios con el mismo nombre
        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}