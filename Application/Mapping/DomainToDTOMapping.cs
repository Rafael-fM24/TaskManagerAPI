using Application.DTOs.TaskItem;
using Application.DTOs.TaskNote;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class DomainToDTOMapping :  Profile
{
    public DomainToDTOMapping()
    {
        CreateMap<TaskItem, TaskItemDTO>();
        CreateMap<CreateTaskItemDTO, TaskItem>();

        CreateMap<TaskNote, TaskNoteDTO>();
    }
}