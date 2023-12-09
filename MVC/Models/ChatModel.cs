namespace MVC.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string SenderID { get; set; }
        public string? ReceiverID { get; set;}
        public string message { get; set;}
    }
}
