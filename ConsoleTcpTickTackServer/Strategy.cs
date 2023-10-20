namespace ConsoleTcpTickTackServer
{
    public class Strategy
    {
        private List<char> list = new();

        public Strategy(List<char> tableList) 
        { 
            list = tableList;
        }

        public int OWin()
        {
            /*
             * 1,2,3
             * 4,5,6
             * 7,8,9
             */
            int[][] winPatterns = new int[8][]
            {
                new int[3] {1, 2, 3},
                new int[3] {4, 5, 6},
                new int[3] {7, 8, 9},
                new int[3] {1, 4, 7},
                new int[3] {2, 5, 8},
                new int[3] {3, 6, 9},
                new int[3] {1, 5, 9},
                new int[3] {3, 5, 7}
            };

            for (int i = 0; i < winPatterns.Length; i++)
            {
                int[] pattern = winPatterns[i];
                int a = pattern[0]; //1
                int b = pattern[1]; //5
                int c = pattern[2]; //9

                int? winMove = IsWining(a, b, c, 'O');
                if (winMove != null)
                {
                    return (int)winMove;
                }

                winMove = IsWining(a, b, c, 'X');

                if (winMove != null)
                {
                    return (int)winMove;
                }
            }


            if (list.FindAll(x => x == 'X').Count == 1)
            {
                if (list[1] == 'X' && list[5] == 'O')
                {
                    return 5;
                }
            }


            if (list[1] == 'X' && list[5] == 'O')
            {
                if (list[9] == 'X')
                {
                    if (list[2] != 'O') return 2;
                    if (list[4] != 'O') return 4;
                    if (list[6] != 'O') return 6;
                    if (list[8] != 'O') return 8;
                }
                else if (list[6] == 'X')
                {
                    if (list[2] != 'O') return 2;
                    if (list[3] != 'O') return 3;
                    if (list[8] != 'O') return 8;
                    if (list[9] != 'O') return 9;
                }
            }


            Random rnd = new();

            return rnd.Next(1, 10);
        }

        public int? IsWining(int a, int b, int c, char player = 'O')
        {
            char op = player == 'X' ? 'O' : 'X';

            if (list[a] == player && list[b] == player && list[c] != op)
            {
                return c;
            }
            else if (list[b] == player && list[c] == player && list[a] != op)
            {
                return a;
            }
            else if (list[a] == player && list[c] == player && list[b] != op)
            {
                return b;
            }
            return null;
        }
    }
}
