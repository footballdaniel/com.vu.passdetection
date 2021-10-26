using Balltracking;
using Balltracking.Scripts;
using NUnit.Framework;


namespace Tests.Editor
{
    public class Test_MovingAverage
    {

        [Test]
        public void Test_MovingAverageReturnsFirstValueIfOnlyOneValuePresent()
        {
            // Arrange
            var value = 5f;
            var filterWindowSize = 5;
            var filter = new MovingAverage(filterWindowSize);

            // Act
            var filteredValue = filter.Add(value).Average;
            
            // Assert
            Assert.AreEqual(value, filteredValue);
        }

        [Test]
        public void TestMovingAverageReturnsAverageWhenTwoValuesPresent()
        {
            // Arrange
            var firstValue = 0f;
            var secondValue = 5f;
            var filterWindowSize = 5;
            var filter = new MovingAverage(filterWindowSize);
            
            // Act
            var filteredValue = filter
                .Add(firstValue)
                .Add(secondValue)
                .Average;
            
            // Assert
            Assert.AreEqual(2.5f, filteredValue);
        }
    }
}