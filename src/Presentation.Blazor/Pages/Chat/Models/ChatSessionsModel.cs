using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Models;

public class ChatSessionsModel : Collection<ChatSessionModel>
{
    private bool _isHandlingPropertyChanged;

    public event EventHandler<ChatSessionModel>? SessionIsActiveChanged;
    public ChatSessionModel? ActiveSession => this.FirstOrDefault(x => x.IsSelected);


    public void RefreshItem(ChatSessionModel item)
    {
        if (item.Id == Guid.Empty)
        {
            return;
        }
        var existingItem = this.FirstOrDefault(x => x.Id == item.Id);
        var existingIndex = IndexOf(existingItem ?? new ChatSessionModel());
        if (existingIndex >= 0)
        {
            SetItem(existingIndex, item);
            if (existingItem!.IsSelected)
            {
                SetActive(existingIndex);
            }
        }
        else
        {
            Add(item);
        }
    }

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

        if (e.PropertyName == nameof(ChatSessionModel.IsSelected) && sender is ChatSessionModel session)
        {
            _isHandlingPropertyChanged = true;
            try
            {
                if (session.IsSelected)
                {
                    foreach (var s in this)
                    {
                        if (!ReferenceEquals(s, session) && s.IsSelected)
                        {
                            s.IsSelected = false;
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
            Unsubscribe(session);
            session.IsSelected = true;
            Subscribe(session);
        }
    }

    public void SetActive(int index)
    {
        if (index >= 0 && index < Count)
        {
            SetActive(this[index]);
        }
    }

    public void ClearActive()
    {
        if (ActiveSession != null)
        {
            Unsubscribe(ActiveSession);
            ActiveSession.IsSelected = false;
        }
    }
}
