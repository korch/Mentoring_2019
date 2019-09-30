using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);   // used parallel just for the first loop... Currently, I don't know how it works inside TPL,  
                                                                       //and how it works if I set parallel loop for others
                                                                       // iterations (loop by columns etc.) I'm checking any information about it to understand difference and what a best way
                                                                       // to use parallel loop in cases like this. :)
                                                               

            for (long i = 0; i < m1.RowCount; i++)
            {
                for (long j = 0; j < m2.ColCount; j++)
                {
                    long sum = 0;
                    Parallel.For(0, m1.ColCount, k => { sum += m1.GetElement(i, k) * m2.GetElement(k, j); });
                    resultMatrix.SetElement(i, j, sum);
                }
            };

            return resultMatrix;
        }
    }
}