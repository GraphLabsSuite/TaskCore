using System.Diagnostics.Contracts;

namespace GraphLabs.Tasks.Core
{
    /// <summary> Поддержка создания глубокой копии </summary>
    [ContractClass(typeof(CloneableContracts))]
    public interface ICloneable
    {
        /// <summary> Создаёт глубокую копию данного объекта </summary>
        object Clone();
    }
}
