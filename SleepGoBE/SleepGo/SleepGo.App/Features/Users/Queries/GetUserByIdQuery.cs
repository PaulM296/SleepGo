using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Users.Queries
{
    public record GetUserByIdQuery(Guid userId) : IRequest<ResponseUserDto>;

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ResponseUserDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.userId);

            if(user == null)
            {
                throw new UserNotFoundException($"Could not get the user with Id {request.userId}, because it doesn't exist!");
            }

            _logger.LogInformation($"User successfully retrieved!");

            return _mapper.Map<ResponseUserDto>(user);
        }
    }
}
