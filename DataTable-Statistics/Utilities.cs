using System;
using System.Activities;
using System.Data;

namespace UiPathTeam.DataTableStatistics.Activities
{
    public static class Utilities
    {
		#region Validations
		
        public static void validateDatatable(NativeActivityContext context, InArgument<DataTable> DT)
        {
            if (DT == null || DT.Get(context) == null)
            {
                throw new Exception("Datatable is a required parameter");
            }

            if (DT.Get(context).Rows.Count == 0)
            {
                throw new Exception("Datatable cannot be empty.  It must have at least 1 row");
            }
        }

        public static int validateColumnNameOrNumber(NativeActivityContext context, InArgument<DataTable> DT, InArgument<String> ColumnName, InArgument<Int32> ColumnNumber)
        {
            int idx = ColumnNumber.Get(context);
            if (ColumnName == null || ColumnName.Get(context) == null || ColumnName.Get(context).Length == 0)
            {
                if (idx < 0 || idx > DT.Get(context).Columns.Count)
                {
                    throw new Exception("Invalid Column Number.  Please specify a valid Column Index.");
                }
            }
            else
            {
                idx = DT.Get(context).Columns.IndexOf(ColumnName.Get(context));
                if (idx < 0)
                {
                    throw new Exception("Column Name not found.  Please specify a valid Column Name.");
                }
            }

            return idx;
        }
		
		#endregion
		
    }
}
