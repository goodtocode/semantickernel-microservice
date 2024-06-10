using GoodToCode.Shared.Cqrs;
using GoodToCode.Shared.Validation;
using Microservice.Persistence;
using Microservice.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class PersonCreateCommand : IRequest<CommandResponse<Person>>
    {
        public IPerson Item { get; set; }

        public PersonCreateCommand() { }

        public PersonCreateCommand(IPerson item)
        {
            Item = item;
        }
    }

    public class PersonCreateHandler : IRequestHandler<PersonCreateCommand, CommandResponse<Person>>
    {
        private readonly PersonCreateValidator _validator;
        private readonly List<KeyValuePair<string, string>> _errors;
        private readonly IAssociateDbContext _dbContext;

        public PersonCreateHandler(IAssociateDbContext context)
        {
            _dbContext = context;
            _validator = new PersonCreateValidator();
            _errors = new List<KeyValuePair<string, string>>();
        }

        public async Task<CommandResponse<Person>> Handle(PersonCreateCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResponse<Person>() { Errors = GetRequestErrors(request) };

            if (result.Errors.Count == 0)
            {
                try
                {
                    var aggregate = new AssociateAggregate(_dbContext);
                    await aggregate.PersonCreateAsync(request.Item);
                    result.Result = (Person)request.Item;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    result.ErrorInfo = new ErrorInfo()
                    {
                        UserErrorMessage = "An internal error has occured",
                        HasException = true
                    };
                }
            }
            return result;
        }

        private List<KeyValuePair<string, string>> GetRequestErrors(PersonCreateCommand request)
        {
            var issues = _validator.Validate((Person)request.Item).Errors;

            foreach (var issue in issues)
                _errors.Add(new KeyValuePair<string, string>(issue.PropertyName, issue.ErrorMessage));
            return _errors;
        }

    }
}
