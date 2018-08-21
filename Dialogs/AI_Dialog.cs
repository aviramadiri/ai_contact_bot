using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;


namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class AI_Dialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text.Contains("next meeting"))
            {
                await context.PostAsync($"1) here we need to send you a card with your next meeting");
            }
            else if (message.Text.Contains("meeting purpose"))
            {
                await context.PostAsync($"2) here we need to send you a card with more tips - depends on the purpose");
            }
            else if (message.Text.Contains("next immediate meeting"))
            {
                await context.PostAsync($"3) here we need to send you a card with more immediate tips - few minutes before the meeting");
            }
            else if (message.Text.Contains("rate tips"))
            {
                await context.PostAsync($"4) here we need to send you a card with the tips that been given - and ask for rating them");
            }
            else
            {
                await context.PostAsync($"sorry, the known commands are: \n" +
                    $"1) next meeting \n" +
                    $"2) meeting purpose \n" +
                    $"3) next immediate meeting \n" +
                    $"4) rate tips");
                context.Wait(MessageReceivedAsync);
            }
        }
    }
}