// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="urb31075">
// All Right Reserved  
// </copyright>
// <summary>
//   Defines the MainForm type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MilestoneReportDisainer
{
    using System;
    using System.Data.OracleClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using FastReport;
    using FastReport.Export.OoXML;

    using MilestoneReportDAL;

    /// <summary>
    /// The main form.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private const string ConnectionString = "Data Source=BAZA.WORLD;Persist Security Info=True;User ID=XXXXX;Password=YYY;Unicode=True";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
        }

        #region Вспомогательные функции

        /// <summary>
        /// The delete reports from blob.
        /// </summary>
        /// <param name="reportName">
        /// The report name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool DeleteReportsFromBlob(string reportName)
        {
            try
            {
                const string QueryString = "delete from COSMOS_REPORT_FORMS where name = :REPORTNAME";

                using (var connection = new OracleConnection(ConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = QueryString;
                        command.Parameters.AddWithValue("REPORTNAME", reportName);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK);
            }

            return false;
        }

        /// <summary>
        /// The save reports to blob.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="reportName">
        /// The report name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool SaveReportsToBlob(string fileName, string reportName, string description)
        {
            try
            {
                const string QueryString = "insert into COSMOS_REPORT_FORMS (NAME, REPORT_FORM, DESCR) values (:ReportName, :ReportForm, :Descr)";

                using (var connection = new OracleConnection(ConnectionString))
                {
                    var blob = File.ReadAllBytes(fileName);
                    var blobParameter = new OracleParameter
                    {
                        OracleType = OracleType.Blob,
                        ParameterName = "ReportForm",
                        Value = blob
                    };

                    using (var command = connection.CreateCommand())
                    {
                        command.Parameters.Add(blobParameter);
                        command.Parameters.AddWithValue("ReportName", reportName);
                        command.Parameters.AddWithValue("Descr", description);
                        command.CommandText = QueryString;
                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK);
            }

            return false;
        }

        /// <summary>
        /// The read blob to file.
        /// </summary>
        /// <param name="reportName">
        /// The report name.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool ReadBlobToFile(string reportName, string fileName)
        {
            try
            {
                const string QueryString = "select REPORT_FORM from LOTSIA_REPORT_FORMS where NAME = :REPORTNAME";

                using (var connection = new OracleConnection(ConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Parameters.AddWithValue("REPORTNAME", reportName);
                        command.CommandText = QueryString;
                        connection.Open();
                        using (var rdr = command.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                var buffer = (byte[])rdr.GetOracleLob(0).Value;
                                var content = new string(Encoding.UTF8.GetChars(buffer));
                                File.WriteAllText(fileName, content);
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Ошибка!", MessageBoxButtons.OK);
            }

            return false;
        }

        #endregion

        /// <summary>
        /// The report button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ReportButtonClick(object sender, EventArgs e)
        {
            var milestoneReportDal = new MilestoneReportDal();
            var milestoneReportCommonInfo = milestoneReportDal.GetMilestoneReportCommonInfo();

            var startDate = Convert.ToDateTime("01.09.2017");
            var finishDate = Convert.ToDateTime("30.09.2017");
            var dates_filter_type = 1;
            var case_id = 9;
            
            // var milestoneReportData = milestoneReportDal.GetMilestoneReportData(startDate, finishDate, dates_filter_type, case_id, "11169", 20);

            var milestoneReportData = milestoneReportDal.GetMilestoneReportData(startDate, finishDate, dates_filter_type, case_id, string.Empty, 2000);

            var report = new Report();
            report.Load(this.ReportTemplatePathTextBox.Text);

            report.RegisterData(milestoneReportCommonInfo, "CommonInfo");
            report.RegisterData(milestoneReportData, "Data");
            if (this.DesignModeCheckBox.Checked)
            {
                report.Design(false);
            }
            else
            {
                report.Show(false);
            }
        }

        /// <summary>
        /// The export button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ExportButtonClick(object sender, EventArgs e)
        {
            var milestoneReportDal = new MilestoneReportDal();
            var milestoneReportCommonInfo = milestoneReportDal.GetMilestoneReportCommonInfo();

            var startDate = Convert.ToDateTime("01.09.2017");
            var finishDate = Convert.ToDateTime("30.09.2017");
            var dates_filter_type = 1;
            var case_id = 9;
            var milestoneReportData = milestoneReportDal.GetMilestoneReportData(startDate, finishDate, dates_filter_type, case_id, "12181", 20);

            var report = new Report();
            report.Load(this.ReportTemplatePathTextBox.Text);

            report.RegisterData(milestoneReportCommonInfo, "CommonInfo");
            report.RegisterData(milestoneReportData, "Data");

            report.Prepare();

            //// var xmlExport = new XMLExport { OpenAfterExport = true };
            //// report.Export(xmlExport, "D:\\DupelReport.xml");

            //// var pptExport = new PowerPoint2007Export{ OpenAfterExport = true };
            //// report.Export(pptExport, "D:\\DupelReport.ppt");

            var excelExport = new Excel2007Export { OpenAfterExport = true };
            report.Export(excelExport, "D:\\DupelReport.xlsx");

            ////var worldExport = new Word2007Export { OpenAfterExport = true };
            ////report.Export(worldExport, "D:\\DupelReport.doc");

            //var pdfExport = new PDFExport { OpenAfterExport = true };
            //report.Export(pdfExport, @"D:\DupelReport.pdf");
        }

        /// <summary>
        /// The save to blob button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SaveToBlobButtonClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Произвести перезапись шаблона отчета в базе данных?", @"Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var result = DeleteReportsFromBlob(@"MilestoneReport");
            if (!result)
            {
                MessageBox.Show(@"Ошибка удаления шаблона из базы данных!", @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            result = SaveReportsToBlob(this.ReportTemplatePathTextBox.Text, @"MilestoneReport", @"Отчет по контрольным точкам проекта");
            if (result)
            {
                MessageBox.Show(@"Шаблон вставлен в базу данных!", @"Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(@"Ошибка записи шаблона в базу данных!", @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// The read from blob button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ReadFromBlobButtonClick(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = @"frx files (*.frx)|*.frx|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var result = ReadBlobToFile(@"MilestoneReport", saveFileDialog.FileName);
                if (result)
                {
                    MessageBox.Show(@"Шаблон записан в файл!", @"Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(@"Ошибка чтения шаблона из базу данных!", @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// The report from db butto_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ReportFromDbButtoClick(object sender, EventArgs e)
        {
            var milestoneReportDal = new MilestoneReportDal();
            var milestoneReportCommonInfo = milestoneReportDal.GetMilestoneReportCommonInfo();

            var startDate = Convert.ToDateTime("01.09.2017");
            var finishDate = Convert.ToDateTime("30.09.2017");
            var dates_filter_type = 1;
            var case_id = 9;
            var milestoneReportData = milestoneReportDal.GetMilestoneReportData(startDate, finishDate, dates_filter_type, case_id, "12181", 20);

            var report = new Report();
            var reportTemplate = this.ReadReportTemplate("MilestoneReport");
            report.Load(reportTemplate);

            report.RegisterData(milestoneReportCommonInfo, "CommonInfo");
            report.RegisterData(milestoneReportData, "Data");
            report.Show(false);
        }

        /// <summary>
        /// The read report template.
        /// </summary>
        /// <param name="reportName">
        /// The report name.
        /// </param>
        /// <returns>
        /// The <see cref="MemoryStream"/>.
        /// </returns>
        public MemoryStream ReadReportTemplate(string reportName)
        {
            try
            {
                const string QueryString = "select REPORT_FORM from COSMOS_REPORT_FORMS where NAME = :REPORTNAME";
                const string CosmosConnectionString = "Data Source = BAZA.WORLD; Persist Security Info = True; User ID = gpe_cosmos; Password = pdtplf; Unicode = True";
                using (var connection = new OracleConnection(CosmosConnectionString))
                {
                    var command = connection.CreateCommand();
                    command.Parameters.AddWithValue("REPORTNAME", reportName);
                    command.CommandText = QueryString;
                    connection.Open();
                    using (var rdr = command.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            var buffer = (byte[])rdr.GetOracleLob(0).Value;
                            var ms = new MemoryStream(buffer);
                            return ms;
                        }
                    }
                }
            }
            catch
            {
                // ProcException(ex, "Ошибка при загрузке отчета");
                // ignore
            }

            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
