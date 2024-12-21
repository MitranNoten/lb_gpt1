using Xunit;
using System;
using System.IO;
using System.Xml.Linq;

namespace TestProject2
{
    public class UnitTest1
    {
        /// <summary>
        /// �����, ���������� ����� ��� �������� ���������������� Fb2Processor.
        /// </summary>
        public class Fb2ProcessorTests
        {
            /// <summary>
            /// ����, �����������, ��� ����� ReplaceWordIfInSlovar �������� ����� �� ������� �� "�������".
            /// </summary>
            [Fact]
            public void ReplaceWordIfInSlovar_WordInSlovar_Replaced()
            {
                // Arrange - ���������� ������ ��� �����.
                string word = "������";

                // Act - ���������� ������������ ������.
                string result = Fb2Processor.ReplaceWordIfInSlovar(word);

                // Assert - �������� ����������.
                Assert.Equal("�������", result);
            }

            /// <summary>
            /// ����, �����������, ��� ����� ReplaceWordIfInSlovar �� �������� �����, �������� ��� � �������.
            /// </summary>
            [Fact]
            public void ReplaceWordIfInSlovar_WordNotInSlovar_NotReplaced()
            {
                // Arrange
                string word = "������";

                // Act
                string result = Fb2Processor.ReplaceWordIfInSlovar(word);

                // Assert
                Assert.Equal("������", result);
            }
            /// <summary>
            /// ����, �����������, ��� ����� ReplaceWordIfInSlovar �������� ����� �� ������� ��� ����� ��������.
            /// </summary>
            [Fact]
            public void ReplaceWordIfInSlovar_WordMixedCase_Replaced()
            {
                // Arrange
                string word = "����";

                // Act
                string result = Fb2Processor.ReplaceWordIfInSlovar(word);

                // Assert
                Assert.Equal("�������", result);
            }
            /// <summary>
            /// ����, �����������, ��� ����� ReplaceWordIfInSlovar ��������� ������������ ������ ������.
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
            /// ����, �����������, ��� ����� ProcessFb2File ��������� �������� ����� � �����.
            /// </summary>
            [Fact]
            public void ProcessFb2File_ValidFile_ReplacedWords()
            {
                // Arrange
                string inputFilePath = "test_input.fb2";
                string outputFilePath = "test_output.fb2";
                string inputContent = "<section><p>� ������ ��� ������. ��� �� ��� � ����, � ����. ���� � ������.</p></section>";
                string expectedOutputContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<section><p>� ������ ��� �������. ��� �� ��� � �������, � �������. ������� � �������.</p></section>";


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
            /// ����, �����������, ��� ����� ProcessFb2File ��������� ������������ ������ ����.
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
                // ���������, ��� ���� �� ������, ���� ������� ������
                Assert.False(File.Exists(outputFilePath));

                // CleanUp
                File.Delete(inputFilePath);
            }
        }
    }
}