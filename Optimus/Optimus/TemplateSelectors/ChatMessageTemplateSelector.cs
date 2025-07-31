using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Optimus.ViewModels;

namespace Optimus.TemplateSelectors
{
    public partial class ChatMessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserTemplate { get; set; } = null!;

        public DataTemplate AgentTemplate { get; set; } = null!;

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var selectedObject = item as ChatMessageViewModel;

            if (selectedObject?.Role == AuthorRole.User)
            {
                return UserTemplate;
            }

            return AgentTemplate;
        }
    }
}
