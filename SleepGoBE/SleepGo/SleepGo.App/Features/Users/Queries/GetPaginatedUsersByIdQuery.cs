using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Users.Queries
{
    public record GetPaginatedUsersByIdQuery(PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseUserDto>>;

    public class GetPaginatedUsersByIdQueryHandler : IRequestHandler<GetPaginatedUsersByIdQuery, PaginationResponseDto<ResponseUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPaginatedUsersByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetPaginatedUsersByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPaginatedUsersByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ResponseUserDto>> Handle(GetPaginatedUsersByIdQuery request, CancellationToken cancellationToken)
        {
            var paginatedUsers = await _unitOfWork.UserRepository.GetPaginatedUsersByIdAsync(request.paginationRequestDto.PageIndex, request.paginationRequestDto.PageSize);

            if(paginatedUsers.Items.Count == 0)
            {
                throw new UserNotFoundException($"There are no users on the platform!");
            }

            var mappedUsers = _mapper.Map<List<ResponseUserDto>>(paginatedUsers.Items);

            var response = new PaginationResponseDto<ResponseUserDto>(
                items: mappedUsers,
                pageIndex: paginatedUsers.PageIndex,
                totalPages: paginatedUsers.TotalPages
            );

            _logger.LogInformation($"Users successfully retrieved!");

            return response;
        }
    }
}
