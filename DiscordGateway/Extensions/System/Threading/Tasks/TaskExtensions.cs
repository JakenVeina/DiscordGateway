namespace System.Threading.Tasks
{
    internal static class TaskExtensions
    {
        public static async Task WithSilentCancellation(this Task task)
        {
            try
            {
                await task;
            }
            catch (OperationCanceledException) { }
        }
    }
}
