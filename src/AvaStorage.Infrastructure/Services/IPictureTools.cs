﻿using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Infrastructure.Services
{
    public interface IPictureTools
    {
        Task<AvatarPicture?> DeserializeAsync(byte[] binary, CancellationToken cancellationToken);

        Task<AvatarPicture> ResizeAndSquareCropAsync(AvatarPicture origin, int targetSize, CancellationToken cancellationToken);
    }
}