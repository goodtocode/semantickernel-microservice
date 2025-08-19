﻿using Goodtocode.SemanticKernel.Core.Application.Author;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetAuthorChatSessionsQueryValidator : Validator<GetAuthorChatSessionsQuery>
{
    public GetAuthorChatSessionsQueryValidator()
    {
        RuleFor(x => x.AuthorId).NotEmpty();

        RuleFor(v => v.StartDate).NotEmpty()
            .When(v => v.EndDate != null)
            .LessThanOrEqualTo(v => v.EndDate);

        RuleFor(v => v.EndDate)
            .NotEmpty()
            .When(v => v.StartDate != null)
            .GreaterThanOrEqualTo(v => v.StartDate);
    }
}