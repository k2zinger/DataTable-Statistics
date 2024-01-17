using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace UiPathTeam.DataTableStatistics.Activities
{
    [DisplayName("Get Average"), Description("Calculates the average of the numerical values within the user-specified column.")]
    public class GetAverage : NativeActivity
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

        [Category("Output"), Description("The ouptut calculated value")]
        public OutArgument<Double> Average { get; set; }

        #endregion

        #region CodeActivity

        protected override void Execute(NativeActivityContext context)
        {
            Utilities.validateDatatable(context, DT);
            int idx = Utilities.validateColumnNameOrNumber(context, DT, ColumnName, ColumnNumber);

            if (DT.Get(context).AsEnumerable().All(x => (x[idx].ToString().Trim()).All(char.IsNumber)))
            {
                Average.Set(context, DT.Get(context).AsEnumerable().Average(x => Convert.ToDouble(x[idx].ToString().Trim())));
            }
            else
            {
                throw new Exception("Cannot find Average since some data within the specified column is non-numeric.");
            }

            if (Verbose.Get(context))
            {
                Console.WriteLine("AVG Column: [ " + DT.Get(context).Columns[idx].ColumnName + " ] = " + Average.Get(context).ToString("N"));
            }
        }

        #endregion

    }
}