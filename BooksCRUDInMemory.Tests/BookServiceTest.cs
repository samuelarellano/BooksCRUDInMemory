using BooksCRUDInMemory.Service;
using BooksCRUDInMemory.Utils;
using FluentAssertions;
using Moq;
using System.Runtime.Serialization;
using System.Text;

namespace BooksCRUDInMemory.Tests;

public class BookServiceTests
{
    private Mock<IConsoleWrapper> _consoleWrapperMock;

    public BookServiceTests()
    {
        _consoleWrapperMock = new Mock<IConsoleWrapper>();
    }

    [Fact]
    public void Add_ShouldAddBookToList()
    {
        // Arrange
        var consoleMock = new Mock<IConsoleWrapper>();
        consoleMock.SetupSequence(x => x.ReadLine())
            .Returns("")
            .Returns("El Quijote")
            .Returns("")
            .Returns("Miguel de Cervantes")
            .Returns("")
            .Returns("Novela");

        BookService.SetConsoleWrapper(consoleMock.Object);
        ValidateInput.SetConsoleWrapper(consoleMock.Object);

        // Act
        var result = BookService.Add();

        // Assert
        result.Should().Be("El libro El Quijote ha sido agregado correctamente en el sistema");
        consoleMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.AtLeast(7));
        consoleMock.Verify(x => x.ReadLine(), Times.Exactly(6));
    }

    [Fact]
    public void Update_ShouldUpdateBook()
    {
        // Arrange
        var consoleMock = new Mock<IConsoleWrapper>();

        consoleMock.SetupSequence(cw => cw.ReadLine())
            .Returns("")
            .Returns("El Quijote")
            .Returns("")
            .Returns("Miguel de Cervantes")
            .Returns("")
            .Returns("Novela");

        BookService.SetConsoleWrapper(consoleMock.Object);
        ValidateInput.SetConsoleWrapper(consoleMock.Object);

        BookService.Add();

        consoleMock.SetupSequence(cw => cw.ReadLine())
            .Returns("1")
            .Returns("")
            .Returns("Updated Title")
            .Returns("")
            .Returns("Updated Author")
            .Returns("")
            .Returns("Updated Category");

        BookService.SetConsoleWrapper(consoleMock.Object);

        var expectedMessage = "El libro con el id 1 ha sido actualizado correctamente";

        // Act
        var result = BookService.Update();

        // Assert
        result.Should().Be(expectedMessage);
    }


    [Fact]
    public void GetAll_ShouldReturnBooksList()
    {
        // Arrange
        _consoleWrapperMock.SetupSequence(cw => cw.ReadLine())
            .Returns("")
            .Returns("El Quijote")
            .Returns("")
            .Returns("Miguel de Cervantes")
            .Returns("")
            .Returns("Novela")
            .Returns("")
            .Returns("1984")
            .Returns("")
            .Returns("George Orwell")
            .Returns("")
            .Returns("Distop�a");

        BookService.SetConsoleWrapper(_consoleWrapperMock.Object);
        ValidateInput.SetConsoleWrapper(_consoleWrapperMock.Object);

        BookService.Add();
        BookService.Add();

        // Act
        var result = BookService.GetAll();

        // Assert
        var builder = new StringBuilder();

        // Agregar encabezado
        builder.AppendLine("|ID".PadRight(10) + "|T�tulo".PadRight(20) + "|Autor".PadRight(20) + "|Categor�a".PadRight(20) + "|Disponible".PadRight(10));

        // Agregar datos est�ticos
        builder.AppendLine("|1".PadRight(10) + "|El Quijote".PadRight(20) + "|Miguel de Cervantes".PadRight(20) + "|Novela".PadRight(20) + "|S�".PadRight(10));
        builder.AppendLine("|2".PadRight(10) + "|1984".PadRight(20) + "|George Orwell".PadRight(20) + "|Distop�a".PadRight(20) + "|S�".PadRight(10));

        var normalizedExpected = Normalize(builder.ToString());
        var normalizedResult = Normalize(result);

        // Detallar diferencia si la prueba falla
        try
        {
            normalizedResult.Should().Be(normalizedExpected);
        }
        catch (Xunit.Sdk.XunitException)
        {
            Console.WriteLine("Expected: [" + string.Join(",", normalizedExpected.Select(c => (int)c)) + "]");
            Console.WriteLine("Actual:   [" + string.Join(",", normalizedResult.Select(c => (int)c)) + "]");
            throw;
        }
    }

    private string Normalize(string input)
    {
        // Normaliza la cadena a FormC para evitar problemas con la codificaci�n de caracteres especiales
        return input.Normalize(NormalizationForm.FormC);
    }




    [Fact]
    public void Delete_ShouldRemoveBook_WhenBookExists()
    {
        // Arrange
        var consoleMock = new Mock<IConsoleWrapper>();

        consoleMock.SetupSequence(cw => cw.ReadLine())
            .Returns("")
            .Returns("El Quijote")
            .Returns("")
            .Returns("Miguel de Cervantes")
            .Returns("")
            .Returns("Novela");

        BookService.SetConsoleWrapper(consoleMock.Object);
        ValidateInput.SetConsoleWrapper(consoleMock.Object);

        BookService.Add();

        consoleMock.Setup(cw => cw.ReadLine()).Returns("1");

        // Act
        var result = BookService.Delete();

        // Assert
        var expectedMessage = "El libro con el id 1 ha sido eliminado correctamente";
        result.Should().Be(expectedMessage);
    }

    [Fact]
    public void Delete_ShouldReturnErrorMessage_WhenBookDoesNotExist()
    {
        // Arrange
        var consoleMock = new Mock<IConsoleWrapper>();

        consoleMock.Setup(cw => cw.ReadLine()).Returns("99");

        BookService.SetConsoleWrapper(consoleMock.Object);

        // Act
        var result = BookService.Delete();

        // Assert
        var expectedMessage = "El libro con Id 99 no existe";
        result.Should().Be(expectedMessage);
    }

    [Fact]
    public void GetById_ShouldReturnBookDetails_WhenBookExists()
    {
        // Arrange
        var consoleMock = new Mock<IConsoleWrapper>();

        consoleMock.SetupSequence(cw => cw.ReadLine())
            .Returns("")
            .Returns("Updated Title")
            .Returns("")
            .Returns("Updated Author")
            .Returns("")
            .Returns("Updated Category");

        BookService.SetConsoleWrapper(consoleMock.Object);
        ValidateInput.SetConsoleWrapper(consoleMock.Object);

        BookService.Add();

        consoleMock.Setup(cw => cw.ReadLine()).Returns("1");

        // Act
        var result = BookService.GetById();

        // Assert
        var expectedOutput = $@"|ID       |T�tulo             |Autor              |Categor�a          |Disponible
|1        |Updated Title      |Updated Author     |Updated Category   |S�       
";

        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void GetById_ShouldReturnErrorMessage_WhenBookDoesNotExist()
    {
        // Arrange
        var consoleMock = new Mock<IConsoleWrapper>();

        consoleMock.Setup(cw => cw.ReadLine()).Returns("99");

        BookService.SetConsoleWrapper(consoleMock.Object);

        // Act
        var result = BookService.GetById();

        // Assert
        var expectedMessage = "El libro con Id 99 no existe";
        result.Should().Be(expectedMessage);
    }
}
