using System.Threading.Tasks;

namespace Konfidence.Tasks;

public static class TaskExtensions
{
    public static Task<T> DefaultIfCanceled<T>(this Task<T> task, T defaultValue = default!)
    {
        return task.ContinueWith(t => t.IsCanceled ? defaultValue : t.Result);
    }

    public static Task DefaultIfCanceled(this Task task)
    {
        return task.ContinueWith(_ => Task.CompletedTask);
    }
}