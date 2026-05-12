public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
}
