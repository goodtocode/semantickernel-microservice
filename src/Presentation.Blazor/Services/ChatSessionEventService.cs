using Goodtocode.SemanticKernel.Presentation.Blazor.Models;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services
{
    public class ChatSessionEventService
    {
        public event Action<ChatSessionModel>? SessionSelected;

        public void RaiseSessionSelected(ChatSessionModel session)
        {
            SessionSelected?.Invoke(session);
        }
    }
}
