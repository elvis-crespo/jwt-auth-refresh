namespace JWT_AuthAndRefrest.Models.Custom
{
    public class AuthorizationResponse
    {
        public string Token { get; set; }
        public string RefrestToken { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}