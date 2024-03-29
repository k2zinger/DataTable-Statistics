using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace UiPathTeam.DataTableStatistics.Activities
{
    [DisplayName("Get Minimum and Maximum"), Description("Retrieves the minimum and maximum numerical values within the user-specified column.")]
    public class GetMinMax : NativeActivity
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

        [Category("Output"), Description("The ouptut minimum value")]
        public OutArgument<Double> Minimum { get; set; }

        [Category("Output"), Description("The ouptut maximum value")]
        public OutArgument<Double> Maximum { get; set; }

        #endregion

        #region CodeActivity

        protected override void Execute(NativeActivityContext context)
        {
            Utilities.validateDatatable(context, DT);
            int idx = Utilities.validateColumnNameOrNumber(context, DT, ColumnName, ColumnNumber);

            if (DT.Get(context).AsEnumerable().All(x => (x[idx].ToString().Trim()).All(char.IsNumber)))
            {
                Minimum.Set(context, DT.Get(context).AsEnumerable().Min(x => Convert.ToDouble(x[idx].ToString().Trim())));
                Maximum.Set(context, DT.Get(context).AsEnumerable().Max(x => Convert.ToDouble(x[idx].ToString().Trim())));
            }
            else
            {
                throw new Exception("Cannot find Minimum and Maximum since some data within the specified column is non-numeric.");
            }
            
            if (Verbose.Get(context))
            {
                Console.WriteLine("MIN Column: [ " + DT.Get(context).Columns[idx].ColumnName + " ] = " + Minimum.Get(context).ToString("N"));
                Console.WriteLine("MAX Column: [ " + DT.Get(context).Columns[idx].ColumnName + " ] = " + Maximum.Get(context).ToString("N"));
            }
        }

        #endregion

    }
}