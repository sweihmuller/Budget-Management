namespace Budget_Management.Services
{
    public interface IUserServices
    {
        int RetrieveUserId();
    }

    public class UserServices : IUserServices
    {
        public int RetrieveUserId()
        {
            return 1;
        }
    }
}
