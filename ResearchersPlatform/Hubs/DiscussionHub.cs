using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using ResearchersPlatform_BAL.Contracts;

namespace ResearchersPlatform.Hubs
{
    [EnableCors("_myAllowSpecificOrigins")]
    public class DiscussionHub : Hub<IChatClient>
    {

    }
}
