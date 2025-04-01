using AvaStorage.Domain;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Application.Services
{
    public interface IAvaCache
    {
        Task<IAvatarFile?> GetAsync(AvatarId avatarId, CancellationToken cancellationToken);
        Task SetAsync(AvatarId avatarId, IAvatarFile avatarFile, CancellationToken cancellationToken);
    }
}
