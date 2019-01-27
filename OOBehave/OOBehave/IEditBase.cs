using OOBehave.Portal;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OOBehave
{
    public interface IEditBase : IValidateBase, IEditMetaProperties, IPortalEditTarget
    {
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IReadOnlyList<string> ModifiedProperties { get; }

        bool IsChild { get; }

        /// <summary>
        /// Marks the object as deleted
        /// </summary>
        void Delete();

        Task Save();

        new IEditPropertyMeta this[string propertyName] { get; }

        EditPropertyMetaByName<bool> PropertyIsModified { get; }
    }



}
