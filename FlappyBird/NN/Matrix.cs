using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.NN
{
    public class Matrix
    {
        private static Random r;

        static Matrix()
        {
            r = new Random(123);
        }
        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            data = new double[rows, cols];
        }

        public delegate double MapFunction(double value, int i, int j);

        public Matrix Copy()
        {
            var matrix = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix.data[i, j] = this.data[i, j];
                }
            }
            return matrix;
        }

        public Matrix Map(MapFunction func)
        {
            Matrix matrix = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix.data[i, j] = func(data[i, j], i, j);
                }
            }
            return matrix;
        }

        public Matrix Substract(Matrix b)
        {
            if (rows != b.rows || cols != b.cols)
            {
                throw new Exception("Columns and Rows of A must match Columns and Rows of B.");
            }
            // Return a new Matrix a-b
            return this
              .Map((_, i, j) => data[i, j] - b.data[i, j]);
        }

        public double[] ToArray()
        {
            List<double> res = new List<double>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    res.Add(data[i, j]);
                }
            }
            return res.ToArray();
        }

        public Matrix Randomize()
        {
            return this.Map((e, _, __) => r.NextDouble() * 2 - 1);
        }

        public Matrix Add(Matrix n)
        {
            if (this.rows != n.rows || this.cols != n.cols)
            {
                throw new Exception("Columns and Rows of A must match Columns and Rows of B.");
            }
            return this.Map((e, i, j) => e + n.data[i, j]);
        }
        public Matrix Add(double n)
        {
            return this.Map((e, i, j) => e + n);
        }

        public Matrix Transpose()
        {
            return this
              .Map((_, i, j) => data[j, i]);
        }

        public Matrix multiply(Matrix b)
        {
            // Matrix product
            if (cols != b.rows)
            {
                throw new Exception("Columns of A must match rows of B.");

            }

            return this
              .Map((e, i, j) =>
              {
                  // Dot product of values in col
                  double sum = 0;
                  for (int k = 0; k < cols; k++)
                  {
                      sum += data[i, k] * b.data[k, j];
                  }
                  return sum;
              });
        }
        public Matrix multiply(double n)
        {
            return this
              .Map((e, i, j) => e * n);
        }



        public int rows { get; }
        public int cols { get; }
        public double[,] data { get; }
    }
}
