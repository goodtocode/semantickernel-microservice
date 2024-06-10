using GoodToCode.Shared.Cqrs;
using Microservice.Persistence;
using Microservice.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class PersonGetQuery : IRequest<QueryResponse<Person>>
    {
        public PersonGetQuery(Guid personKey)
        {
            PersonKey = personKey;
        }

        public Guid PersonKey { get; set; }
    }

    public class PersonGetHandler : IRequestHandler<PersonGetQuery, QueryResponse<Person>>
    {

        private readonly PersonGetValidator _validator;
        private readonly List<KeyValuePair<string, string>> _errors;
        private readonly ILogger<PersonGetHandler> _logger;
        private readonly AssociateDbContext _dbContext;

        public PersonGetHandler(AssociateDbContext dbContext)
        {

            _dbContext = dbContext;
            _validator = new PersonGetValidator();
            _errors = new List<KeyValuePair<string, string>>();
        }

        public async Task<QueryResponse<Person>> Handle(PersonGetQuery request, CancellationToken cancellationToken)
        {
            var response = new QueryResponse<Person>() { Errors = ValidateRequest(request) };

            if (!response.Errors.Any())
            {
                try
                {
                    response.Result = await _dbContext.Person.FindAsync(request.PersonKey);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e.ToString());
                    response.ErrorInfo.UserErrorMessage = "An unknown error has occurred.";
                    response.ErrorInfo.HasException = true;
                }
            }

            return response;
        }

        private List<KeyValuePair<string, string>> ValidateRequest(PersonGetQuery request)
        {
            var issues = _validator.Validate(request).Errors;

            foreach (var issue in issues)
                _errors.Add(new KeyValuePair<string, string>(issue.PropertyName, issue.ErrorMessage));

            return _errors;
        }
    }
}