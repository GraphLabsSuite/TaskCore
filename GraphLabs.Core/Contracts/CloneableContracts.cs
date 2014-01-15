using System.Diagnostics.Contracts;

namespace GraphLabs.Graphs.Contracts
{
    /// <summary> Класс контрактов для ICloneable </summary>
    [ContractClassFor(typeof(ICloneable))]
    public abstract class CloneableContracts : ICloneable
    {
        #region Implementation of ICloneable

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        public object Clone()
        {
            Contract.Ensures(Contract.Result<object>() != null);

            return default(object);
        }

        #endregion
    }
}
