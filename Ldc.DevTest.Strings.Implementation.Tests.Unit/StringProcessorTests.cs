using FluentAssertions;
using Ldc.DevTest.Strings.Contracts;
using System;
using System.Collections.Generic;
using Xunit;

namespace Ldc.DevTest.Strings.Implementation.Tests.Unit
{
    public class StringProcessorTests
    {
        private const int MaxInputLength = 15;
        private const string TestStringA = "A 1238 (*%! A th1s is an input string used in our tests";
        private const string TestStringB = "B 56789 )-=+ B th1s is another input string used in our tests";
        private const string TestStringC = "C 93 )-=+ th1s is another input string used in our tests";

        private IStringProcessor _sut;

        public StringProcessorTests()
        {
            _sut = new StringProcessor();
        }

        [Fact]
        public void When_InputStringsCollection_IsEmpty_Return_Empty()
        {
            // Arrange
            var stringsToProcess = new List<string> { };

            // Act
            var processedStrings = _sut.ProcessStrings(stringsToProcess);

            // Assert
            processedStrings.Should().NotBeNull();
            processedStrings.Should().BeEmpty();
        }

        [Fact]
        public void WhenInputString_GreaterThan_MaxLength_Truncate()
        {
            // Arrange
            var smallerThanMaxLengthString = TestStringA.Substring(0, MaxInputLength - 1);
            var maxLengthString = TestStringB.Substring(0, MaxInputLength);
            var largerThanMaxLengthString = TestStringC.Substring(0, MaxInputLength + 1);
            var largerThanMaxLengthStringTruncated = TestStringC.Substring(0, MaxInputLength);

            var stringsToProcess = new List<string> { smallerThanMaxLengthString, maxLengthString, largerThanMaxLengthString };

            // Act
            var processedStrings = _sut.ProcessStrings(stringsToProcess);

            // Assert
            processedStrings.Should().NotBeNull();
            processedStrings.Should().HaveCount(stringsToProcess.Count);

            processedStrings.Should().Contain(smallerThanMaxLengthString);
            processedStrings.Should().Contain(maxLengthString);
            processedStrings.Should().Contain(largerThanMaxLengthStringTruncated);
        }

        [Fact]
        public void WhenInputString_Contains_ContigiousDuplicate_Chars_ReduceToSingleChar()
        {
            // Arrange
            const string InputStringA = "AWWWwwwThis ppPPPP AA";
            const string ExpectedOutputStringA = "AWwThis pP A";

            const string InputStringB = "BzZZZe bbCCCC --+**";
            const string ExpectedOutputStringB = "BzZe bC -+*";

            const string InputStringC = "String C";
            const string ExpectedOutputStringC = "String C";

            var stringsToProcess = new List<string> { InputStringA, InputStringB, InputStringC };

            // Act
            var processedStrings = _sut.ProcessStrings(stringsToProcess);

            // Assert
            processedStrings.Should().NotBeNull();
            processedStrings.Should().HaveCount(stringsToProcess.Count);

            processedStrings.Should().Contain(ExpectedOutputStringA);
            processedStrings.Should().Contain(ExpectedOutputStringB);
            processedStrings.Should().Contain(ExpectedOutputStringC);
        }

        [Fact]
        public void WhenInputString_ContainsDollarSigns_ReplaceWithPoundSign()
        {
            // Arrange
            const string InputStringA = "A $ is $";
            const string ExpectedOutputStringA = "A £ is £";

            const string InputStringB = "This $$$ is the $";
            const string ExpectedOutputStringB = "This £ is the £";


            var stringsToProcess = new List<string> { InputStringA, InputStringB };

            // Act
            var processedStrings = _sut.ProcessStrings(stringsToProcess);

            // Assert
            processedStrings.Should().NotBeNull();
            processedStrings.Should().HaveCount(stringsToProcess.Count);

            processedStrings.Should().Contain(ExpectedOutputStringA);
            processedStrings.Should().Contain(ExpectedOutputStringB);
        }

        [Fact]
        public void WhenInputString_Contains4Character_RemoveIt()
        {
            // Arrange
            const string InputStringA = "A 4 in 4 str";
            const string ExpectedOutputStringA = "A  in  str";

            const string InputStringB = "A-444-is-the-44";
            const string ExpectedOutputStringB = "A--is-the-";

            var stringsToProcess = new List<string> { InputStringA, InputStringB };

            // Act
            var processedStrings = _sut.ProcessStrings(stringsToProcess);

            // Assert
            processedStrings.Should().NotBeNull();
            processedStrings.Should().HaveCount(stringsToProcess.Count);

            processedStrings.Should().Contain(ExpectedOutputStringA);
            processedStrings.Should().Contain(ExpectedOutputStringB);
        }

        [Fact]
        public void WhenInputString_ContainsUnderscoreCharacter_RemoveIt()
        {
            // Arrange
            const string InputStringA = "A _ in _ str";
            const string ExpectedOutputStringA = "A  in  str";

            const string InputStringB = "A-___-is-the-__";
            const string ExpectedOutputStringB = "A--is-the-";

            var stringsToProcess = new List<string> { InputStringA, InputStringB };

            // Act
            var processedStrings = _sut.ProcessStrings(stringsToProcess);

            // Assert
            processedStrings.Should().NotBeNull();
            processedStrings.Should().HaveCount(stringsToProcess.Count);

            processedStrings.Should().Contain(ExpectedOutputStringA);
            processedStrings.Should().Contain(ExpectedOutputStringB);
        }

        [Fact]
        public void WhenInputStringIsNull_OmitInOutput()
        {
            // Arrange
            const string InputStringA = "String1";
            const string InputStringB = "String2";

            var stringsToProcess = new List<string> { InputStringA, null, InputStringB, null};

            // Act
            var processedStrings = _sut.ProcessStrings(stringsToProcess);

            // Assert
            processedStrings.Should().NotBeNull();
            processedStrings.Should().HaveCount(2);

            processedStrings.Should().Contain(InputStringA);
            processedStrings.Should().Contain(InputStringB);
        }

        [Fact]
        public void WhenStringWouldBeEmpty_OmitInOutput()
        {
            // Arrange
            const string InputStringA = "String1";
            const string InputStringB = "String2";

            var stringsToProcess = new List<string> { InputStringA, string.Empty, InputStringB, "4444" };

            // Act
            var processedStrings = _sut.ProcessStrings(stringsToProcess);

            // Assert
            processedStrings.Should().NotBeNull();
            processedStrings.Should().HaveCount(2);

            processedStrings.Should().Contain(InputStringA);
            processedStrings.Should().Contain(InputStringB);
        }
    }
}
