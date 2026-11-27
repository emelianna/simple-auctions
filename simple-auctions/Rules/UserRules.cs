using System.Net.Mail;

namespace SimpleAuctions.Rules;

public static class UserRules
{
    public static string? Validate(string email, string? password) 
    {
        if (email.Length > 254 || !MailAddress.TryCreate(email, out var address) || address.Address != email
            || string.IsNullOrWhiteSpace(password) || password.Length > 128)
        
            return "A valid email and password (1–128 characters) are required.";
        return null;
        
    }
}