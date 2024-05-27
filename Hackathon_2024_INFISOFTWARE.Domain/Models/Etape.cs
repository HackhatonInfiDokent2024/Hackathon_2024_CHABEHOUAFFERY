namespace Hackathon_2024_INFISOFTWARE.Domain.Models
{
    public class Etape
    {
        public string Nom { get; set; }

        public string Description { get; set; }

        public List<ChampRequis> ChampsRequis { get; set; }

        public Participants Participants { get; set; }

        public List<ActionPossible> ActionsPossibles { get; set; }
    }
}
