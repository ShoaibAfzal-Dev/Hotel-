using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using MVC.Models;

public class ChatHub : Hub
{
    private readonly HttpClient _httpClient;
    // Add your url here
    private string BaseURL = "";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChatHub(IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BaseURL);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task SendMessageToAdmin(string message)
    {
        
        var userId = Context.ConnectionId; 
        var user_id = _httpContextAccessor.HttpContext.Session.GetString("UserId");
        if (message != null)
        {
            var da = new ChatModel() {
                SenderID = user_id,
                message = message
            };
            await Clients.Group("Admin").SendAsync("ReceiveMessageFromUser", userId, message, user_id);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/SendToAdmin", da);

            if(response.IsSuccessStatusCode)
            {

            }
        }

       
    }

    public async Task SendMessageToUser(string userId,string inputData)
    {
        /*var adminId = Context.ConnectionId;  */
            await Clients.All.SendAsync("ReceiveMessageFromAdmin", userId, inputData);
    }

    public override async Task OnConnectedAsync()
    {
        var userRole = _httpContextAccessor.HttpContext.Session.GetString("UserRole");

        if (userRole == "admin")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
        }

        await base.OnConnectedAsync();
    }
}
