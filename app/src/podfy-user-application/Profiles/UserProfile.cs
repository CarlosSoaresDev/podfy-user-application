using AutoMapper;
using podfy_user_application.Entity;
using System.Diagnostics.CodeAnalysis;

namespace podfy_user_application.Profiles;

[ExcludeFromCodeCoverage]
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

