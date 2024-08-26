namespace ChessMarathon.Models
{
    public class Players
    {

        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string Country { get; set; }
        public int CurrentWorldRanking { get; set; }
        public int TotalMatchesPlayed { get; set; }
    }
}
