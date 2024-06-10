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
    public class BusinessSaveCommand : IRequest<CommandResponse<Business>>
    {
        public IBusiness Item { get; set; }

        public BusinessSaveCommand() { }

        public BusinessSaveCommand(IBusiness item)
        {
            Item = item;
        }
    }

    public class BusinessSaveHandler : IRequestHandler<BusinessSaveCommand, CommandResponse<Business>>
    {
        private readonly BusinessSaveValidator _validator;
        private readonly List<KeyValuePair<string, string>> _errors;
        private readonly IAssociateDbContext _dbContext;

        public BusinessSaveHandler(IAssociateDbContext context)
        {
            _dbContext = context;
            _validator = new BusinessSaveValidator();
            _errors = new List<KeyValuePair<string, string>>();
        }

        public async Task<CommandResponse<Business>> Handle(BusinessSaveCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResponse<Business>() { Errors = GetRequestErrors(request) };

            if (result.Errors.Count == 0)
            {
                try
                {
                    var aggregate = new AssociateAggregate(_dbContext);
                    await aggregate.BusinessSaveAsync(request.Item);
                    result.Result = (Business)request.Item;
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

        private List<KeyValuePair<string, string>> GetRequestErrors(BusinessSaveCommand request)
        {
            var issues = _validator.Validate((Business)request.Item).Errors;

            foreach (var issue in issues)
                _errors.Add(new KeyValuePair<string, string>(issue.PropertyName, issue.ErrorMessage));
            return _errors;
        }

    }
}
