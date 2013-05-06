namespace Google.DataTable.Net.Wrapper.Extension
{
    /// <summary>
    /// Class that implements an extension for a System.Data.DataTable
    /// </summary>
    public static class SystemDataTableExtension
    {
        /// <summary>
        /// Converts a System.Data.DataTable into a  Google.DataTable.Net.Wrapper.DataTable
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable ToGoogleDataTable(this System.Data.DataTable source)
        {
            if (source == null)
                return null;

            return SystemDataTableConverter.Convert(source);
        }
    }
}
