using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Models;

public class ChatSessionsModel : Collection<ChatSessionModel>
{
    public event EventHandler<ChatSessionModel>? SessionIsActiveChanged;

    private bool _isHandlingPropertyChanged;

    protected override void InsertItem(int index, ChatSessionModel item)
    {
        base.InsertItem(index, item);
        Subscribe(item);
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        Unsubscribe(item);
        base.RemoveItem(index);
    }

    protected override void ClearItems()
    {
        foreach (var item in this)
        {
            Unsubscribe(item);
        }
        base.ClearItems();
    }

    protected override void SetItem(int index, ChatSessionModel item)
    {
        Unsubscribe(this[index]);
        base.SetItem(index, item);
        Subscribe(item);
    }

    public void AddRange(IEnumerable<ChatSessionModel> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    private void Subscribe(ChatSessionModel item)
    {
        item.PropertyChanged += OnItemPropertyChanged;
    }

    private void Unsubscribe(ChatSessionModel item)
    {
        item.PropertyChanged -= OnItemPropertyChanged;
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_isHandlingPropertyChanged) return;

        if (e.PropertyName == nameof(ChatSessionModel.IsActive) && sender is ChatSessionModel session)
        {
            _isHandlingPropertyChanged = true;
            try
            {
                if (session.IsActive)
                {
                    foreach (var s in this)
                    {
                        if (!ReferenceEquals(s, session) && s.IsActive)
                        {
                            s.IsActive = false;
                        }
                    }
                }
                SessionIsActiveChanged?.Invoke(this, session);
            }
            finally
            {
                _isHandlingPropertyChanged = false;
            }
        }
    }

    public void SetActive(ChatSessionModel session)
    {
        if (!ReferenceEquals(session, ActiveSession))
        {
            session.IsActive = true;
        }
    }

    public void SetActive(int index)
    {
        if (index > 0 && index < Count)
        {
            SetActive(this[index]);
        }        
    }

    public ChatSessionModel? ActiveSession => this.FirstOrDefault(x => x.IsActive);
}
