using Balltracking.Scripts;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class Test_Projection
    {
        Vector3 _xAxis = new Vector3(1, 0, 0);

        [Test]
        public void Test_ParallelVectorsGettingProjectedCorrectly()
        {
            // Arrange
            var parallelVector = new Vector3(0.1f, 0, 0);

            // Act
            var projection = new VectorProjection(parallelVector);
            var projectedVector = projection.InDirection(_xAxis);

            // Assert
            Assert.AreEqual(parallelVector, projectedVector);
        }

        [Test]
        public void Test_DiagonalVectorGetsProjectedCorrectly()
        {
            // Arrange
            var diagonalVector = new Vector3(1, 1, 0);
            
            // Act
            var projection = new VectorProjection(diagonalVector);
            var projectedVector = projection.InDirection(_xAxis);
            
            // Assert
            Assert.AreEqual(_xAxis, projectedVector);
        }

        [Test]
        public void Test_XComponentOfVectorGetsProjectedCorrectly()
        {
            // Arrange
            var shortDiagnoalVector = new Vector3(0.5f, 1f, 0);
            
            // Act
            var projection = new VectorProjection(shortDiagnoalVector);
            var projectedVector = projection.InDirection(_xAxis);
            
            // Assert
            Assert.AreEqual(new Vector3(0.5f, 0, 0), projectedVector);
        }
    }
}