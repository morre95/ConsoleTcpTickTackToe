namespace ConsoleTcpTickTackToe
{
    public class Square
    {
        private Player player = Player.None;

        private char num;

        public Player Player { get { return player; } set { player = value; } }

        public char Mark { get { if (player != Player.None) return (char)player; else return num; } set { num = value; } }

        public Square(char number)
        {
            num = number;
        }

        public bool SetPlayerMove(Player player)
        {
            if (this.player != Player.None) { return false; }
            this.player = player;
            return true;
        }



    }
}
