using BooksCRUDInMemory.Service;
using BooksCRUDInMemory.Utils;
using FluentAssertions;
using Moq;

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
            .Returns("Distopía");

        BookService.SetConsoleWrapper(_consoleWrapperMock.Object);
        ValidateInput.SetConsoleWrapper(_consoleWrapperMock.Object);

        BookService.Add();
        BookService.Add();

        // Act
        var result = BookService.GetAll();

        // Assert
        var expectedOutput =
            "|ID       |Título             |Autor              |Categoría          |Disponible\r\n" +
            "|1        |El Quijote         |Miguel de Cervantes|Novela             |Sí       \r\n" +
            "|2        |1984               |George Orwell      |Distopía           |Sí       \r\n";

        result.Should().Be(expectedOutput);
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
        var expectedOutput = $@"|ID       |Título             |Autor              |Categoría          |Disponible
|1        |Updated Title      |Updated Author     |Updated Category   |Sí       
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
