using BooksCRUDInMemory.Service;
using BooksCRUDInMemory.Utils;
using Moq;

namespace BooksCRUDInMemory.Tests
{
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
                .Returns("") // Primer intento vac�o para title, repite hasta que se proporcione una entrada v�lida
                .Returns("El Quijote") // Entrada v�lida para title
                .Returns("") // Primer intento vac�o para author
                .Returns("Miguel de Cervantes") // Entrada v�lida para author
                .Returns("") // Primer intento vac�o para category
                .Returns("Novela"); // Entrada v�lida para category

            BookService.SetConsoleWrapper(consoleMock.Object);
            ValidateInput.SetConsoleWrapper(consoleMock.Object);

            // Act
            var result = BookService.Add();

            // Assert
            Assert.Equal("El libro El Quijote ha sido agregado correctamente en el sistema", result);
            consoleMock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.AtLeast(7)); // Verifica que WriteLine se llame al menos 7 veces.
            consoleMock.Verify(x => x.ReadLine(), Times.Exactly(6)); // Verifica que ReadLine se llame exactamente 6 veces.
        }

        [Fact]
        public void Update_ShouldUpdateBook()
        {
            // Arrange
            var consoleMock = new Mock<IConsoleWrapper>();

            // A�adir un libro primero para poder actualizarlo
            consoleMock.SetupSequence(cw => cw.ReadLine())
                .Returns("") // Primer intento vac�o para title, repite hasta que se proporcione una entrada v�lida
                .Returns("El Quijote") // Entrada v�lida para title
                .Returns("") // Primer intento vac�o para author
                .Returns("Miguel de Cervantes") // Entrada v�lida para author
                .Returns("") // Primer intento vac�o para category
                .Returns("Novela"); // Entrada v�lida para category

            // Configurar el BookService y ValidateInput con el mock
            BookService.SetConsoleWrapper(consoleMock.Object);
            ValidateInput.SetConsoleWrapper(consoleMock.Object);

            BookService.Add(); // A�adir el libro inicial

            // Configurar el mock para simular la entrada del usuario
            consoleMock.SetupSequence(cw => cw.ReadLine())
                .Returns("1")  // Id del libro
                .Returns("")
                .Returns("Updated Title")  // T�tulo actualizado
                .Returns("")
                .Returns("Updated Author")  // Autor actualizado
                .Returns("")
                .Returns("Updated Category");  // Categor�a actualizada

            BookService.SetConsoleWrapper(consoleMock.Object);


            var expectedMessage = "El libro con el id 1 ha sido actualizado correctamente";

            // Act
            var result = BookService.Update();

            // Assert
            Assert.Equal(expectedMessage, result);
        }



        [Fact]
        public void GetAll_ShouldReturnBooksList()
        {
            // Arrange


            // Configurar el mock para simular la entrada del usuario
            _consoleWrapperMock.SetupSequence(cw => cw.ReadLine())
                .Returns("")                 // Primer intento vac�o para t�tulo
                .Returns("El Quijote")       // T�tulo del primer libro
                .Returns("")                 // Primer intento vac�o para autor
                .Returns("Miguel de Cervantes") // Autor del primer libro
                .Returns("")                 // Primer intento vac�o para categor�a
                .Returns("Novela")           // Categor�a del primer libro
                .Returns("")                 // Primer intento vac�o para t�tulo del segundo libro
                .Returns("1984")             // T�tulo del segundo libro
                .Returns("")                 // Primer intento vac�o para autor del segundo libro
                .Returns("George Orwell")    // Autor del segundo libro
                .Returns("")                 // Primer intento vac�o para categor�a del segundo libro
                .Returns("Distop�a");        // Categor�a del segundo libro

            // Configurar el BookService para usar el mock
            BookService.SetConsoleWrapper(_consoleWrapperMock.Object);
            ValidateInput.SetConsoleWrapper(_consoleWrapperMock.Object);

            // A�adir los libros
            BookService.Add(); // A�adir el primer libro
            BookService.Add(); // A�adir el segundo libro

            // Act
            var result = BookService.GetAll();

            // Assert
            var expectedOutput =
                "|ID       |T�tulo             |Autor              |Categor�a          |Disponible\r\n" +
                "|1        |El Quijote         |Miguel de Cervantes|Novela             |S�       \r\n" +
                "|2        |1984               |George Orwell      |Distop�a           |S�       \r\n";

            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void Delete_ShouldRemoveBook_WhenBookExists()
        {
            // Arrange
            var consoleMock = new Mock<IConsoleWrapper>();

            // Simular la adici�n de un libro
            consoleMock.SetupSequence(cw => cw.ReadLine())
                .Returns("")                 // Primer intento vac�o para t�tulo
                .Returns("El Quijote")       // T�tulo del primer libro
                .Returns("")                 // Primer intento vac�o para autor
                .Returns("Miguel de Cervantes") // Autor del primer libro
                .Returns("")                 // Primer intento vac�o para categor�a
                .Returns("Novela");           // Categor�a del primer libro

            // Configurar el BookService y ValidateInput con el mock
            BookService.SetConsoleWrapper(consoleMock.Object);
            ValidateInput.SetConsoleWrapper(consoleMock.Object);

            // A�adir un libro
            BookService.Add();

            // Configurar el mock para simular la entrada del usuario para eliminar un libro
            consoleMock.Setup(cw => cw.ReadLine()).Returns("1");

            // Act
            var result = BookService.Delete();

            // Assert
            var expectedMessage = "El libro con el id 1 ha sido eliminado correctamente";
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void Delete_ShouldReturnErrorMessage_WhenBookDoesNotExist()
        {
            // Arrange
            var consoleMock = new Mock<IConsoleWrapper>();

            // Configurar el mock para simular la entrada del usuario con un ID inexistente
            consoleMock.Setup(cw => cw.ReadLine()).Returns("99");

            // Configurar el BookService con el mock
            BookService.SetConsoleWrapper(consoleMock.Object);

            // Act
            var result = BookService.Delete();

            // Assert
            var expectedMessage = "El libro con Id 99 no existe";
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void GetById_ShouldReturnBookDetails_WhenBookExists()
        {
            // Arrange
            var consoleMock = new Mock<IConsoleWrapper>();

            // Simular la adici�n de un libro
            consoleMock.SetupSequence(cw => cw.ReadLine())
                .Returns("")
                .Returns("Updated Title")  // T�tulo actualizado
                .Returns("")
                .Returns("Updated Author")  // Autor actualizado
                .Returns("")
                .Returns("Updated Category");  // Categor�a actualizada

            // Configurar el BookService con el mock
            BookService.SetConsoleWrapper(consoleMock.Object);
            ValidateInput.SetConsoleWrapper(consoleMock.Object);

            // A�adir un libro
            BookService.Add();

            // Configurar el mock para simular la entrada del usuario para buscar un libro por ID
            consoleMock.Setup(cw => cw.ReadLine()).Returns("1");

            // Act
            var result = BookService.GetById();

            // Assert

            Console.WriteLine(result);

            var expectedOutput = $@"|ID       |T�tulo             |Autor              |Categor�a          |Disponible
|1        |Updated Title      |Updated Author     |Updated Category   |S�       
";


            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void GetById_ShouldReturnErrorMessage_WhenBookDoesNotExist()
        {
            // Arrange
            var consoleMock = new Mock<IConsoleWrapper>();

            // Configurar el mock para simular la entrada del usuario con un ID inexistente
            consoleMock.Setup(cw => cw.ReadLine()).Returns("99");

            // Configurar el BookService con el mock
            BookService.SetConsoleWrapper(consoleMock.Object);

            // Act
            var result = BookService.GetById();

            // Assert
            var expectedMessage = "El libro con Id 99 no existe";
            Assert.Equal(expectedMessage, result);
        }


    }
}
