namespace ETICARET.WebUI.Models
{
    public class AccountModel
    {
        public string Id { get; set; }=string.Empty;
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }

    }
}
