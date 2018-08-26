using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;
using SimpleEchoBot.Dialogs;
using LogicLayer;
using System.Collections.Generic;
using System;
using System.Text;
using EntitiesLayer;
using System.Linq;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private bool isTeams = true;
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));

            // check if activity is of type message
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {

                Activity replyToConversation = CreateReply(activity);

                await connector.Conversations.SendToConversationAsync(replyToConversation);
                //await Conversation.SendAsync(activity, () => new AI_Dialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private Activity CreateReply(Activity activity)
        {
            Activity replyToConversation;
            if (string.Equals(activity.Text, UserMessage.FirstMessage, StringComparison.OrdinalIgnoreCase)) {
                replyToConversation = activity.CreateReply("Hi there :)");
                Attachment attachment = CreateFirstCard();
                replyToConversation.Attachments.Add(attachment);
            }
            else if (string.Equals(activity.Text, UserMessage.SecondMessage, StringComparison.OrdinalIgnoreCase))
            {
                replyToConversation = activity.CreateReply("ok...");
                Attachment attachment = CreateSecondCard();
                replyToConversation.Attachments.Add(attachment);
            }
            else if (string.Equals(activity.Text, UserMessage.ThirdMessage, StringComparison.OrdinalIgnoreCase))
            {
                replyToConversation = activity.CreateReply("Hi again!");
                Attachment attachment = CreateThirdCard();
                replyToConversation.Attachments.Add(attachment);
            }
            else if (string.Equals(activity.Text, UserMessage.FourthMessage, StringComparison.OrdinalIgnoreCase))
            {
                replyToConversation = activity.CreateReply("I see the meeting is ended");
                Attachment attachment = CreateFourtCard();
                replyToConversation.Attachments.Add(attachment);
            }
            else
            {
                replyToConversation = activity.CreateReply(UserMessage.CrateHelpMessage());
            }
            return replyToConversation;
        }

        private Attachment CreateFirstCard()
        {
            var server = ServerMock.GetInstance();
            var myContact = server.GetContactByName("shir esh");

            Meeting meeting = server.FindNextMeeting(myContact);
            Contact otherContact = meeting.Attendees.Where(c => c != myContact).FirstOrDefault();
            var tips = server.GetTipsAboutPersonForMeeting(meeting, myContact, false);
            List<string> tipContents = tips.Select(x => x.Content).ToList();

            string concatenedTips = ConcatAllTips(tipContents);

            var card = new HeroCard //ThumbnailCard
            {
                Title = $"Your next meeting is with {otherContact.Name}",
                Subtitle = $"Meeting title: {meeting.Title}",
                Text = concatenedTips,
                Images = new List<CardImage>()
                {
                    new CardImage(otherContact.ImagePath)
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction("openUrl", "Linkedin", value: otherContact.ImagePath)
                }
            };
            
            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = HeroCard.ContentType,
                Content = card
            };
            return attachment;
        }

        // TODO: change this card!
        private Attachment CreateSecondCard()
        {
            var server = ServerMock.GetInstance();
            var myContact = server.GetContactByName("shir esh");

            Meeting meeting = server.FindNextMeeting(myContact);
            Contact otherContact = meeting.Attendees.Where(c => c != myContact).FirstOrDefault();
            var tips = server.GetTipsAboutPersonForMeeting(meeting, myContact, false);
            List<string> tipContents = tips.Select(x => x.Content).ToList();

            string concatenedTips = ConcatAllTips(tipContents);

            var card = new HeroCard //ThumbnailCard
            {
                Title = $"Your next meeting is with {otherContact.Name}",
                Subtitle = $"Meeting title: {meeting.Title}",
                Text = concatenedTips,
                Images = new List<CardImage>()
                {
                    new CardImage(otherContact.ImagePath)
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction("openUrl", "Linkedin", value: otherContact.ImagePath)
                }
            };

            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = HeroCard.ContentType,
                Content = card
            };
            return attachment;
        }

        // TODO: change this card!
        private Attachment CreateThirdCard()
        {
            var server = ServerMock.GetInstance();
            var myContact = server.GetContactByName("shir esh");

            Meeting meeting = server.FindNextMeeting(myContact);
            Contact otherContact = meeting.Attendees.Where(c => c != myContact).FirstOrDefault();
            var tips = server.GetTipsAboutPersonForMeeting(meeting, myContact, false);
            List<string> tipContents = tips.Select(x => x.Content).ToList();

            string concatenedTips = ConcatAllTips(tipContents);

            var card = new HeroCard //ThumbnailCard
            {
                Title = $"Your next meeting is with {otherContact.Name}",
                Subtitle = $"Meeting title: {meeting.Title}",
                Text = concatenedTips,
                Images = new List<CardImage>()
                {
                    new CardImage(otherContact.ImagePath)
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction("openUrl", "Linkedin", value: otherContact.ImagePath)
                }
            };

            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = HeroCard.ContentType,
                Content = card
            };
            return attachment;
        }

        // TODO: change this card!
        private Attachment CreateFourtCard()
        {
            var server = ServerMock.GetInstance();
            var myContact = server.GetContactByName("shir esh");

            Meeting meeting = server.FindNextMeeting(myContact);
            Contact otherContact = meeting.Attendees.Where(c => c != myContact).FirstOrDefault();
            var tips = server.GetTipsAboutPersonForMeeting(meeting, myContact, false);
            List<string> tipContents = tips.Select(x => x.Content).ToList();

            string concatenedTips = ConcatAllTips(tipContents);

            var card = new HeroCard //ThumbnailCard
            {
                Title = $"Your next meeting is with {otherContact.Name}",
                Subtitle = $"Meeting title: {meeting.Title}",
                Text = concatenedTips,
                Images = new List<CardImage>()
                {
                    new CardImage(otherContact.ImagePath)
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction("openUrl", "Linkedin", value: otherContact.ImagePath)
                }
            };

            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = HeroCard.ContentType,
                Content = card
            };
            return attachment;
        }

        private string NewLine()
        {
            if (isTeams)
            {
                return " <br> ";
            }

            return " \n\r ";
        }

        private string ConcatAllTips(List<string> tips)
        {
            int i = 1;
            var ans = new StringBuilder("Meeting Tips:");
            foreach (string tip in tips)
            {
                ans = ans.Append(NewLine()).Append($"   {i}) ").Append(tip);
                i++;
            }
            return ans.ToString();
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }

    public static class UserMessage
    {
        public static string FirstMessage = "my next meeting";
        public static string SecondMessage = "next tips";
        public static string ThirdMessage = "next immediate meeting";
        public static string FourthMessage = "rate tips";

        public static string CrateHelpMessage()
        {
            string ans = "I'm sorry, I just understand the following requests:";
            ans += "\n - " + FirstMessage;
            ans += "\n - " + SecondMessage;
            ans += "\n - " + ThirdMessage;
            ans += "\n - " + FourthMessage;
            return ans;

        }
    }
}