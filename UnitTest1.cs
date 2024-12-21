using Xunit;
using System;
using System.IO;
using System.Xml.Linq;

namespace TestProject2
{
    public class UnitTest1
    {
        /// <summary>
        /// Класс, содержащий тесты для проверки функциональности Fb2Processor.
        /// </summary>
        public class Fb2ProcessorTests
        {
            /// <summary>
            /// Тест, проверяющий, что метод ReplaceWordIfInSlovar заменяет слово из словаря на "человек".
            /// </summary>
            [Fact]
            public void ReplaceWordIfInSlovar_WordInSlovar_Replaced()
            {
                // Arrange - Подготовка данных для теста.
                string word = "хоббит";

                // Act - Выполнение тестируемого метода.
                string result = Fb2Processor.ReplaceWordIfInSlovar(word);

                // Assert - Проверка результата.
                Assert.Equal("человек", result);
            }

            /// <summary>
            /// Тест, проверяющий, что метод ReplaceWordIfInSlovar не заменяет слово, которого нет в словаре.
            /// </summary>
            [Fact]
            public void ReplaceWordIfInSlovar_WordNotInSlovar_NotReplaced()
            {
                // Arrange
                string word = "дракон";

                // Act
                string result = Fb2Processor.ReplaceWordIfInSlovar(word);

                // Assert
                Assert.Equal("дракон", result);
            }
            /// <summary>
            /// Тест, проверяющий, что метод ReplaceWordIfInSlovar заменяет слово из словаря без учета регистра.
            /// </summary>
            [Fact]
            public void ReplaceWordIfInSlovar_WordMixedCase_Replaced()
            {
                // Arrange
                string word = "ЭльФ";

                // Act
                string result = Fb2Processor.ReplaceWordIfInSlovar(word);

                // Assert
                Assert.Equal("человек", result);
            }
            /// <summary>
            /// Тест, проверяющий, что метод ReplaceWordIfInSlovar правильно обрабатывает пустую строку.
            /// </summary>
            [Fact]
            public void ReplaceWordIfInSlovar_WordEmpty_NotReplaced()
            {
                // Arrange
                string word = "";

                // Act
                string result = Fb2Processor.ReplaceWordIfInSlovar(word);

                // Assert
                Assert.Equal("", result);
            }
            /// <summary>
            /// Тест, проверяющий, что метод ProcessFb2File правильно заменяет слова в файле.
            /// </summary>
            [Fact]
            public void ProcessFb2File_ValidFile_ReplacedWords()
            {
                // Arrange
                string inputFilePath = "test_input.fb2";
                string outputFilePath = "test_output.fb2";
                string inputContent = "<section><p>В пещере жил хоббит. Там же был и эльф, и гном. орки и тролли.</p></section>";
                string expectedOutputContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<section><p>В пещере жил человек. Там же был и человек, и человек. человек и человек.</p></section>";


                File.WriteAllText(inputFilePath, inputContent);
                File.WriteAllText("expected_output.fb2", expectedOutputContent);

                // Act
                Fb2Processor.ProcessFb2File(inputFilePath, outputFilePath);

                // Assert
                XDocument actualXml = XDocument.Load(outputFilePath);
                XDocument expectedXml = XDocument.Load("expected_output.fb2");

                Assert.True(XNode.DeepEquals(expectedXml, actualXml));

                // CleanUp
                File.Delete(inputFilePath);
                File.Delete(outputFilePath);
                File.Delete("expected_output.fb2");
            }
            /// <summary>
            /// Тест, проверяющий, что метод ProcessFb2File правильно обрабатывает пустой файл.
            /// </summary>
            [Fact]
            public void ProcessFb2File_EmptyFile_NoChanges()
            {
                // Arrange
                string inputFilePath = "empty_input.fb2";
                string outputFilePath = "empty_output.fb2";
                File.WriteAllText(inputFilePath, "");

                // Act
                Fb2Processor.ProcessFb2File(inputFilePath, outputFilePath);

                // Assert
                // Проверяем, что файл не создан, если входной пустой
                Assert.False(File.Exists(outputFilePath));

                // CleanUp
                File.Delete(inputFilePath);
            }
        }
    }
}