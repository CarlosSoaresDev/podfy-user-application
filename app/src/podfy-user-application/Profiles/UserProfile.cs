using AutoMapper;
using podfy_user_application.Entity;

namespace podfy_user_application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Model.UserSingUpRequest, User>()
            .AfterMap((source, destination) =>
            {
                destination.Id = Guid.NewGuid();
                destination.CreatedAt = DateTime.Now;
            });
    }
}

