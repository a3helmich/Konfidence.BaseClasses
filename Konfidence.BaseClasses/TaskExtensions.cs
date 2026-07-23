using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Konfidence.Base;

public static class TaskExtensions
{
    [UsedImplicitly]
    public static Task<T> DefaultIfCanceled<T>(this Task<T> task, T defaultValue = default!)
    {
        return task.ContinueWith(t => t.IsCanceled ? defaultValue : t.Result);
    }

    [UsedImplicitly]
    public static Task DefaultIfCanceled(this Task task)
    {
        return task.ContinueWith(_ => Task.CompletedTask);
    }
}
