using Application.DTOs.TaskItem;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class DomainToDTOMapping :  Profile
{
    public DomainToDTOMapping()
    {
        CreateMap<TaskItem, TaskItemDTO>();
        CreateMap<CreateTaskItemDTO, TaskItem>();

        CreateMap<TaskItem, TaskItemDTO>();
    }
}