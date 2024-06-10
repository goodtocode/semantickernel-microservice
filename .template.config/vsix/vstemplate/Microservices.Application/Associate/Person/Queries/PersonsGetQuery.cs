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
    public class PersonsGetQuery : IRequest<QueryResponse<List<Person>>>
    {
        public Func<Person, bool> QueryPredicate { get; } = x => x.RowKey != Guid.Empty;


        public PersonsGetQuery()
        {
        }

        public PersonsGetQuery(Func<Person, bool> predicateExpression)
        {
            QueryPredicate = predicateExpression;
        }
    }

    public class PersonsGetHandler : IRequestHandler<PersonsGetQuery, QueryResponse<List<Person>>>
    {

        private readonly PersonsGetValidator _validator;
        private readonly List<KeyValuePair<string, string>> _errors;
        private readonly ILogger<PersonsGetHandler> _logger;
        private readonly AssociateDbContext _dbContext;

        public PersonsGetHandler(AssociateDbContext dbContext)
        {

            _dbContext = dbContext;
            _validator = new PersonsGetValidator();
            _errors = new List<KeyValuePair<string, string>>();
        }

        public async Task<QueryResponse<List<Person>>> Handle(PersonsGetQuery request, CancellationToken cancellationToken)
        {
            var response = new QueryResponse<List<Person>>() { Errors = ValidateRequest(request) };

            if (!response.Errors.Any())
            {
                try
                {
                    response.Result = _dbContext.Person.Where(request.QueryPredicate).ToList();

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

        private List<KeyValuePair<string, string>> ValidateRequest(PersonsGetQuery request)
        {
            var issues = _validator.Validate(request).Errors;

            foreach (var issue in issues)
                _errors.Add(new KeyValuePair<string, string>(issue.PropertyName, issue.ErrorMessage));

            return _errors;
        }
    }
}