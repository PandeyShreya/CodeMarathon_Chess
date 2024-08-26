namespace ChessMarathon.Models
{
    public class PlayerPerformance
    {

        public string Name { get; set; }
        public  int TotalMatchesPlayed { get; set; }
        public int TotalWinCount { get; set; }
        public Double WinPercentage { get; set; }
    }
}
