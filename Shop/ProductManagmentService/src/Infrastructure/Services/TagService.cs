using Application.IManagers;
using Application.IService;
using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

namespace Infrastructure.Service;

public class TagService : ITagService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    public TagService(IRepositoryManager repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<TagDTO> CreateTag(TagDTO tagDTO)
    {
        var tag = _mapper.Map<Tag>(tagDTO);
        var createdTagDTO = await _repository.TagRepository.CreateTag(tag);
        await _repository.SaveAsync();
        return _mapper.Map<TagDTO>(createdTagDTO);
    }

    public async Task<List<TagDTO>> GetAll()
    {
        return _mapper.Map<List<TagDTO>>(await _repository.TagRepository.GetAll());
    }

    public async Task<TagDTO> GetById(Guid id)
    {
        return _mapper.Map<TagDTO>(await _repository.TagRepository.GetById(id));
    }

    public async Task RemoveTag(Guid id)
    {
        await _repository.TagRepository   
            .RemoveTag(id);
        await _repository.SaveAsync();
    }

    public async Task<TagDTO> UpdateTag(Guid id, TagDTO tagDTO)
    {
        var tag = _mapper.Map<Tag>(tagDTO);
        var updatedTag = await _repository.TagRepository.UpdateTag(id, tag);
        await _repository.SaveAsync();
        return _mapper.Map<TagDTO>(updatedTag);
    }
}