using System.Diagnostics.Contracts;
using GraphLabs.Graphs.Contracts;

namespace GraphLabs.Graphs
{
    /// <summary> Поддержка создания глубокой копии </summary>
    [ContractClass(typeof(CloneableContracts))]
    public interface ICloneable
    {
        /// <summary> Создаёт глубокую копию данного объекта </summary>
        object Clone();
    }
}
