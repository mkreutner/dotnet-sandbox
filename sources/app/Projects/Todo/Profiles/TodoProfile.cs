// File: Projects/Todo/Profiles/TodoProfile.cs
using AutoMapper;
using Todo.DTOs;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        // 1. Pour la création : On mappe du DTO envoyé par le client vers notre Entité DB
        CreateMap<TodoCreateDto, TodoItem>();

        // 2. Pour la réponse : On mappe de l'Entité DB vers le DTO de sortie
        CreateMap<TodoItem, TodoResponseDto>();
    }
}

