namespace green_craze_be_v1.Application.Intefaces
{
    public interface IBackgroundJobService
    {
        Task CancelOrder(long orderId);
    }
}
