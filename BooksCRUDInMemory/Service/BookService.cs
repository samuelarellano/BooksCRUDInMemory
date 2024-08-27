namespace BooksCRUDInMemory.Service;

public static class BookService
{
    private static List<Book> books;

    //Constructor estático
    static BookService()
    {
        books = new List<Book>();
    }

    public static string Add()
    {
        Console.WriteLine("Añadiendo un libro...");
        Console.WriteLine("Ingrese el titulo del libro:");
        string title = ValidateInput.RequestInput(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Ingrese el autor del libro:");
        string author = ValidateInput.RequestInput(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Ingrese la categoria del libro:");
        string category = ValidateInput.RequestInput(Console.ReadLine() ?? string.Empty);

        // Crear un nuevo libro con la información ingresada por consola
        var book = new Book
        {
            Id = books.Count + 1,
            Title = title,
            Author = author,
            Category = category,
            IsAvailable = true
        };
        books.Add(book);

        return $"El libro {book.Title} ha sido agregado correctamente en el sistema";
    }

    public static string Update()
    {
        Console.WriteLine("Actualizar Libro");

        Console.WriteLine("Ingrese el Id del libro");
        int.TryParse(Console.ReadLine(), out int id);
        Console.WriteLine("Ingrese el titulo del libro");
        string title = ValidateInput.RequestInput(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Ingrese el autor del libro");
        string author = ValidateInput.RequestInput(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Ingrese la categoria del libro");
        string category = ValidateInput.RequestInput(Console.ReadLine() ?? string.Empty);

        var book = books.FirstOrDefault(x => x.Id == id);

        if (book is null)
        {
            return $"El libro con Id {id} no existe";
        }

        book.Title = title;
        book.Author = author;
        book.Category = category;

        return $"El libro con el id {book.Id} ha sido actualizado correctamente";
    }

    public static string GetAll()
    {
        string message = string.Empty;
        Console.WriteLine("Listado de Libros");

        if (!books.Any())
            return "No existen libros en memoria";

        var builder = new StringBuilder();
        builder.AppendLine("|ID".PadRight(10) + "|Título".PadRight(20) + "|Autor".PadRight(20) + "|Categoría".PadRight(20) + "|Disponible".PadRight(10));
        foreach (var book in books)
        {
            builder.AppendLine($"|{book.Id.ToString().PadRight(9)}|{book.Title.PadRight(19)}|{book.Author.PadRight(19)}|{book.Category.PadRight(19)}|{(book.IsAvailable ? "Sí" : "No").PadRight(9)}");

        }
        return builder.ToString();
    }
    public static string Delete()
    {
        Console.WriteLine("Eliminar Libro");

        Console.WriteLine("Ingrese el Id del libro");
        int.TryParse(Console.ReadLine(), out int id);


        var book = books.FirstOrDefault(x => x.Id == id);

        if (book is null)
        {
            return $"El libro con Id {id} no existe";
        }

        books.Remove(book);

        return $"El libro con el id {book.Id} ha sido eliminado correctamente";
    }

    public static string GetById()
    {
        Console.WriteLine("Búsqueda de Libro por ID");

        Console.WriteLine("Ingrese el Id del libro");
        int.TryParse(Console.ReadLine(), out int id);


        var book = books.FirstOrDefault(x => x.Id == id);

        if (book is null)
        {
            return $"El libro con Id {id} no existe";
        }

        var builder = new StringBuilder();
        builder.AppendLine("|ID".PadRight(10) + "|Título".PadRight(20) + "|Autor".PadRight(20) + "|Categoría".PadRight(20) + "|Disponible".PadRight(10));
        builder.AppendLine($"|{book.Id.ToString().PadRight(9)}|{book.Title.PadRight(19)}|{book.Author.PadRight(19)}|{book.Category.PadRight(19)}|{(book.IsAvailable ? "Sí" : "No").PadRight(9)}");

        return builder.ToString();
    }
}
