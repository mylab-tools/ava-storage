﻿using AvaService.Infrastructure;
using AvaService.Infrastructure.Services;
using AvaService.Infrastructure.Tools;
using AvaStorage.Domain.ValueObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace AvaStorage.Infrastructure.ImageSharp
{
    public partial class PictureTools : IPictureTools
    {
        public async Task<AvatarPicture> ResizeAndSquareCropAsync(AvatarPicture origin, int targetSize, CancellationToken cancellationToken)
        {
            using var readMem = new MemoryStream(origin.Binary.ToArray());
            using var img = await Image.LoadAsync(readMem, cancellationToken);

            if (img.Width == targetSize && img.Height == targetSize)
                return origin;

            var newSize = PictureResizeCalculator.Calculate(new ImageSize(img.Width, img.Height), targetSize);
            var crop = PictureCropCalculator.CalculateSquare(new ImageSize(img.Width, img.Height));

            img.Mutate(x => x
                    .Resize(newSize.Width, newSize.Height)
                    .Crop(new Rectangle(crop.X, crop.Y, crop.Width, crop.Height)));

            using var writeMem = new MemoryStream();
            await img.SaveAsync(writeMem, new PngEncoder(), cancellationToken);

            return new AvatarPicture(writeMem.ToArray(), new PictureSize(img.Width, img.Height));

        }
    }
}
