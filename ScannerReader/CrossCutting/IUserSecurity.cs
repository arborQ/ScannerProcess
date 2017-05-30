namespace CrossCutting
{
    public interface IUserSecurity
    {
        void SetCurrentUser(string login);

        bool ValidateUser(string login, string password);

        bool ValidateBarCodeUser(string login);

        string CurrentUserLogin();
    }
}
