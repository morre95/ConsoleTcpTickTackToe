namespace ConsoleTcpTickTackToe
{
    public class Board
    {
        private Square[] squares;

        private int size = 3;

        public Board(int size)
        {
            this.size = size;
            squares = new Square[size * size];
            for (int i = 0; i < size * size; i++)
            {
                squares[i] = new Square(char.Parse((i + 1).ToString()));
            }
        }

        public bool SetPlayer(int index, Player player)
        {
            return squares[index].SetPlayer(player);
        }

        public char GetSquare(int index)
        {
            return squares[index].Mark;
        }

        public List<char> ToList()
        {
            List<char> list = new();
            foreach (Square sq in squares)
            {
                list.Add(sq.Mark);
            }
            return list;
        }

        public Result CheckWin()
        {

            int[,] winConditions =
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 },
                { 1, 4, 7 },
                { 2, 5, 8 },
                { 3, 6, 9 },
                { 1, 5, 9 },
                { 3, 5, 7 }
            };

            for (int i = 0; i < winConditions.GetLength(0); i++)
            {
                int a = winConditions[i, 0] - 1;
                int b = winConditions[i, 1] - 1;
                int c = winConditions[i, 2] - 1;
                if (squares[a].Mark == squares[b].Mark && squares[b].Mark == squares[c].Mark)
                {
                    return Result.Winner;
                }
            }

            if (squares.All(cell => cell.Mark != '1' && cell.Mark != '2' && cell.Mark != '3' && cell.Mark != '4' && cell.Mark != '5' && cell.Mark != '6' && cell.Mark != '7' && cell.Mark != '8' && cell.Mark != '9'))
            {
                return Result.Draw;
            }

            return Result.None;
        }

        public void Print()
        {
            // TODO: Detta försöka att göra Print() med dynamisk fungerar inte då det är chars som inte klarar mer än ett tecken.
            // Och när man ändrar till string så blir det knas när det är två tecken eller mera eftersom två tecken tar mer plats
            string str = RepeatPattern();
            str += "\n";
            for (int i = 0; i < size * size; i++)
            {
                if (i % size == 0 && i != 0)
                {
                    str += "\n";
                    str += RepeatPattern('-');
                    str += "\n";
                }
                str += $"  {GetSquare(i)}  ";
                if (i % size != size - 1)
                {
                    str += "|";
                }
            }
            str += "\n";
            str += RepeatPattern();
            Console.WriteLine(str);
        }

        private string RepeatPattern(char c = ' ')
        {
            string str = "";
            for (int i = 0; i < size; i++)
            {
                if (i == 0)
                {
                    str += new string(c, 5);
                }
                else
                {
                    str += $"|{new string(c, 5)}";
                }
            }

            return str;
        }
    }

}
