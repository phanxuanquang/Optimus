using Microsoft.UI.Xaml.Controls;
using Optimus.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Optimus.Controls
{
    public sealed partial class AgentChatMessage : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ChatMessageViewModel _viewModel = new();
        public ChatMessageViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    OnPropertyChanged();
                }
            }
        }
        public AgentChatMessage()
        {
            InitializeComponent();
        }
    }
}
