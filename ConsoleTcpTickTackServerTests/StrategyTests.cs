using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleTcpTickTackServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTcpTickTackServer.Tests
{
    [TestClass()]
    public class StrategyTests
    {
        [TestMethod()]
        public void StrategyTest()
        {
            char[] arr = { '0',
                '1', '2', '3',
                '4', '5', '6',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);

            Assert.AreEqual(list.Count, strat.Board.Count);
        }

        [TestMethod()]
        public void OWinTest_StartX1Get5()
        {
            char[] arr = { '0',
                'X', '2', '3',
                '4', '5', '6',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(5, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_NextXTakes9Get2()
        {
            char[] arr = { '0',
                'X', '2', '3',
                '4', 'O', '6',
                '7', '8', 'X'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(2, strat.OWin());
        }


        // TODO: den borde få 8 här. Men den skyddar sig kör 7
        [TestMethod()]
        public void OWinTest_NextXTakes4Get8ForWin()
        {
            char[] arr = { '0',
                'X', 'O', '3',
                'X', 'O', '6',
                '7', '8', 'X'
            };

            List<char> list = new(arr);
            Strategy strat = new(list);

            int[][] winPatterns = new int[][]
            {
                new int[] {1, 2, 3},
                new int[] {4, 5, 6},
                new int[] {7, 8, 9},
                new int[] {1, 4, 7},
                new int[] {2, 5, 8},
                new int[] {3, 6, 9},
                new int[] {1, 5, 9},
                new int[] {3, 5, 7}
            };

            for (int i = 0; i < winPatterns.Length; i++)
            {
                int[] pattern = winPatterns[i];
                int a = pattern[0]; 
                int b = pattern[1]; 
                int c = pattern[2];

                int? winMove = strat.IsWining(a, b, c, 'O');
                if (winMove != null)
                {
                    Assert.AreEqual(8, winMove);
                    break;
                }

            }

            Assert.AreEqual(8, strat.OWin());
        }

        [TestMethod()]
        public void IsWiningTest_Get8()
        {
            char[] arr = { '0',
                'X', 'O', '3',
                'X', 'O', '6',
                '7', '8', 'X'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(8, strat.IsWining(2, 5, 8));
        }

        [TestMethod()]
        public void IsWiningTest_Get7()
        {
            char[] arr = { '0',
                'O', 'O', '3',
                'O', 'X', 'X',
                '7', '8', 'X'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(7, strat.IsWining(1,4,7));
        }


        [TestMethod()]
        public void IsWiningTest_GetNull()
        {
            char[] arr = { '0',
                'O', '3', '3',
                '4', 'O', 'X',
                '7', '8', 'X'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(null, strat.IsWining(3, 6, 9));
        }

        [TestMethod()]
        public void OWinTest_X2X3Get1()
        {
            char[] arr = { '0',
                '1', 'X', 'X',
                '4', 'O', '6',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(1, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X1X2Get3()
        {
            char[] arr = { '0',
                'X', 'X', '3', 
                '4', 'O', '6', 
                '7', '8', '9' 
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(3, strat.OWin());
        }

        

        [TestMethod()]
        public void OWinTest_X5X6Get4()
        {
            char[] arr = { '0',
                'O', '2', '3',
                '4', 'X', 'X',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(4, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X4X5Get6()
        {
            char[] arr = { '0',
                'O', '2', '3',
                'X', 'X', '6',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(6, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X8X9Get7()
        {
            char[] arr = { '0',
                'O', '2', '3',
                '4', '5', '6',
                '7', 'X', 'X'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(7, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X7X8Get9()
        {
            char[] arr = { '0',
                'O', '2', '3',
                '4', '5', '6',
                'X', 'X', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(9, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X7X4Get1()
        {
            char[] arr = { '0',
                '1', '2', '3',
                'X', '5', '6',
                'X', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(1, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X1X4Get7()
        {
            char[] arr = { '0',
                'X', '2', '3',
                'X', '5', '6',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(7, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X5X8Get2()
        {
            char[] arr = { '0',
                '1', '2', '3',
                '4', 'X', '6',
                '7', 'X', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(2, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X2X5Get8()
        {
            char[] arr = { '0',
                '1', 'X', '3',
                '4', 'X', '6',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(8, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X3X6Get9()
        {
            char[] arr = { '0',
                '1', '2', 'X',
                '4', '5', 'X',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(9, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X1X5Get9()
        {
            char[] arr = { '0',
                'X', '2', '3',
                '4', 'X', '6',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(9, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X5X9Get1()
        {
            char[] arr = { '0',
                '1', '2', '3',
                '4', 'X', '6',
                '7', '8', 'X'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(1, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X3X5Get7()
        {
            char[] arr = { '0',
                '1', '2', 'X',
                '4', 'X', '6',
                '7', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(7, strat.OWin());
        }

        [TestMethod()]
        public void OWinTest_X5X7Get3()
        {
            char[] arr = { '0',
                '1', '2', '3',
                '4', 'X', '6',
                'X', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(3, strat.OWin());
        }



        [TestMethod()]
        public void OWinTest_X1X5Get9_Test2()
        {
            char[] arr = { '0',
                'X', '2', 'O',
                'X', 'X', '6',
                'O', '8', '9'
            };
            List<char> list = new(arr);
            Strategy strat = new(list);
            Assert.AreEqual(6, strat.OWin());
            Assert.AreNotEqual(9, strat.OWin());
        }
    }
}