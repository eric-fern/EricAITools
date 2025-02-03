Some considerations to keep in mind:

For the chat functionality, you'll want to think about:

How to handle long-running conversations
State management between the frontend and Azure Functions
Authentication and user session management if required


For the Azure Functions:

Consider using Durable Functions if you need to maintain conversation state
Think about how you'll handle timeouts and retries
Plan for appropriate security measures between your frontend and Functions

Cost Benefits:


Azure Functions would only run when a prompt is sent, rather than maintaining a persistent connection
Each function execution is discrete and billable only for the actual processing time
No need for complex state management infrastructure


Technical Feasibility:


HTTP triggered Azure Functions are perfect for this stateless approach
You can send the entire conversation history as part of each request
The function can process the context and new prompt, then return a single response
This matches the typical pattern of how most AI services work anyway


Simplifications:


No need for WebSockets or SignalR
No session management complexity
Simpler error handling and retry logic
Easier to test and debug

The main tradeoff would be:

Slightly higher bandwidth usage since you're sending the conversation history each time
Potentially slightly higher latency per request
Need to manage conversation history on the client side