using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Data;
using Windows.System;

namespace Optimus.ViewModels
{
    public class ChatMessageViewModel
    {
        public AuthorRole Role { get; set; }
        public string? Message { get; set; } = null;
        public object? Data { get; set; } = null;

        public static ChatMessageViewModel CreateUserMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            return new ChatMessageViewModel
            {
                Message = message.Trim(),
                Role = AuthorRole.User,
            };
        }

        public static ChatMessageViewModel CreateAssistantMessage(string? message, DataTable? data = null)
        {
            return new ChatMessageViewModel
            {
                Message = message?.Trim(),
                Role = AuthorRole.Assistant,
            };
        }

        public async void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            var uri = new Uri(e.Link);
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
