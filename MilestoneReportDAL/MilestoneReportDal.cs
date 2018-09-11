// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MilestoneReportDal.cs" company="urb31075">
// All Right Reserved  
// </copyright>
// <summary>
//   Defines the MilestoneReportDal type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MilestoneReportDAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.OracleClient;
    using System.Data.SqlClient;
    using System.Linq;

    /// <summary>
    /// DAL для получения информации о вехах проекта
    /// </summary>
    public class MilestoneReportDal
    {
        /// <summary>
        /// The cosmos connection string.
        /// </summary>
        private const string CosmosConnectionString = "Data Source = BAZA.WORLD; Persist Security Info = True; User ID = gpe_cosmos; Password = pdtplf; Unicode = True";

        /// <summary>
        /// The ms project connection string.
        /// </summary>
        private const string MsProjectConnectionString = "Data Source = SQL2008; Initial Catalog = GPIProjectServer_Reporting; Persist Security Info=True;User ID = kasupi; Password=kasupiuser";

        /// <summary>
        /// Gets or sets the last error.
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// The get milestone categories info list.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<MilestoneCategoriesInfo> GetMilestoneFullCategoriesInfoList()
        {
            this.LastError = string.Empty;
            var milestoneInfoList = new List<MilestoneCategoriesInfo>();
            var milestoneCategoriesInfoDictionary = new Dictionary<int, MilestoneCategoriesInfo>();
            var milestonePointInfoDictionary = new Dictionary<int, List<MilestonePointInfo>>();
            try
            {
                var queryString = MilestoneReportResource.GetMilestoneFullListSqlReqest;
                using (var connection = new OracleConnection(CosmosConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = queryString;
                        connection.Open();
                        using (var rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var milestoneCategoriesInfo = new MilestoneCategoriesInfo
                                {
                                    CatId = rdr["ID_MS_CAT"] == DBNull.Value ? -1 : Convert.ToInt32(rdr["ID_MS_CAT"]),
                                    CatName = rdr["MS_CAT_NAME"] == DBNull.Value ? string.Empty : rdr["MS_CAT_NAME"].ToString(),
                                    CatOrder = rdr["MS_CAT_ORD"] == DBNull.Value ? -1 : Convert.ToInt32(rdr["MS_CAT_ORD"])
                                };

                                var milestonePointInfo = new MilestonePointInfo
                                {
                                    MilestoneId = rdr["ID_MILESTONE"] == DBNull.Value ? -1 : Convert.ToInt32(rdr["ID_MILESTONE"]),
                                    MilestoneName = rdr["MILESTONE_NAME"] == DBNull.Value ? string.Empty : rdr["MILESTONE_NAME"].ToString(),
                                    MilestoneOrder = rdr["ORD"] == DBNull.Value ? -1 : Convert.ToInt32(rdr["ORD"]),
                                };

                                if (!milestoneCategoriesInfoDictionary.ContainsKey(milestoneCategoriesInfo.CatId))
                                {
                                    milestoneCategoriesInfoDictionary.Add(milestoneCategoriesInfo.CatId, milestoneCategoriesInfo);
                                }

                                if (!milestonePointInfoDictionary.ContainsKey(milestoneCategoriesInfo.CatId))
                                {
                                    milestonePointInfoDictionary.Add(milestoneCategoriesInfo.CatId, new List<MilestonePointInfo>());
                                }

                                if (milestonePointInfoDictionary[milestoneCategoriesInfo.CatId].All(c => c.MilestoneId != milestonePointInfo.MilestoneId))
                                {
                                    milestonePointInfoDictionary[milestoneCategoriesInfo.CatId].Add(milestonePointInfo);
                                }
                            }
                        }
                    }
                }

                foreach (var categories in milestoneCategoriesInfoDictionary)
                {
                    milestoneInfoList.Add(categories.Value);
                }

                foreach (var milestoneInfo in milestoneInfoList)
                {
                    milestoneInfo.MilestonePointInfoList = milestonePointInfoDictionary[milestoneInfo.CatId];
                }

            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
            }

            return milestoneInfoList;

        }

        /// <summary>
        /// The get milestone report common info.
        /// </summary>
        /// <returns>
        /// Возврат общей информации о проекте
        /// </returns>
        public List<MilestoneReportCommonInfo> GetMilestoneReportCommonInfo()
        {
            return new List<MilestoneReportCommonInfo>
                       {
                           new MilestoneReportCommonInfo
                           {
                               ReportName = "Режим отладки отчета по контрольным точкам проекта",
                               GenerateDate = DateTime.Now
                           }
                       };
        }

        /// <summary>
        /// The get milestone report data.
        /// </summary>
        /// <param name="startDate">
        /// The start Date.
        /// </param>
        /// <param name="finishDate">
        /// The finish Date.
        /// </param>
        /// <param name="datesFilterType">
        /// The dates Filter Type.
        /// </param>
        /// <param name="caseId">
        /// The case Id.
        /// </param>
        /// <param name="dog">
        /// The dog.
        /// </param>
        /// <param name="maxAmount">
        /// The max Amount.
        /// </param>
        /// <returns>
        /// Возврат информации о вехах проекта
        /// </returns>
        public List<MilestoneReportData> GetMilestoneReportData(DateTime startDate, DateTime finishDate, int datesFilterType, int caseId, string dog, int maxAmount = 10000)
        {
            var start = DateTime.Now;
            var milestoneReportDataList = this.GetMilestoneInfo(startDate, finishDate, datesFilterType, caseId, dog);
            var count = 0;
            foreach (var milestoneReportData in milestoneReportDataList)
            {
                if (count++ < maxAmount)
                { // Костыль для отладки что бы побыстрее
                    milestoneReportData.MsProjectInfo = this.GetMsProjectExists(milestoneReportData.Dog);
                    milestoneReportData.MilestoneCategiriesInfoList = this.GetMilestoneForDogovor(milestoneReportData.Dog, milestoneReportData.Dc);
                }
                else
                {
                    milestoneReportData.MsProjectInfo = new MsProjectInfo();
                    milestoneReportData.MilestoneCategiriesInfoList = new List<MilestoneCategoriesInfo>();
                }
            }

            var span = (DateTime.Now - start).TotalMilliseconds;
            return milestoneReportDataList;
        }

        /// <summary>
        /// The get milestone for dogovor.
        /// </summary>
        /// <param name="dog">
        /// The dog.
        /// </param>
        /// <param name="dc">
        /// The dc.
        /// </param>
        /// <returns>
        /// Получение контрольных точек для договора
        /// </returns>
        public List<MilestoneCategoriesInfo> GetMilestoneForDogovor(string dog, string dc)
        {
            var start = DateTime.Now;
            this.LastError = string.Empty;
            var milestoneInfoList = new List<MilestoneCategoriesInfo>();
            var milestoneCategoriesInfoDictionary = new Dictionary<int, MilestoneCategoriesInfo>();
            var milestonePointInfoDictionary = new Dictionary<int, List<MilestonePointInfo>>();
            try
            {
                var queryString = MilestoneReportResource.GetMilestoneForDogovorSqlReqest;
                using (var connection = new OracleConnection(CosmosConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Parameters.AddWithValue("DOGOVOR", dog);
                        command.Parameters.AddWithValue("DC", dc);
                        command.CommandText = queryString;
                        connection.Open();
                        using (var rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var milestoneCategoriesInfo = new MilestoneCategoriesInfo
                                { 
                                    CatId = rdr["ID_MS_CAT"] == DBNull.Value ? -1 : Convert.ToInt32(rdr["ID_MS_CAT"]),
                                    CatName = rdr["MS_CAT_NAME"] == DBNull.Value ? string.Empty : rdr["MS_CAT_NAME"].ToString(),
                                    CatOrder = rdr["MS_CAT_ORD"] == DBNull.Value ? -1 : Convert.ToInt32(rdr["MS_CAT_ORD"])
                                };

                                var milestonePointInfo = new MilestonePointInfo
                                { 
                                       MilestoneId = rdr["ID_MILESTONE"] == DBNull.Value ? -1 : Convert.ToInt32(rdr["ID_MILESTONE"]),
                                       MilestoneName = rdr["MILESTONE_NAME"] == DBNull.Value ? string.Empty : rdr["MILESTONE_NAME"].ToString(),
                                       MilestoneIdMsp = rdr["ID_MSP"] == DBNull.Value ? string.Empty : rdr["ID_MSP"].ToString(),
                                       MilestoneOrder = rdr["ORD"] == DBNull.Value ? -1 : Convert.ToInt32(rdr["ORD"]),
                                       ProjectUserName = rdr["USER_NAME"] == DBNull.Value ? string.Empty : rdr["USER_NAME"].ToString(),
                                       ProjectFactData = rdr["FACT_DATE"] == DBNull.Value ? string.Empty : Convert.ToDateTime(rdr["FACT_DATE"]).ToShortDateString()
                                };

                                milestonePointInfo.MilestoneDateInfoList = this.GetMilestoneDateList(milestonePointInfo.MilestoneIdMsp);
                                if (milestonePointInfo.MilestoneDateInfoList.Any())
                                {
                                    if (!milestoneCategoriesInfoDictionary.ContainsKey(milestoneCategoriesInfo.CatId))
                                    {
                                        milestoneCategoriesInfoDictionary.Add(milestoneCategoriesInfo.CatId,milestoneCategoriesInfo);
                                    }

                                    if (!milestonePointInfoDictionary.ContainsKey(milestoneCategoriesInfo.CatId))
                                    {
                                        milestonePointInfoDictionary.Add(milestoneCategoriesInfo.CatId, new List<MilestonePointInfo>());
                                    }

                                    if (milestonePointInfoDictionary[milestoneCategoriesInfo.CatId].All(c => c.MilestoneId != milestonePointInfo.MilestoneId))
                                    {
                                        milestonePointInfoDictionary[milestoneCategoriesInfo.CatId].Add(milestonePointInfo);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var categories in milestoneCategoriesInfoDictionary)
                {
                    milestoneInfoList.Add(categories.Value);
                } 

                foreach (var milestoneInfo in milestoneInfoList)
                {
                    milestoneInfo.MilestonePointInfoList = milestonePointInfoDictionary[milestoneInfo.CatId];
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
            }

            var span = (DateTime.Now - start).TotalMilliseconds;
            return milestoneInfoList;
        }

        /// <summary>
        /// The get milestone date list.
        /// </summary>
        /// <param name="idmsp">
        /// The idmsp.
        /// </param>
        /// <returns>
        /// Пролучение дат контрольных точек
        /// </returns>
        public List<MilestoneDateInfo> GetMilestoneDateList(string idmsp)
        {
            var milestoneDateInfoList = new List<MilestoneDateInfo>();
            if (string.IsNullOrEmpty(idmsp))
            {
                return milestoneDateInfoList;
            }

            // d34454fa-a9cc-4366-93f3-76f07bfb76ac
            this.LastError = string.Empty;
            try
            {
                var queryString = MilestoneReportResource.GetMilestouneDateSqlRequest;
                using (var connection = new SqlConnection(MsProjectConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Parameters.AddWithValue("ID_MSP", idmsp);
                        command.CommandText = queryString;
                        connection.Open();
                        using (var rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var milestoneDateInfo = new MilestoneDateInfo();
                                milestoneDateInfo.TaskStartDate = rdr["TaskStartDate"] == DBNull.Value? string.Empty : Convert.ToDateTime(rdr["TaskStartDate"]).ToShortDateString();
                                milestoneDateInfo.TaskFinishDate = rdr["TaskFinishDate"] == DBNull.Value ? string.Empty : Convert.ToDateTime(rdr["TaskFinishDate"]).ToShortDateString();
                                milestoneDateInfo.ControlPointType = rdr["Тип контрольной точки"] == DBNull.Value ? string.Empty : rdr["Тип контрольной точки"].ToString();
                                milestoneDateInfoList.Add(milestoneDateInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
            }

            return milestoneDateInfoList;
        }

        /// <summary>
        /// The get ms project exists.
        /// </summary>
        /// <param name="dog">
        /// The dog.
        /// </param>
        /// <returns>
        /// Получение информации по договору из  MsProject
        /// </returns>
        private MsProjectInfo GetMsProjectExists(string dog)
        {
            this.LastError = string.Empty;
            var msprojectInfo = new MsProjectInfo();
            try
            {
                var queryString = MilestoneReportResource.IsExistsInMsProjectSqlRequest;
                using (var connection = new SqlConnection(MsProjectConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Parameters.AddWithValue("dog", dog);
                        command.CommandText = queryString;
                        connection.Open();
                        using (var rdr = command.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                msprojectInfo.Dog = rdr["dog"] == DBNull.Value ? string.Empty : rdr["dog"].ToString();
                                msprojectInfo.Dc = rdr["dc"] == DBNull.Value ? string.Empty : rdr["dc"].ToString();
                                msprojectInfo.CreatDate = rdr["creat_date"] == DBNull.Value ? string.Empty : rdr["creat_date"].ToString();
                                msprojectInfo.Status = rdr["status"] == DBNull.Value ? string.Empty : rdr["status"].ToString();
                                msprojectInfo.Works = rdr["works"] == DBNull.Value ? string.Empty : rdr["works"].ToString();
                            }
                        }

                        msprojectInfo.IsExists = string.IsNullOrEmpty(msprojectInfo.CreatDate) ? "Нет" : "Есть";
                    }
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
            }

            return msprojectInfo;
        }

        /// <summary>
        /// The get milestone info.
        /// </summary>
        /// <param name="startDate">
        /// The start date.
        /// </param>
        /// <param name="finishDate">
        /// The finish date.
        /// </param>
        /// <param name="datesFilterType">
        /// The dates filter type.
        /// </param>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <param name="dog">
        /// The dog.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<MilestoneReportData> GetMilestoneInfo(DateTime startDate, DateTime finishDate, int datesFilterType, int caseId, string dog)
        {
            this.LastError = string.Empty;
            var milestoneReportDataList = new List<MilestoneReportData>();
            try
            {
                const string QueryString = "select * from table(ENGINEERING_SURVEY_PKG.GETPROJECTINFO(:startDate, :finishDate, :isAgreed, :in_gip, :projName, :datesFilterType, :caseId, :in_dog))";
                using (var connection = new OracleConnection(CosmosConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Parameters.AddWithValue("startDate", startDate);
                        command.Parameters.AddWithValue("finishDate", finishDate);
                        command.Parameters.AddWithValue("datesFilterType", datesFilterType);
                        command.Parameters.AddWithValue("caseId", caseId);
                        command.Parameters.AddWithValue("projName", DBNull.Value);
                        command.Parameters.AddWithValue("isAgreed", DBNull.Value);
                        command.Parameters.AddWithValue("in_gip", DBNull.Value);

                        if (string.IsNullOrEmpty(dog))
                        {
                            command.Parameters.AddWithValue("in_dog", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("in_dog", dog);
                        }

                        command.CommandText = QueryString;
                        connection.Open();
                        using (var rdr = command.ExecuteReader())
                        {
                            var number = 1;
                            while (rdr.Read())
                            {
                                var milestoneReportData = new MilestoneReportData
                                {
                                    Number = number++,
                                    Prd = rdr["PRD"] == DBNull.Value ? string.Empty : rdr["PRD"].ToString(),
                                    Dog = rdr["DOG"] == DBNull.Value ? string.Empty : rdr["DOG"].ToString(),
                                    Dc = rdr["DC"] == DBNull.Value ? string.Empty : rdr["DC"].ToString(),
                                    Ct = rdr["CT"] == DBNull.Value ? string.Empty : rdr["CT"].ToString(),
                                    Npr = rdr["NPR"] == DBNull.Value ? string.Empty : rdr["NPR"].ToString(),
                                    Fio = rdr["FIO"] == DBNull.Value ? string.Empty : rdr["FIO"].ToString(),
                                    KpStart = rdr["KP_START"] == DBNull.Value ? string.Empty : Convert.ToDateTime(rdr["KP_START"]).ToShortDateString(),
                                    KpFinish = rdr["KP_FINISH"] == DBNull.Value ? string.Empty : Convert.ToDateTime(rdr["KP_FINISH"]).ToShortDateString(),
                                    PpStart = rdr["PP_START"] == DBNull.Value ? string.Empty : Convert.ToDateTime(rdr["PP_START"]).ToShortDateString(),
                                    PpFinish = rdr["PP_FINISH"] == DBNull.Value ? string.Empty : Convert.ToDateTime(rdr["PP_FINISH"]).ToShortDateString(),
                                    DokPrim = rdr["DOK_PRIM"] == DBNull.Value ? string.Empty : rdr["DOK_PRIM"].ToString(),
                                    GrafStatus = rdr["GRAF_STATUS"] == DBNull.Value ? string.Empty : rdr["GRAF_STATUS"].ToString(),
                                    VpkStatus = rdr["VPK_STATUS"] == DBNull.Value ? string.Empty : rdr["VPK_STATUS"].ToString()
                                };
                                milestoneReportDataList.Add(milestoneReportData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
            }

            return milestoneReportDataList;
        }
    }

    /// <summary>
    /// The milestone report common info.
    /// </summary>
    public class MilestoneReportCommonInfo
    {
        /// <summary>
        /// Gets or sets the report name.
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the generate date.
        /// </summary>
        public DateTime GenerateDate { get; set; }
    }

    /// <summary>
    /// The milestone report data.
    /// </summary>
    public class MilestoneReportData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MilestoneReportData"/> class.
        /// </summary>
        public MilestoneReportData()
        {
            this.Number = 0;
            this.Prd = string.Empty;
            this.Dog = string.Empty;
            this.Dc = string.Empty;
            this.Fio = string.Empty;
            this.Ct = string.Empty;
            this.Npr = string.Empty;
            this.KpFinish = string.Empty;
            this.KpStart = string.Empty;
            this.KpFinish = string.Empty;
            this.PpStart = string.Empty;
            this.PpFinish = string.Empty;
            this.DokPrim = string.Empty;
            this.GrafStatus = string.Empty;
            this.VpkStatus = string.Empty;
            this.MsProjectInfo = new MsProjectInfo();
            this.MilestoneCategiriesInfoList = new List<MilestoneCategoriesInfo>();
        }

        public int Number { get; set; }

        public string Prd { get; set; }

        public string Dog { get; set; }

        public string Dc { get; set; }

        public string Fio { get; set; }

        public string Ct { get; set; }

        public string Npr { get; set; }

        public string KpStart { get; set; }

        public string KpFinish { get; set; }

        public string PpStart { get; set; }

        public string PpFinish { get; set; }

        public string DokPrim { get; set; }

        public string GrafStatus { get; set; }

        public string VpkStatus { get; set; }

        public MsProjectInfo MsProjectInfo { get; set; }

        public List<MilestoneCategoriesInfo> MilestoneCategiriesInfoList { get; set; }
    }

    public class MsProjectInfo
    {
        public MsProjectInfo()
        {
            this.Dog = string.Empty;
            this.Dc = string.Empty;
            this.CreatDate = string.Empty;
            this.Status = string.Empty;
            this.Works = string.Empty;
            this.IsExists = string.Empty;
        }

        public string Dog { get; set; }

        public string Dc { get; set; }

        public string CreatDate { get; set; }

        public string Status { get; set; }

        public string Works { get; set; }

        public string IsExists { get; set; }
    }

    public class MilestoneCategoriesInfo
    {
        public MilestoneCategoriesInfo()
        {
            this.CatId = -1;
            this.CatName = string.Empty;
            this.MilestonePointInfoList = new List<MilestonePointInfo>();
        }

        public int CatId { get; set; }

        public string CatName { get; set; }

        public int CatOrder { get; set; }

        public List<MilestonePointInfo> MilestonePointInfoList { get; set; }
    }

    public class MilestonePointInfo
    {
        public MilestonePointInfo()
        {
            this.MilestoneId = -1;
            this.MilestoneName = string.Empty;
            this.MilestoneIdMsp = string.Empty;
            this.MilestoneOrder = 0;
            this.MilestoneDateInfoList = new List<MilestoneDateInfo>();
        }

        public int MilestoneId { get; set; }

        public string MilestoneName { get; set; }

        public string MilestoneIdMsp { get; set; }

        public int MilestoneOrder { get; set; }

        public List<MilestoneDateInfo> MilestoneDateInfoList { get; set; }
        public string ProjectFactData { get; set; }

        public string ProjectUserName { get; set; }
    }

    public class MilestoneDateInfo
    {
        public MilestoneDateInfo()
        {
            this.TaskStartDate = string.Empty;
            this.TaskFinishDate = string.Empty;
            this.ControlPointType = string.Empty;
        }

        public string TaskStartDate { get; set; }

        public string TaskFinishDate { get; set; }

        public string ControlPointType { get; set; }
    }
}
