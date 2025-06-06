using System.ComponentModel;
using Goodtocode.SemanticKernel.Presentation.WebApi.Client;

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
            IsSelected = false,
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
            IsSelected = false,
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

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
