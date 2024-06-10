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
    public class BusinessGetQuery : IRequest<QueryResponse<Business>>
    {
        public BusinessGetQuery(Guid businessKey)
        {
            BusinessKey = businessKey;
        }

        public Guid BusinessKey { get; set; }
    }

    public class BusinessGetHandler : IRequestHandler<BusinessGetQuery, QueryResponse<Business>>
    {

        private readonly BusinessGetValidator _validator;
        private readonly List<KeyValuePair<string, string>> _errors;
        private readonly ILogger<BusinessGetHandler> _logger;
        private readonly AssociateDbContext _dbContext;

        public BusinessGetHandler(AssociateDbContext dbContext)
        {

            _dbContext = dbContext;
            _validator = new BusinessGetValidator();
            _errors = new List<KeyValuePair<string, string>>();
        }

        public async Task<QueryResponse<Business>> Handle(BusinessGetQuery request, CancellationToken cancellationToken)
        {
            var response = new QueryResponse<Business>() { Errors = ValidateRequest(request) };

            if (!response.Errors.Any())
            {
                try
                {
                    response.Result = await _dbContext.Business.FindAsync(request.BusinessKey);
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

        private List<KeyValuePair<string, string>> ValidateRequest(BusinessGetQuery request)
        {
            var issues = _validator.Validate(request).Errors;

            foreach (var issue in issues)
                _errors.Add(new KeyValuePair<string, string>(issue.PropertyName, issue.ErrorMessage));

            return _errors;
        }
    }
}