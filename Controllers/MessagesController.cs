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
            else if (string.Equals(activity.Text, "You can do better...", StringComparison.OrdinalIgnoreCase))
            {
                ServerMock.ResetServer();
                replyToConversation = activity.CreateReply("I'll do my best to be better next time, Thanks!");
            }
            else if (string.Equals(activity.Text, "Great Tip!", StringComparison.OrdinalIgnoreCase))
            {
                ServerMock.ResetServer();
                replyToConversation = activity.CreateReply("Good to know :) Thanks!");
            }
            else
            {
                replyToConversation = activity.CreateReply();
                Attachment attachment = CreateHelpCard(activity.Text);
                replyToConversation.Attachments.Add(attachment);
            }
            return replyToConversation;
        }


        private Attachment CreateTipsFeedbackCard(string message)
        {

            IServer server = ServerMock.GetInstance();
            var myContact = server.GetContactByName("shir esh");
            var meeting = server.FindNextMeeting(myContact);
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
            string tip = " <font size=\"3\" color=\"#6699ff\"> <b>   " + server.GetGivenTips(meeting, myContact).FirstOrDefault().Content + "</li> </b> </font> ";
            string text = "So, What do you think about this tip? :)" + NewLine() + NewLine() + tip + NewLine();

            var card = new HeroCard //ThumbnailCard
            {
                Title = title,
                Subtitle = subTitle,
                Text = text,


                Buttons = new List<CardAction>()
                {
                     new CardAction("imBack", "Great Tip!", value: "Great Tip!"),
                     new CardAction("imBack", "You can do better...", value: "You can do better..."),
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

            var linkedinLogo = @"data:image/png;base64,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";

            var card = new ThumbnailCard// HeroCard //ThumbnailCard
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
                    new CardAction("openUrl","LinkedIn",value:otherContact.LinkedinPath, image: linkedinLogo)
                }
            };
            
            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = ThumbnailCard.ContentType,
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

            string concatenedTips = ConcatAllTips(tipContents, "");

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

        private string ConcatAllTips(List<string> tips, string title = "Here are some tips to help you prepare:")
        {
            title = title.Equals("Here are some tips to help you prepare:") ? title + " <small> " + NewLine() + " </small> " : title; 
            int i = 1;
            var ans = new StringBuilder();
            foreach (string tip in tips)
            {
                ans = ans.Append(" <font size=\"3\" color=\"#6699ff\"> <b> <li>").Append(tip).Append("</li> </b> </font> ");
                i++;
            }
            return title + " <ul> " + ans.ToString() + " </ul> ";
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