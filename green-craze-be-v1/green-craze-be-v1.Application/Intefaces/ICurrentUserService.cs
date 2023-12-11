namespace green_craze_be_v1.Application.Intefaces
{
    public interface ICurrentUserService
    {
        public string UserId { get; }

        bool IsInRole(string role);
    }
}