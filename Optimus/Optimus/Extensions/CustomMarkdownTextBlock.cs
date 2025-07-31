using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.System;

namespace Optimus.Extensions
{
    public partial class CustomMarkdownTextBlock : MarkdownTextBlock
    {
        public CustomMarkdownTextBlock()
        {
            TextWrapping = TextWrapping.WrapWholeWords;

            ParagraphLineHeight = 24;
            QuotePadding = new Thickness(12);
            CodePadding = new Thickness(12);

            Background = new SolidColorBrush(Colors.Transparent);

            LinkClicked += CustomMarkdownTextBlock_LinkClicked;
        }

        private async void CustomMarkdownTextBlock_LinkClicked(object? sender, LinkClickedEventArgs e)
        {
            var uri = new Uri(e.Link);
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
