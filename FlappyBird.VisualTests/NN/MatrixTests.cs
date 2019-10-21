using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlappyBird.NN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlappyBird.Visual.NN;

namespace FlappyBird.NN.Tests
{
    [TestClass()]
    public class MatrixTests
    {
        [TestMethod()]
        public void AddEscalarTest()
        {
            var m = new Matrix(3, 3);
            m.data[0] = new double[] { 1, 2, 3 };
            m.data[1] = new double[] { 4, 5, 6 };
            m.data[2] = new double[] { 7, 8, 9 };
            var m2 = m.add(1);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(m.data[i][j] + 1, m2.data[i][j]);
                }
            }
        }
        [TestMethod()]
        public void AddMatrixTest()
        {
            var m = new Matrix(3, 3);
            m.data[0] = new double[] { 1, 2, 3 };
            m.data[1] = new double[] { 4, 5, 6 };
            m.data[2] = new double[] { 7, 8, 9 };
            var m2 = m.add(m);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(m.data[i][j] * 2, m2.data[i][j]);
                }
            }
        }
    }
}