using FluentValidation;

namespace ChurchMS.Application.Features.Evangelism.Commands.RecordFollowUp;

public class RecordFollowUpValidator : AbstractValidator<RecordFollowUpCommand>
{
    public RecordFollowUpValidator()
    {
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.FollowUpDate).NotEmpty();
        RuleFor(x => x.Notes).MaximumLength(2000).When(x => x.Notes is not null);
    }
}
