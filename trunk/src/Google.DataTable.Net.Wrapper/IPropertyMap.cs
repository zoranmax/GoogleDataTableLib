using System.Collections.Generic;

namespace Google.DataTable.Net.Wrapper
{
    public interface IPropertyMap
    {
        /// <summary>
        /// Returns a list of currently assigned properties to the Cell
        /// </summary>
        IEnumerable<Property> PropertyMap { get; }

        /// <summary>
        /// Adds a new property to the list of properties.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        Property AddProperty(Property p);

        /// <summary>
        /// Removes a property from the Property Map
        /// </summary>
        /// <param name="p"></param>
        void RemoveProperty(Property p);

        /// <summary>
        /// Removes a property from the Property Map by an index.
        /// </summary>
        /// <param name="index"></param>
        void RemoveProperty(int index);
    }
}