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
                .Returns("") // Primer intento vacío para title, repite hasta que se proporcione una entrada válida
                .Returns("El Quijote") // Entrada válida para title
                .Returns("") // Primer intento vacío para author
                .Returns("Miguel de Cervantes") // Entrada válida para author
                .Returns("") // Primer intento vacío para category
                .Returns("Novela"); // Entrada válida para category

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

            // Añadir un libro primero para poder actualizarlo
            consoleMock.SetupSequence(cw => cw.ReadLine())
                .Returns("") // Primer intento vacío para title, repite hasta que se proporcione una entrada válida
                .Returns("El Quijote") // Entrada válida para title
                .Returns("") // Primer intento vacío para author
                .Returns("Miguel de Cervantes") // Entrada válida para author
                .Returns("") // Primer intento vacío para category
                .Returns("Novela"); // Entrada válida para category

            // Configurar el BookService y ValidateInput con el mock
            BookService.SetConsoleWrapper(consoleMock.Object);
            ValidateInput.SetConsoleWrapper(consoleMock.Object);

            BookService.Add(); // Añadir el libro inicial

            // Configurar el mock para simular la entrada del usuario
            consoleMock.SetupSequence(cw => cw.ReadLine())
                .Returns("1")  // Id del libro
                .Returns("")
                .Returns("Updated Title")  // Título actualizado
                .Returns("")
                .Returns("Updated Author")  // Autor actualizado
                .Returns("")
                .Returns("Updated Category");  // Categoría actualizada

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
                .Returns("")                 // Primer intento vacío para título
                .Returns("El Quijote")       // Título del primer libro
                .Returns("")                 // Primer intento vacío para autor
                .Returns("Miguel de Cervantes") // Autor del primer libro
                .Returns("")                 // Primer intento vacío para categoría
                .Returns("Novela")           // Categoría del primer libro
                .Returns("")                 // Primer intento vacío para título del segundo libro
                .Returns("1984")             // Título del segundo libro
                .Returns("")                 // Primer intento vacío para autor del segundo libro
                .Returns("George Orwell")    // Autor del segundo libro
                .Returns("")                 // Primer intento vacío para categoría del segundo libro
                .Returns("Distopía");        // Categoría del segundo libro

            // Configurar el BookService para usar el mock
            BookService.SetConsoleWrapper(_consoleWrapperMock.Object);
            ValidateInput.SetConsoleWrapper(_consoleWrapperMock.Object);

            // Añadir los libros
            BookService.Add(); // Añadir el primer libro
            BookService.Add(); // Añadir el segundo libro

            // Act
            var result = BookService.GetAll();

            // Assert
            var expectedOutput =
                "|ID       |Título             |Autor              |Categoría          |Disponible\r\n" +
                "|1        |El Quijote         |Miguel de Cervantes|Novela             |Sí       \r\n" +
                "|2        |1984               |George Orwell      |Distopía           |Sí       \r\n";

            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void Delete_ShouldRemoveBook_WhenBookExists()
        {
            // Arrange
            var consoleMock = new Mock<IConsoleWrapper>();

            // Simular la adición de un libro
            consoleMock.SetupSequence(cw => cw.ReadLine())
                .Returns("")                 // Primer intento vacío para título
                .Returns("El Quijote")       // Título del primer libro
                .Returns("")                 // Primer intento vacío para autor
                .Returns("Miguel de Cervantes") // Autor del primer libro
                .Returns("")                 // Primer intento vacío para categoría
                .Returns("Novela");           // Categoría del primer libro

            // Configurar el BookService y ValidateInput con el mock
            BookService.SetConsoleWrapper(consoleMock.Object);
            ValidateInput.SetConsoleWrapper(consoleMock.Object);

            // Añadir un libro
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

            // Simular la adición de un libro
            consoleMock.SetupSequence(cw => cw.ReadLine())
                .Returns("")
                .Returns("Updated Title")  // Título actualizado
                .Returns("")
                .Returns("Updated Author")  // Autor actualizado
                .Returns("")
                .Returns("Updated Category");  // Categoría actualizada

            // Configurar el BookService con el mock
            BookService.SetConsoleWrapper(consoleMock.Object);
            ValidateInput.SetConsoleWrapper(consoleMock.Object);

            // Añadir un libro
            BookService.Add();

            // Configurar el mock para simular la entrada del usuario para buscar un libro por ID
            consoleMock.Setup(cw => cw.ReadLine()).Returns("1");

            // Act
            var result = BookService.GetById();

            // Assert

            Console.WriteLine(result);

            var expectedOutput = $@"|ID       |Título             |Autor              |Categoría          |Disponible
|1        |Updated Title      |Updated Author     |Updated Category   |Sí       
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
