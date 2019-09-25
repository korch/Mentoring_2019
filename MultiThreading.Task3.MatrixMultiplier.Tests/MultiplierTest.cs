using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    public enum Multipliers
    {
        Usual = 1,
        Parallel
    }

    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            var capacity = 2;
            Multipliers multiplier = Multipliers.Usual;

            while (multiplier == Multipliers.Usual)
            {
                multiplier = WhoIsBetter(capacity);
                capacity++;
            }

            Assert.IsTrue(capacity > 15); // capacity can be differ. It depends on PC configuration.
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        private Multipliers WhoIsBetter(int capacity)
        {
            var m1 = new Matrix(capacity, capacity);
            var m2 = new Matrix(capacity, capacity);

            SetupValues(m1);
            SetupValues(m2);

            var multiplier = new MatricesMultiplier();
            var multiplierParallel = new MatricesMultiplierParallel();

            var milliseconds = Time(() => multiplier.Multiply(m1, m2));
            var millisecondsParallel = Time(() => multiplierParallel.Multiply(m1, m2));

            return millisecondsParallel < milliseconds ? Multipliers.Parallel : Multipliers.Usual;
        }

        private void SetupValues(Matrix matrix)
        {
            var rand = new Random();
            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColCount; j++)
                {
                    matrix.SetElement(i, j, rand.Next(1, 100));
                }
            }
        }

        private long Time(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }

        #endregion
    }
}
