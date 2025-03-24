using SleepGo.App.Interfaces;

namespace SleepGo.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SleepGoDbContext _context;
        public IAmenityRepository AmenityRepository { get; private set; }
        public IHotelRepository HotelRepository { get; private set; }
        public IImageRepository ImageRepository { get; private set; }
        public IReservationRepository ReservationRepository { get; private set; }
        public IReviewRepository ReviewRepository { get; private set; }
        public IRoomRepository RoomRepository { get; private set; }
        public IUserProfileRepository UserProfileRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IPaymentRepository PaymentRepository { get; private set; }

        public UnitOfWork(SleepGoDbContext context, IAmenityRepository amenityRepository, IHotelRepository hotelRepository,
            IImageRepository imageRepository, IReservationRepository reservationRepository, IReviewRepository reviewRepository,
            IRoomRepository roomRepository, IUserProfileRepository userProfileRepository, IUserRepository userRepository,
            IPaymentRepository paymentRepository)
        {
            _context = context;
            AmenityRepository = amenityRepository;
            HotelRepository = hotelRepository;
            ImageRepository = imageRepository;
            ReservationRepository = reservationRepository;
            ReviewRepository = reviewRepository;
            RoomRepository = roomRepository;
            UserProfileRepository = userProfileRepository;
            UserRepository = userRepository;
            PaymentRepository = paymentRepository;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
