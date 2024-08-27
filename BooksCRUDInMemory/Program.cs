
using BooksCRUDInMemory.Service;
using BooksCRUDInMemory.Utils;

while (true)
{
    Console.Clear();
    var option = Options();

    if (option.Equals((int) OptionEnum.Exit))
        return;

    OptionEnum optionEnum = (OptionEnum)option;

    string message = optionEnum switch
    {
        OptionEnum.Add => BookService.Add(),
        OptionEnum.Update => BookService.Update(),
        OptionEnum.GetAll => BookService.GetAll(),
        OptionEnum.GetById => BookService.GetById(),
        OptionEnum.Delete => BookService.Delete(),
        OptionEnum.Exit => "Gracias por usar el Sistema",
        _ => "Opción inválida"
    };

    Console.WriteLine(message);
    Console.ReadLine();
}


static int Options()
{
    Console.WriteLine("Bienvenido al Sistema de Administración de Libros");
    Console.WriteLine("1. Agregar Libro");
    Console.WriteLine("2. Actualizar Libro");
    Console.WriteLine("3. Buscar Libro por Nombre");
    Console.WriteLine("4. Mostrar todos los Libros");
    Console.WriteLine("5. Eliminar Libro por Id");
    Console.WriteLine("6. Salir del Sistema");
    int.TryParse(Console.ReadLine(),out int option);
    return option;
}