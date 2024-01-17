using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace UiPathTeam.DataTableStatistics.Activities
{
    [DisplayName("DataTable Editor"), Description("Allows user to edit cells, add and remove rows, sort columns, and save the changes.")]
    public class DataTableEditor : NativeActivity
    {

        #region Properties

        [Category("Input"), Description("Datatable to show in the Status message")]
        [RequiredArgument]
        public InArgument<DataTable> DT_Input { get; set; }

        [Category("Input"), Description("Background opacity.  Value should be betwen 0 and 1.  Default: 1")]
        public InArgument<Double> Opacity { get; set; } = 1;

        [Category("Input"), Description("Text to be displayed in the UI Title Bar.")]
        public InArgument<String> Title { get; set; }

        [Category("Input"), Description("Set to true to bring the form to the front of all other windows.  Default: True")]
        public InArgument<Boolean> Top { get; set; } = true;

        [Category("Input"), Description("Set TRUE to output debugging information to the console.  Default: False")]
        public InArgument<Boolean> Verbose { get; set; } = false;

        [Category("Output"), Description("Output Datatable upon clicking the Save button")]
        public virtual OutArgument<DataTable> DT_Output { get; set; }

        protected virtual bool IsEditor { get; set; } = true;

        #endregion

        #region CodeActivity

        protected override void Execute(NativeActivityContext context)
        {
            if (DT_Input == null || DT_Input.Get(context) == null)
            {
                DT_Input.Set(context, new DataTable());
            }

            if (Verbose.Get(context))
            {
                Console.WriteLine("Input Datatable has: " + (DT_Input.Get(context).Rows.Count - 1).ToString() + " Rows.");
            }

            if (Title.Get(context).Length > 120)
            {
                Title.Set(context, Title.Get(context).Substring(0, 120));
                Console.WriteLine("Message was too long.  Truncating to 120 characters");
            }

            DataTable dtNew = new DataTable();
            int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            int ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

            Form frm = new Form();
            Label lbl = new Label();
            Panel pnl = new Panel();
            Button btn = new Button();
            Button btnSave = new Button();

            frm.StartPosition = FormStartPosition.CenterScreen;

            // Set Colors
            frm.BackColor = Color.WhiteSmoke;
            pnl.BackColor = Color.WhiteSmoke;
            lbl.ForeColor = Color.Black;

            frm.TopMost = Top.Get(context);
            frm.ControlBox = true;
            frm.Text = string.Empty;
            frm.Text = (IsEditor ? "Editor: " : "Viewer: ") + Title.Get(context);
            frm.Opacity = Opacity.Get(context);
            frm.Padding = new Padding(1);
            frm.ShowInTaskbar = false;
            frm.AutoSize = true;
            // frm.BackColor = Color.OrangeRed
            frm.Location = new Point(0, 0);
            frm.ClientSize = new Size(100, 100); // minimun

            pnl.Location = new Point(2, 2);
            pnl.Size = new Size(frm.Width, frm.Height);
            pnl.AutoSize = true;
            // pnl.BackColor = Color.Red

            // lbl.ForeColor = Color.White
            lbl.BackColor = Color.Transparent;
            lbl.Font = new Font(lbl.Font.FontFamily, 8.5f);
            lbl.Size = new Size(pnl.Width, pnl.Height);
            lbl.AutoSize = true;
            lbl.Padding = new Padding(5);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = (IsEditor ? "Editor" : "Viewer");// Title

            btn.Name = "CloseBtn";
            btn.Font = new Font("Malett", 8.5f);
            btn.Text = "CLOSE";
            btn.Size = new Size(55, 20);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.OrangeRed;
            btn.ForeColor = Color.White;

            btnSave.Name = "SaveBtn";
            btnSave.Font = new Font("Malett", 8.5f);
            btnSave.Text = "SAVE";
            //btn.Size = new Size(55, 20);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.BackColor = Color.ForestGreen;
            btnSave.ForeColor = Color.White;

            pnl.Controls.Add(lbl);
            pnl.Controls.Add(btn);
            pnl.Controls.Add(btnSave);

            // Build DataTable-----------------------------------------------------------
            int dvSRowLimit = (DT_Input.Get(context).Rows.Count < 30) ? (DT_Input.Get(context).Rows.Count + 1) : 30;
            int dvSColLimit = (DT_Input.Get(context).Columns.Count < 25) ? (DT_Input.Get(context).Columns.Count + 1) : 25;

            Label block = new Label();
            DataGridView dgv = new DataGridView();

            if (DT_Input.Get(context).Rows.Count > 0)
            {
                dgv.Name = "mydgv";
                dgv.DataSource = DT_Input.Get(context);
                dgv.AllowUserToAddRows = IsEditor;
                dgv.AllowUserToDeleteRows = IsEditor;
                if (!IsEditor)
                {
                    dgv.EditMode = DataGridViewEditMode.EditProgrammatically;
                }
                dgv.Width = 60 + 80 * dvSColLimit;
                dgv.Height = 60 + 20 * dvSRowLimit;

                lbl.Width = dvSRowLimit * 100;

                // console.WriteLine(dgV.Width.ToString+", "+dgv.Height.ToString)
                // console.WriteLine((60+(80*dvSColLimit)).ToString+", "+(20+(20*dvSRowLimit)).ToString)

                pnl.Controls.Add(dgv);
                dgv.Top = lbl.Bottom + 10;
                dgv.Left = 0;
                dgv.ColumnHeadersHeight = 50;
                // dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                // dgv. = New Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Point)
                // dgv.AutoSize = False
                // dgv.ScrollBars = ScrollBars.Both
                // dgv.rows(0).DefaultCellStyle.BackColor = Color.Black
                dgv.BackgroundColor = Color.Black;
                // block.Width = dgv.Width
                // block.Height = 40
                block.Top = dgv.Bottom;
                block.Text = "block";
                // block.backColor = system.Drawing.Color.Red
                // pnl.controls.Add(block)
            }
            // ----------------------------------------------------------------------------

            frm.Controls.Add(pnl);
            btn.BringToFront();
            btnSave.BringToFront();

            btn.ClientSize = btnSave.ClientSize;
            btnSave.Location = new Point(5, 5);
            btn.Location = new Point(btnSave.Right + 5, 5);
            if (!IsEditor)
            {
                btnSave.Visible = false;
                btnSave.Enabled = false;
            }
            lbl.Location = new Point(btn.Right + 5, 2);

            btn.Click += (object sender, EventArgs e) => frm.Close();

            btnSave.Click += (object sender, EventArgs e) =>
            {
                lbl.ForeColor = Color.Black;
                lbl.Text = "SAVING CHANGES.........";
                frm.Refresh();

                bool errd = false;
                // Dim dtNew As DataTable = New DataTable
                foreach (DataGridViewColumn column in dgv.Columns)
                    dtNew.Columns.Add(column.HeaderText, column.ValueType);

                // Extract DataTable from DGV
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (errd)
                    {
                        break;
                    }

                    dtNew.Rows.Add();
                    int cind = 0;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        try
                        {
                            dtNew.Rows[dtNew.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                        }
                        catch (Exception)
                        {
                            // system.console.WriteLine("Empty Cell found at: Row:"+row.index.ToString+", Column"+cind.ToString+vbcrlf+"")
                            // errd = True
                            // Exit For
                        }
                        finally
                        {
                            cind++;
                        }
                    }
                }

                frm.Close();
            };

            if (lbl.Width > ScreenWidth - 20)
            {
                lbl.AutoSize = false;
                lbl.Width = 1000 - 20;
                lbl.Height = 300 - 10;

                pnl.AutoSize = false;
                pnl.Width = 1000 - 15;
                pnl.Height = 300 - 5;

                frm.AutoSize = false;
                frm.ClientSize = new Size(1000, 300);
            }

            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();

            if (IsEditor)
            {
                DT_Output.Set(context, dtNew.Copy());

                if (Verbose.Get(context))
                {
                    Console.WriteLine("Output Datatable has: " + (dtNew.Rows.Count - 1).ToString() + " Rows");
                }
            }
        }

        #endregion

    }
}