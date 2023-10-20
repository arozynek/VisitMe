using Microsoft.AspNetCore.Mvc;
using VisitMe.Api.Models;

namespace VisitMe.Api.Controllers
{
    [ApiController]
    [Route(template: "reservations")]
    public class ReservationsController : ControllerBase
    {
        private static int _id = 1;
        private static readonly List<Reservation> Reservations = new();
        private static readonly List<string> PlaceToVisit = new()
        {
            "PM", "stomatologist", "pharmacist", "cardiologist", "surgeon"
        };

        [HttpGet]
        public IEnumerable<Reservation> Get() => Reservations;

        [HttpGet(template:"{id:int}")]
        public Reservation Get(int id)
        {
            var reservation = Reservations.SingleOrDefault(x => x.Id == id);
            if (reservation == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return default;
            }
            return reservation;
        }

        [HttpPost]
        public ActionResult Post(Reservation reservation)
        {
            if (PlaceToVisit.All(x => x != reservation.PlaceToVisit))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest();
            }

            reservation.Date = DateTime.UtcNow.AddDays(1).Date;
            var reservationAlreadyExists = Reservations
                .Any(x => x.PlaceToVisit == reservation.PlaceToVisit && x.Date.Date == reservation.Date.Date);
            if (reservationAlreadyExists)
            {
                return BadRequest();
            }

            reservation.Id = _id;
            _id++;
            Reservations.Add(reservation);

            return CreatedAtAction(nameof(Get), new {id = reservation.Id}, null );
        }
    }
}
