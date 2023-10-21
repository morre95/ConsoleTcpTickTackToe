namespace ConsoleTcpTickTackToe
{
    public class Square
    {
        private Player player = Player.None;

        private char num;

        public char Mark { get { if (player != Player.None) return (char)player; else return num; } }

        public Square(char number)
        {
            num = number;
        }

        public bool SetPlayer(Player player)
        {
            if (this.player != Player.None) { return false; }
            this.player = player;
            return true;
        }



    }
}
