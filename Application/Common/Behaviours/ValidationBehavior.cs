using FluentValidation;
using MediatR;

namespace Application.Common.Behaviours;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    // Реализация метода Handle() интерфейса IPipelineBehavior.
    // Параметр request - это объект запроса, переданный через метод Mediator.Send.
    // Параметр next - это асинхронное продолжение для следующего действия в цепочке
    // вызовов нашего Behavior.
    public Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList();
        // выкидываем исключение, если есть хоть одна ошибка валидации
        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }
        return next();
    }
}