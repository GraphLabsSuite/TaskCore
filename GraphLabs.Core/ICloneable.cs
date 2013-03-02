using System.Diagnostics.Contracts;

namespace GraphLabs.Core
{
    /// <summary> Поддержка создания глубокой копии </summary>
    [ContractClass(typeof(CloneableContracts))]
    public interface ICloneable
    {
        /// <summary> Создаёт глубокую копию данного объекта </summary>
        object Clone();
    }
}
