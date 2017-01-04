namespace Common.Interfaces
{
    public interface IUserSecurity
    {
        void SetCurrentUser(string login);

        string CurrentUserLogin();
    }
}