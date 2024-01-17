using System.Activities;
using System.ComponentModel;
using System.Data;

namespace UiPathTeam.DataTableStatistics.Activities
{
    [DisplayName("DataTable Viewer"), Description("Allows user to interact with a read only view of the DataTable.")]
    public class DataTableViewer : DataTableEditor
    {

        #region Properties

        [Browsable(false)]
        public override OutArgument<DataTable> DT_Output { get; set; }

        protected override bool IsEditor { get; set; } = false;

        #endregion

    }
}