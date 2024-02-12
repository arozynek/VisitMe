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
        public ActionResult<IEnumerable<Reservation>> Get() => Ok(Reservations);

        [HttpGet(template: "{id:int}")]
        public ActionResult<Reservation> Get(int id)
        {
            var reservation = Reservations.SingleOrDefault(x => x.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult Post(Reservation reservation)
        {
            if (PlaceToVisit.All(x => x != reservation.PlaceToVisit))
            {
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

            return CreatedAtAction(nameof(Get), new { id = reservation.Id }, null);
        }
        [HttpPut(template:"{id:int}")]
        public ActionResult Put(int id,Reservation reservation) 
        {
            var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);
            if (existingReservation == null)
            {
                return NotFound();
            }
            existingReservation.PersonalID = reservation.PersonalID;
            return NoContent();
        }
    }
}
