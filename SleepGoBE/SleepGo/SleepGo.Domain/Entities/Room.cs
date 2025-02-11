﻿using SleepGo.Domain.Entities.BaseEntities;
using SleepGo.Domain.Enums;

namespace SleepGo.Domain.Entities
{
    public class Room : BaseEntity
    {
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public RoomType RoomType { get; set; }
        public decimal Price { get; set; }
        public int RoomNumber { get; set; }
        public bool Balcony { get; set; }
        public bool AirConditioning { get; set; }
        public bool Kitchenette { get; set; }
        public bool Hairdryer { get; set; }
        public bool TV { get; set; }
        public bool IsReserved { get; set; } = false;
        public ICollection<Reservation> Reservations { get; set; }
    }
}
