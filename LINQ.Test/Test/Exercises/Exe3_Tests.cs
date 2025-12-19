using LINQ_Exercises.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_Exercise_Test.Test.Exercises
{
    public class Exe3_Tests
    {
        [Fact]
        public void DisplayGreaterThen_ReturnsNumbersGreaterThan80()
        {
            // Arrange
            var sut = new Exe_3();
            var input = new List<int> { 50, 81, 90, 80, 100, 79 };
            var expected = new List<int> { 81, 90, 100 };
            var input2 = new List<int> { 10, 20, 80 };
            var input3 = new List<int>(); ;
            // Act
            var result = sut.DisplayGreaterThen(input);
            var result1 = sut.DisplayGreaterThen(input2);
            var result2 = sut.DisplayGreaterThen(input3);

            // Assert - preserves input order for filtered items
            Assert.Equal(expected, result);
            Assert.Empty(result1);
            Assert.Empty(result2);
        }

    }
}
