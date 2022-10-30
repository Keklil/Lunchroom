namespace Contracts
{
    public interface IMailIdleClient
    {
        Task RunAsync();
        void Exit();
        void Dispose();
    }
}
