// File: Projects/Todo/Validators/TodoCreateDtoValidator.cs
using FluentValidation;
using Todo.DTOs;

public class TodoCreateDtoValidator : AbstractValidator<TodoCreateDto>
{
    public TodoCreateDtoValidator()
    {
        // Rule #1: Title cannot be empty
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Todo item title cannot be empty.");

        // Rule 2 : The title must not exceed 100 characters.
        RuleFor(x => x.Title)
            .MaximumLength(100)
            .WithMessage("Todo item title must be a maximum of 100 characters long.");
    }
}
