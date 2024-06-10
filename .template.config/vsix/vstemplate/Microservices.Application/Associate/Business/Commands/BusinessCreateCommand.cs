using GoodToCode.Shared.Cqrs;
using GoodToCode.Shared.Validation;
using MediatR;
using Microservice.Domain;
using Microservice.Persistence;

namespace $safeprojectname$
{
    public class BusinessCreateCommand : IRequest<CommandResponse<Business>>
    {
        public IBusiness Item { get; set; }

        public BusinessCreateCommand() { }

        public BusinessCreateCommand(IBusiness item)
        {
            Item = item;
        }
    }

    public class BusinessCreateHandler : IRequestHandler<BusinessCreateCommand, CommandResponse<Business>>
    {
        private readonly BusinessCreateValidator _validator;
        private readonly List<KeyValuePair<string, string>> _errors;
        private readonly IAssociateDbContext _dbContext;

        public BusinessCreateHandler(IAssociateDbContext context)
        {
            _dbContext = context;
            _validator = new BusinessCreateValidator();
            _errors = new List<KeyValuePair<string, string>>();
        }

        public async Task<CommandResponse<Business>> Handle(BusinessCreateCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResponse<Business>() { Errors = GetRequestErrors(request) };

            if (result.Errors.Count == 0)
            {
                try
                {
                    var aggregate = new AssociateAggregate(_dbContext);
                    await aggregate.BusinessCreateAsync(request.Item);
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

        private List<KeyValuePair<string, string>> GetRequestErrors(BusinessCreateCommand request)
        {
            var issues = _validator.Validate((Business)request.Item).Errors;

            foreach (var issue in issues)
                _errors.Add(new KeyValuePair<string, string>(issue.PropertyName, issue.ErrorMessage));
            return _errors;
        }

    }
}
