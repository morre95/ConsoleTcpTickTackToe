
using ConsoleTcpTickTackToe;

namespace ConsoleTcpTickTackServer
{
    public class TicTacAI
    {
        private static int Minimax(bool isMaxTurn, Player maximizerMark, Board board)
        {
            State state = board.GetState();
            if (state == State.Draw)
            {
                return 0;
            }
            else if (state == State.Winner)
            {
                return (board.GetWinner() == maximizerMark) ? 1 : -1;
            }

            List<int> scores = new List<int>();
            foreach (int move in board.GetPossibleMoves())
            {
                if (!board.MakeMove(move)) continue;
                int score = Minimax(!isMaxTurn, maximizerMark, board);
                board.Undo();
                scores.Add(score);
            }

            return isMaxTurn ? scores.Max() : scores.Min();
        }

        public static int MakeBestMove(Board ticTacBoard, Player aiPlayer)
        {
            int[,] winingMoves = ticTacBoard.GetPossibleMoves();
            for (int i = 0; i < winingMoves.GetLength(0); i++)
            {
                int a = winingMoves[i, 0] - 1; //1
                int b = winingMoves[i, 1] - 1; //5
                int c = winingMoves[i, 2] - 1; //9

                // TODO: 'X' och 'O' borde vara olika beroende på vad aiPlayer har för värde
                if (ticTacBoard.Squares[a].Mark == 'X' && ticTacBoard.Squares[b].Mark == 'X' && ticTacBoard.Squares[c].Mark != 'O')
                {
                    return c + 1;
                }
                else if (ticTacBoard.Squares[b].Mark == 'X' && ticTacBoard.Squares[c].Mark == 'X' && ticTacBoard.Squares[a].Mark != 'O')
                {
                    return a + 1;
                }
                else if (ticTacBoard.Squares[a].Mark == 'X' && ticTacBoard.Squares[c].Mark == 'X' && ticTacBoard.Squares[b].Mark != 'O')
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
