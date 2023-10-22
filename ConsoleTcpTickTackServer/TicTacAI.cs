
using ConsoleTcpTickTackToe;

namespace ConsoleTcpTickTackServer
{
    public class TicTacAI
    {
        private static int Minimax(bool isMaxTurn, Player maximiserPlayer, Board board)
        {
            State state = board.GetState();
            if (state == State.Draw)
            {
                return 0;
            }
            else if (state == State.Winner)
            {
                return (board.GetWinner() == maximiserPlayer) ? 1 : -1;
            }

            List<int> scores = new List<int>();
            foreach (int move in board.GetPossibleMoves())
            {
                if (!board.MakeMove(move)) continue;
                int score = Minimax(!isMaxTurn, maximiserPlayer, board);
                board.Undo();
                scores.Add(score);
            }

            return isMaxTurn ? scores.Max() : scores.Min();
        }

        public static int MakeBestMove(Board ticTacBoard, Player aiPlayer)
        {
            char op = 'X';
            char ai = 'O';
            if (aiPlayer == Player.You)
            {
                op = 'O';
                ai = 'X';
            }
            int[,] winingMoves = ticTacBoard.GetPossibleMoves();
            for (int i = 0; i < winingMoves.GetLength(0); i++)
            {
                int a = winingMoves[i, 0] - 1;
                int b = winingMoves[i, 1] - 1;
                int c = winingMoves[i, 2] - 1;

                if (ticTacBoard.Squares[a].Badge == op && ticTacBoard.Squares[b].Badge == op && ticTacBoard.Squares[c].Badge != ai)
                {
                    return c + 1;
                }
                else if (ticTacBoard.Squares[b].Badge == op && ticTacBoard.Squares[c].Badge == op && ticTacBoard.Squares[a].Badge != ai)
                {
                    return a + 1;
                }
                else if (ticTacBoard.Squares[a].Badge == op && ticTacBoard.Squares[c].Badge == op && ticTacBoard.Squares[b].Badge != ai)
                {
                    return b + 1;
                }
            }

            int bestScore = int.MinValue;
            int bestMove = -1;
            foreach (int move in ticTacBoard.GetPossibleMoves())
            {
                if (!ticTacBoard.MakeMove(move)) continue;
                int score = Minimax(false, aiPlayer, ticTacBoard);
                ticTacBoard.Undo();
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }
            return bestMove;
        }
    }
}
