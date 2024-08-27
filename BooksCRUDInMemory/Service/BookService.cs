namespace BooksCRUDInMemory.Service;

public interface IConsoleWrapper
{
    void WriteLine(string message);
    string ReadLine();
}

public class ConsoleWrapper : IConsoleWrapper
{
    public void WriteLine(string message) => Console.WriteLine(message);
    public string ReadLine() => Console.ReadLine() ?? string.Empty;
}

public static class BookService
{
    private static List<Book> books;
    private static IConsoleWrapper _consoleWrapper;

    static BookService()
    {
        books = new List<Book>();
        _consoleWrapper = new ConsoleWrapper();
    }

    public static void SetConsoleWrapper(IConsoleWrapper consoleWrapper)
    {
        _consoleWrapper = consoleWrapper;
    }

    public static string Add()
    {
        _consoleWrapper.WriteLine("Añadiendo un libro...");
        _consoleWrapper.WriteLine("Ingrese el titulo del libro:");
        string title = ValidateInput.RequestInput(_consoleWrapper.ReadLine() ?? string.Empty, _consoleWrapper);
        _consoleWrapper.WriteLine("Ingrese el autor del libro:");
        string author = ValidateInput.RequestInput(_consoleWrapper.ReadLine() ?? string.Empty, _consoleWrapper);
        _consoleWrapper.WriteLine("Ingrese la categoria del libro:");
        string category = ValidateInput.RequestInput(_consoleWrapper.ReadLine() ?? string.Empty, _consoleWrapper);

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
        _consoleWrapper.WriteLine("Actualizar Libro");
        _consoleWrapper.WriteLine("Ingrese el Id del libro");
        int.TryParse(_consoleWrapper.ReadLine(), out int id);
        _consoleWrapper.WriteLine("Ingrese el titulo del libro");
        string title = ValidateInput.RequestInput(_consoleWrapper.ReadLine() ?? string.Empty, _consoleWrapper);
        _consoleWrapper.WriteLine("Ingrese el autor del libro");
        string author = ValidateInput.RequestInput(_consoleWrapper.ReadLine() ?? string.Empty, _consoleWrapper);
        _consoleWrapper.WriteLine("Ingrese la categoria del libro");
        string category = ValidateInput.RequestInput(_consoleWrapper.ReadLine() ?? string.Empty, _consoleWrapper);

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
        _consoleWrapper.WriteLine("Listado de Libros");

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
        _consoleWrapper.WriteLine("Eliminar Libro");
        _consoleWrapper.WriteLine("Ingrese el Id del libro");
        int.TryParse(_consoleWrapper.ReadLine(), out int id);

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
        _consoleWrapper.WriteLine("Búsqueda de Libro por ID");
        _consoleWrapper.WriteLine("Ingrese el Id del libro");
        int.TryParse(_consoleWrapper.ReadLine(), out int id);

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
