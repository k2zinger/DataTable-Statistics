using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace UiPathTeam.DataTableStatistics.Activities
{
    [DisplayName("Get Duplicates"), Description("Retrieves the count of duplicates and a list of the duplicated values within the user-specified column.")]
    public class GetDuplicates : NativeActivity
    {

        #region Properties

        [Category("Input"), Description("Input Datatable")]
        [RequiredArgument]
        public InArgument<DataTable> DT { get; set; }

        [Category("Input"), Description("Target Column Name to process")]
        [RequiredArgument]
        [OverloadGroup("ColumnName")]
        public InArgument<String> ColumnName { get; set; }

        [Category("Input"), Description("Target Colummn Index to process, starting with 0")]
        [RequiredArgument]
        [OverloadGroup("ColumnNumber")]
        public InArgument<Int32> ColumnNumber { get; set; }

        [Category("Input"), Description("Set True to output debugging information to the console.  Default: False")]
        public InArgument<Boolean> Verbose { get; set; } = false;

        [Category("Output"), Description("The count of all the duplicates found")]
        public OutArgument<Int32> Count { get; set; }

        [Category("Output"), Description("The ouptut list of duplicates")]
        public OutArgument<List<String>> Duplicates { get; set; }

        #endregion

        #region CodeActivity

        protected override void Execute(NativeActivityContext context)
        {
            Utilities.validateDatatable(context, DT);
            int idx = Utilities.validateColumnNameOrNumber(context, DT, ColumnName, ColumnNumber);

            Duplicates.Set(context, DT.Get(context).AsDataView().ToTable(false, new string[] { DT.Get(context).Columns[idx].ColumnName }).AsEnumerable().GroupBy(a => a[0]).Where(b => b.Count() > 1).Select(h => h.Key.ToString()).ToList());
            Count.Set(context, Duplicates.Get(context).Count);

            if (Verbose.Get(context))
            {
                Console.WriteLine("Duplicates in Column  [ " + DT.Get(context).Columns[idx].ColumnName + " ] (" + DT.Get(context).Columns[idx].DataType.ToString() + ") = " + Count.Get(context));
                Console.WriteLine("Duplicates: " + Environment.NewLine + String.Join(Environment.NewLine, Duplicates.Get(context)));
            }
        }

        #endregion

    }
}