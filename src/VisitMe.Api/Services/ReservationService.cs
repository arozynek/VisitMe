using VisitMe.Api.Models;

namespace VisitMe.Api.Services
{
    public class ReservationService
    {
        private static int _id = 1;
        private static readonly List<Reservation> Reservations = new();
        private static readonly List<string> PlaceToVisit = new()
        {
            "PM", "stomatologist", "pharmacist", "cardiologist", "surgeon"
        };

        public Reservation Get(int id)
            =>  Reservations.SingleOrDefault(x => x.Id == id);
       

        public IEnumerable<Reservation> GetAll() 
            => Reservations;
       
        public int? Create(Reservation reservation)
        {

            var now = DateTime.UtcNow.Date;
            var pastDays = now.DayOfWeek is DayOfWeek.Sunday? 7 : (int)now.DayOfWeek;
            var remainingDays = 7 - pastDays;

            if (PlaceToVisit.All(x => x != reservation.PlaceToVisit))
            {
                return default;
            }

            if(!(reservation.Date.Date > now && reservation.Date.Date < now.AddDays(remainingDays)))
            {
                return default;
            }

            var reservationAlreadyExists = Reservations
                .Any(x => x.PlaceToVisit == reservation.PlaceToVisit && x.Date.Date == reservation.Date.Date);

            if (reservationAlreadyExists)
            {
                return default;
            }

            reservation.Id = _id;
            _id++;
            Reservations.Add(reservation);
            return reservation.Id;
        }

        public bool Update(Reservation reservation)
        {
            var existingReservation = Reservations.SingleOrDefault(x => x.Id == reservation.Id);
            if (existingReservation == null)
            {
                return false;
            }
            if (existingReservation.Date <= DateTime.UtcNow) 
            {
                return false;
            }
            existingReservation.PersonalID = reservation.PersonalID;
            return true;
        }

        public bool Delete(int id)
        {
            var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);
            if (existingReservation == null)
            {
                return false;
            }
            Reservations.Remove(existingReservation);
            return true;
        }
    }
}
