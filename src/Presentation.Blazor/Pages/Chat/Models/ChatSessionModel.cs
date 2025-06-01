using Goodtocode.SemanticKernel.Presentation.WebApi.Client;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Models;

public class ChatSessionModel : INotifyPropertyChanged
{
    public static List<ChatSessionModel> Create(ICollection<ChatSessionDto> chatSessions)
    {
        return [.. chatSessions.Select(dto => new ChatSessionModel
        {
            Id = dto.Id,
            Title = dto.Title,
            AuthorId = dto.AuthorId,
            Timestamp = dto.Timestamp,
            IsActive = false,
            Messages = [.. dto.Messages.Select(m => new ChatMessageModel
            {
                Id = m.Id,
                Content = m.Content,
                Role = m.Role,
                Timestamp = m.Timestamp
            })]
        })];
    }

    public static ChatSessionModel Create(ChatSessionDto chatSession)
    {
        return new ChatSessionModel
        {
            Id = chatSession.Id,
            Title = chatSession.Title,
            AuthorId = chatSession.AuthorId,
            Timestamp = chatSession.Timestamp,
            IsActive = false,
            Messages = [.. chatSession.Messages.Select(m => new ChatMessageModel
            {
                Id = m.Id,
                Content = m.Content,
                Role = m.Role,
                Timestamp = m.Timestamp
            })]
        };
    }

    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public Guid AuthorId { get; set; } = Guid.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public virtual ICollection<ChatMessageModel>? Messages { get; set; }

    private bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive != value)
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
