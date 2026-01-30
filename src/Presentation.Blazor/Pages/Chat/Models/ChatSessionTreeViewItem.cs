using Microsoft.FluentUI.AspNetCore.Components;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Models
{
    public class ChatSessionTreeViewItem : ITreeViewItem
    {
        public ChatSessionModel Session { get; }

        public ChatSessionTreeViewItem(ChatSessionModel session)
        {
            Session = session;
        }

        public static ChatSessionTreeViewItem CreateFrom(ChatSessionModel session)
            => new(session);

        // ITreeViewItem implementation
        public string Id
        {
            get { return Session.Id.ToString(); }
            set { }
        }

        public string Text
        {
            get { return string.IsNullOrWhiteSpace(Session.Title) ? "Untitled Session" : Session.Title; }
            set { }
        }

        // Flat list, no children
        public IEnumerable<ITreeViewItem>? Children => null;

        public IEnumerable<ITreeViewItem>? Items
        {
            get => Children;
            set { }
        }

        public Icon? IconCollapsed { get; set; }
        public Icon? IconExpanded { get; set; }
        public bool Disabled { get; set; }
        public bool Expanded { get; set; }
        public Func<TreeViewItemExpandedEventArgs, Task>? OnExpandedAsync { get; set; }

        // Add other ITreeViewItem members as required by your Fluent UI version
    }
}