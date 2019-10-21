using FlappyBird.Visual.Game;
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
            r = Constants.r;
        }
        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            data = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                data[i] = new double[cols];
            }
        }

        public static Matrix fromArray(double[] arr)
        {
            return new Matrix(arr.Length, 1).map((e, i, _) => arr[i]);
        }

        public delegate double MapFunctionWithIndices(double value, int i, int j);
        public delegate double MapFunction(double value);
        

        public Matrix copy()
        {
            var matrix = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix.data[i][j] = this.data[i][j];
                }
            }
            return matrix;
        }

        public Matrix map(MapFunctionWithIndices func)
        {
            Matrix matrix = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix.data[i][j] = func(data[i][j], i, j);
                }
            }
            return matrix;
        }
        public Matrix map(MapFunction func)
        {
            Matrix matrix = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix.data[i][j] = func(data[i][j]);
                }
            }
            return matrix;
        }

        public Matrix substract(Matrix b)
        {
            if (rows != b.rows || cols != b.cols)
            {
                throw new Exception("Columns and Rows of A must match Columns and Rows of B.");
            }
            // Return a new Matrix a-b
            return this
              .map((_, i, j) => data[i][j] - b.data[i][j]);
        }

        public double[] toArray()
        {
            List<double> res = new List<double>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    res.Add(data[i][j]);
                }
            }
            return res.ToArray();
        }

        public Matrix randomize()
        {
            return this.map((e, _, __) => r.NextDouble() * 2 - 1);
        }

        public Matrix add(Matrix n)
        {
            if (this.rows != n.rows || this.cols != n.cols)
            {
                throw new Exception("Columns and Rows of A must match Columns and Rows of B.");
            }
            return this.map((e, i, j) => e + n.data[i][j]);
        }
        public Matrix add(double n)
        {
            return this.map((e, i, j) => e + n);
        }

        public Matrix transpose()
        {
            return this
              .map((_, i, j) => data[j][i]);
        }

        public Matrix multiply(Matrix b)
        {
            // Matrix product
            if (cols != b.rows)
            {
                throw new Exception("Columns of A must match rows of B.");

            }

            return new Matrix(this.rows, b.cols)
              .map((e, i, j) =>
              {
                  // Dot product of values in col
                  double sum = 0;
                  for (int k = 0; k < cols; k++)
                  {
                      sum += data[i][k] * b.data[k][j];
                  }
                  return sum;
              });
        }
        public Matrix multiply(double n)
        {
            return this
              .map((e, i, j) => e * n);
        }



        public int rows { get; }
        public int cols { get; }
        public double[][] data { get; }
    }
}
