using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using podfy_user_application.Entity;
using podfy_user_application.Model;
using podfy_user_application.Repository;
using podfy_user_application.Utils;

namespace podfy_user_application.Service;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISQSQueueService _sQSQueueService;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository, IMapper mapper, ISQSQueueService sQSQueueService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _sQSQueueService = sQSQueueService; 
    }

    public async Task<UserTokenResponse> UserSingInAsync(UserSingInRequest userLoginRequest)
    {
        var conditions = new List<ScanCondition>()
        {
            new ScanCondition("Password", ScanOperator.Equal, userLoginRequest.Password),
            new ScanCondition("Email", ScanOperator.Equal, userLoginRequest.Email)
        };

        var user = await _userRepository.FindUserAsync(conditions);

        return user?.GenerateJwtToken();

    }

    public async Task<UserTokenResponse> UserSingUpAsync(UserSingUpRequest userLoginRequest)
    {
        var userMapped = _mapper.Map<User>(userLoginRequest);

        await _userRepository.SaveUserAsync(userMapped);
        await _sQSQueueService.SendMessageAsync(userMapped);

        return userMapped.GenerateJwtToken();
    }
}
