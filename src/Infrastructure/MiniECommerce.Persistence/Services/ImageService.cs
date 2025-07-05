using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Image;
using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Persistence.Services;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;

    public ImageService(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<List<ImageDto>> GetAllByProductIdAsync(Guid productId)
    {
        var images = _imageRepository
            .GetByFiltered(i => i.ProductId == productId)
            .ToList();

        return images.Select(i => new ImageDto
        {
            Id = i.Id,
            ImageUrl = i.ImageUrl,
            IsMain = i.IsMain
        }).ToList();
    }

    public async Task<ImageDto?> GetByIdAsync(Guid id)
    {
        var image = await _imageRepository.GetByIdAsync(id);
        if (image == null)
            return null;

        return new ImageDto
        {
            Id = image.Id,
            ImageUrl = image.ImageUrl,
            IsMain = image.IsMain
        };
    }

    public async Task CreateAsync(CreateImageDto dto)
    {
        var image = new Image
        {
            Id = Guid.NewGuid(),
            ImageUrl = dto.ImageUrl,
            IsMain = dto.IsMain,
            ProductId = dto.ProductId
        };

        await _imageRepository.AddAsync(image);
        await _imageRepository.SaveChangeAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateImageDto dto)
    {
        var image = await _imageRepository.GetByIdAsync(id);
        if (image == null)
            throw new Exception("Image not found");

        image.ImageUrl = dto.ImageUrl;
        image.IsMain = dto.IsMain;

        _imageRepository.Update(image);
        await _imageRepository.SaveChangeAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var image = await _imageRepository.GetByIdAsync(id);
        if (image == null)
            throw new Exception("Image not found");

        _imageRepository.Delete(image);
        await _imageRepository.SaveChangeAsync();
    }
}
