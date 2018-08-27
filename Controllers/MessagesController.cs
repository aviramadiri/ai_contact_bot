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
            if (string.Equals(activity.Text, UserMessage.FirstMessage, StringComparison.OrdinalIgnoreCase))
            {
                replyToConversation = activity.CreateReply("Hi!");
                replyToConversation.Attachments.Add(CreateGeneralMeetingCard(false));
                replyToConversation.Attachments.Add(CreatePurposesCard());
            }
            else if (IsPurpose(activity.Text))
            {
                replyToConversation = activity.CreateReply();
                Attachment attachment = CreateTipsForPurposeCard(activity.Text);
                replyToConversation.Attachments.Add(attachment);
            }
            else if (string.Equals(activity.Text, UserMessage.ThirdMessage, StringComparison.OrdinalIgnoreCase))
            {
                replyToConversation = activity.CreateReply("Hi again!");
                Attachment attachment = CreateGeneralMeetingCard(true);
                replyToConversation.Attachments.Add(attachment);
            }
            else if (string.Equals(activity.Text, UserMessage.FourthMessage, StringComparison.OrdinalIgnoreCase))
            {
                replyToConversation = activity.CreateReply();
                Attachment attachment = CreateMeetingFeedbackCard();
                replyToConversation.Attachments.Add(attachment);
            }
            else if (IsSatisfation(activity.Text))
            {
                replyToConversation = activity.CreateReply();
                Attachment attachment = CreateTipsFeedbackCard(activity.Text);
                replyToConversation.Attachments.Add(attachment);
            }
            else
            {
                replyToConversation = activity.CreateReply();
                Attachment attachment = CreateHelpCard(activity.Text);
                replyToConversation.Attachments.Add(attachment);
            }
            return replyToConversation;
        }

        // todo: build this card
        private Attachment CreateTipsFeedbackCard(string message)
        {
            string title;
            string subTitle;
            if (message == MeetingSatisfaction.VeryGood || message == MeetingSatisfaction.Good)
            {
                title = "Good to hear!";
                subTitle = "Hope my tips were helpful (:";
            }
            else
            {
                title = "it can't be that bad...";
                subTitle = "Hope that at least my tips were helpful";
            }
            string text = "Please build this card: tips rating";

            var card = new HeroCard //ThumbnailCard
            {
                Title = title,
                Subtitle = subTitle,
                Text = text
        };

            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = HeroCard.ContentType,
                Content = card
            };
            return attachment;
        }

        private Attachment CreateGeneralMeetingCard(bool isSupposedToStart)
        {
            var server = ServerMock.GetInstance();
            var myContact = server.GetContactByName("shir esh");

            Meeting meeting = server.FindNextMeeting(myContact);
            Contact otherContact = meeting.Attendees.Where(c => c != myContact).FirstOrDefault();
            var tips = server.GetTipsAboutPersonForMeeting(meeting, myContact, isSupposedToStart);
            List<string> tipContents = tips.Select(x => x.Content).ToList();

            string title = isSupposedToStart ? 
                $"Your meeting with {otherContact.Name} is about to start..." : 
                $"Your next meeting is with {otherContact.Name}";

            string concatenedTips = ConcatAllTips(tipContents);

            var card = new HeroCard //ThumbnailCard
            {
                Title = title,
                Subtitle = $"Meeting title: {meeting.Title}",
                Text = concatenedTips,
                Images = new List<CardImage>()
                {
                    new CardImage(otherContact.ImagePath)
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction("openUrl", "Linkedin")
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

        private Attachment CreatePurposesCard()
        {
            // TODO: replace with MeetingPurpose
            var purposes = new List<CardAction>();
            var listOfPurposes = MeetingPurpose.GetPurposes();
            foreach (var p in listOfPurposes)
            {
                purposes.Add(new CardAction("imBack", p, value:p));
            }

            var card = new HeroCard //ThumbnailCard
            {
                Title = $"What is the purpose of the meeting?",
                Buttons = purposes
            };

            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = HeroCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public bool IsPurpose(string message)
        {
            return MeetingPurpose.GetPurposes().Contains(message);
        }

        // TODO: change this card!
        private Attachment CreateTipsForPurposeCard(string purpose)
        {
            // TODO: save the purpose
            var server = ServerMock.GetInstance();
            var myContact = server.GetContactByName("shir esh");

            Meeting meeting = server.FindNextMeeting(myContact);
            Contact otherContact = meeting.Attendees.Where(c => c != myContact).FirstOrDefault();
            var tips = server.GetTipsAboutPersonForMeeting(meeting, myContact, false);
            List<string> tipContents = tips.Select(x => x.Content).ToList();

            string concatenedTips = ConcatAllTips(tipContents, NewLine());

            var card = new HeroCard //ThumbnailCard
            {
                Subtitle = $"Great! For this kind of meeting try to follow these tips:",
                Text = concatenedTips
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
        private Attachment CreateMeetingFeedbackCard()
        {
            // TODO: replace with MeetingPurpose
            var satisfations = new List<CardAction>();
            var satisfationsAsList = MeetingSatisfaction.GetMeetingSatisfactions();
            foreach (var s in satisfationsAsList)
            {
                satisfations.Add(new CardAction("imBack", s, value:s));
            }

            var card = new HeroCard //ThumbnailCard
            {
                Title = $"Looks like the meeting is ended (:",
                Subtitle = "So how was the meeting?",
                Buttons = satisfations
            };

            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = HeroCard.ContentType,
                Content = card
            };
            return attachment;
        }

        public bool IsSatisfation(string message)
        {
            return MeetingSatisfaction.GetMeetingSatisfactions().Contains(message);
        }

        private string NewLine()
        {
            if (isTeams)
            {
                return " <br> ";
            }

            return " \n\r ";
        }

        private string ConcatAllTips(List<string> tips, string title = "Meeting Tips:")
        {
            int i = 1;
            var ans = new StringBuilder(title);
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

        private Attachment CreateHelpCard(string text)
        {
            // TODO: replace with MeetingPurpose
            var purposes = new List<CardAction>();
            var listOfPurposes = MeetingPurpose.GetPurposes();
            foreach (var p in listOfPurposes)
            {
                purposes.Add(new CardAction("imBack", p, value: p));
            }

            var card = new HeroCard //ThumbnailCard
            {
                Title = $"Hi there! (:",
                Subtitle = $" I'm Yoda and I'm here to help ",
                Text = "What would you like to know?",
                Buttons = new List<CardAction>()
                {
                    new CardAction("imBack", UserMessage.FirstMessage, value: UserMessage.FirstMessage),
                    new CardAction("imBack", UserMessage.ThirdMessage, value: UserMessage.ThirdMessage),
                    new CardAction("imBack", UserMessage.FourthMessage, value: UserMessage.FourthMessage)
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
    }

    public static class UserMessage
    {
        public static string FirstMessage = "What is my next meeting?";
        public static string ThirdMessage = "Is there a meeting that about to start?";
        public static string FourthMessage = "metting is ended";

        public static string CrateHelpMessage()
        {
            string ans = "I'm sorry, for now I just understand the following requests:";
            ans += "\n - " + FirstMessage;
            ans += "\n - " + ThirdMessage;
            ans += "\n - " + FourthMessage;
            return ans;

        }
    }
}