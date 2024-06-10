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
    public class PersonUpdateCommand : IRequest<CommandResponse<Person>>
    {
        public IPerson Item { get; set; }

        public PersonUpdateCommand() { }

        public PersonUpdateCommand(IPerson item)
        {
            Item = item;
        }
    }

    public class PersonUpdateHandler : IRequestHandler<PersonUpdateCommand, CommandResponse<Person>>
    {
        private readonly PersonUpdateValidator _validator;
        private readonly List<KeyValuePair<string, string>> _errors;
        private readonly IAssociateDbContext _dbContext;

        public PersonUpdateHandler(IAssociateDbContext context)
        {
            _dbContext = context;
            _validator = new PersonUpdateValidator();
            _errors = new List<KeyValuePair<string, string>>();
        }

        public async Task<CommandResponse<Person>> Handle(PersonUpdateCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResponse<Person>() { Errors = GetRequestErrors(request) };

            if (result.Errors.Count == 0)
            {
                try
                {
                    var aggregate = new AssociateAggregate(_dbContext);
                    await aggregate.PersonUpdateAsync(request.Item);
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

        private List<KeyValuePair<string, string>> GetRequestErrors(PersonUpdateCommand request)
        {
            var issues = _validator.Validate((Person)request.Item).Errors;

            foreach (var issue in issues)
                _errors.Add(new KeyValuePair<string, string>(issue.PropertyName, issue.ErrorMessage));
            return _errors;
        }

    }
}
