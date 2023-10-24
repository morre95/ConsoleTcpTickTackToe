using System.Text.Json.Serialization;

namespace ConsoleTcpTickTackToe
{
    public class Board
    {
        private Square[] squares = new Square[9];

        [JsonIgnore]
        public Square[] Squares => squares;

        [JsonPropertyName("JsonSquares")]
        public char[] MySquaresAsArray
        {
            get
            {
                char[] sq = new char[squares.Length];
                for (int i = 0; i < squares.Length; i++)
                {
                    sq[i] = squares[i].Badge;
                }
                return sq;
            }
            set
            {
                int numRows = value.Length;
                for (int i = 0; i < value.Length; i++)
                {
                    Square square = new Square(value[i]);
                    if (value[i] == 'X') square.Player = Player.You;
                    else if (value[i] == 'O') square.Player = Player.Server;
                    else square.Player = Player.None;
                    squares[i] = square;
                }
            }
        }

        private int size = 3;

        [JsonIgnore]
        private readonly int[,] winningCombination =
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

        Stack<int> moves = new();

        public Board() { }

        public Board(int size)
        {
            this.size = size;
            squares = new Square[size * size];
            for (int i = 0; i < size * size; i++)
            {
                squares[i] = new Square(char.Parse((i + 1).ToString()));
            }
        }

        public int[,] GetPossibleMoves() => winningCombination;

        public bool SetPlayerMove(int index, Player player)
        {
            return squares[index - 1].SetPlayerMove(player);
        }

        public char GetSquare(int index)
        {
            return squares[index].Badge;
        }

        public List<char> ToList()
        {
            List<char> list = new();
            foreach (Square sq in squares)
            {
                list.Add(sq.Badge);
            }
            return list;
        }

        public State GetState()
        {
            for (int i = 0; i < winningCombination.GetLength(0); i++)
            {
                int a = winningCombination[i, 0] - 1;
                int b = winningCombination[i, 1] - 1;
                int c = winningCombination[i, 2] - 1;
                if (squares[a].Badge == squares[b].Badge && squares[b].Badge == squares[c].Badge)
                {
                    return State.Winner;
                }
            }

            if (squares.All(cell => cell.Badge != '1' && cell.Badge != '2' && cell.Badge != '3' && cell.Badge != '4' && cell.Badge != '5' && cell.Badge != '6' && cell.Badge != '7' && cell.Badge != '8' && cell.Badge != '9'))
            {
                return State.Draw;
            }

            return State.None;
        }

        

        public bool MakeMove(int move)
        {
            Player player = Player.Server;
            if (moves.Count % 2 == 0)
            {
                player = Player.You;
            }

            if (SetPlayerMove(move, player))
            {
                moves.Push(move);
                return true;
            }
            return false;
        }

        public void Undo()
        {
            int last = moves.Pop();
            squares[last - 1] = new Square(char.Parse(last.ToString()));
        }

        public Player GetWinner() => (moves.Count % 2 != 0) ? Player.You : Player.Server;

        public Player WhosTurn() => (moves.Count % 2 == 0) ? Player.You : Player.Server;

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
