namespace GraphLabs.Common
{
    /// <summary> Класс, умеющий асинхронно выполнять операции, тем не менее требующие блокировки UI</summary>
    public interface IUiBlockerAsyncProcessor
    {
        /// <summary> Выполняется бокирующая UI операция </summary>
        bool IsBusy { get; }
    }
}