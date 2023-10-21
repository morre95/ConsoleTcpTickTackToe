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
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {GetSquare(0)}  |  {GetSquare(1)}  |  {GetSquare(2)}");
            Console.WriteLine("-----|-----|-----");
            Console.WriteLine($"  {GetSquare(3)}  |  {GetSquare(4)}  |  {GetSquare(5)}");
            Console.WriteLine("-----|-----|-----");
            Console.WriteLine($"  {GetSquare(6)}  |  {GetSquare(7)}  |  {GetSquare(8)}");
            Console.WriteLine("     |     |     ");
        }

    }

}
