namespace VisitMe.Api.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string VisitorName { get; set; }
        public string PlaceToVisit { get; set; }
        public string PersonalID { get; set; }
        public DateTime Date { get; set; }

    }
}
