public sealed record RefreshToken(
    string Token,          
    string TokenHash,
    DateTime Expiracion
);