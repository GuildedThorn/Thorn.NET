using System.Diagnostics;
using ThornBot.Logger;

namespace ThornBot.Events; 

public class EventHandler {

    public static Task InitEvents() {
        Debug.Assert(ThornBot.Client != null, "ThornBot.Client != null");
        
        // Handle any logger types
        ThornBot.Client.Log += LoggerHandler.InitLogger;

        return Task.CompletedTask;
    }
    
}