using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            var reply = activity.CreateReply();
            reply.Text = "hi there";
            Attachment attach = new Attachment();
            attach.ContentType = "application/vnd.microsoft.card.hero";
            attach.Content = "{\r\n                \"title\": \"Your next meeting is with Shir Esh\",\r\n                \"subtitle\": \"Meeting tips\",\r\n                \"text\": \"    1) use small-talk.\n    2) use numbers.\",\r\n                \"images\": [\r\n                    {\r\n                        \"url\": \"https://static.wixstatic.com/media/db9144_6df95743434c4614bee7dc5bcb1508c7~mv2_d_1236_1592_s_2.png/v1/crop/x_0,y_0,w_1236,h_1235/fill/w_285,h_285,al_c,usm_0.66_1.00_0.01/db9144_6df95743434c4614bee7dc5bcb1508c7~mv2_d_1236_1592_s_2.png\",\r\n                        \"alt\": \"Shir Esh\",\r\n                    }\r\n                ],\r\n                \"buttons\": [\r\n                    {\r\n                        \"type\": \"openUrl\",\r\n                        \"title\": \"Linkedin\",\r\n                        \"image\": \"https://yt3.ggpht.com/a-/ACSszfGDvjYK2vL_d3Bglghs2VQhTwbPrTGWxBaNDQ=s900-mo-c-c0xffffffff-rj-k-no\",\r\n                        \"value\": \"https://www.linkedin.com/in/shir-esh-aa5981165/\"\r\n                    }\r\n                ]\r\n            }";
            reply.Attachments.Add(attach);
            var connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
            
            
           
            // check if activity is of type message
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                //await Conversation.SendAsync(activity, () => new ContactDialog());
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
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
}