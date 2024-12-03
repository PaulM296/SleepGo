namespace SleepGo.App.Interfaces
{
    public interface IUnitOfWork
    {
        public IAmenityRepository AmenityRepository { get; }
        public IHotelRepository HotelRepository { get; }
        public IImageRepository ImageRepository { get; }
        public IReservationRepository ReservationRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        public IRoomRepository RoomRepository { get; }
        public IUserProfileRepository UserProfileRepository { get; }
        public IUserRepository UserRepository { get; }

        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
