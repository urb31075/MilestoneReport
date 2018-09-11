using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MilestoneReportWeb
{
    using System.Data;
    using System.Linq;

    using DevExpress.Web;

    using MilestoneReportDAL;

    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void MilestoneReportButtonClick(object sender, EventArgs e)
        {
            var milestoneReportDal = new MilestoneReportDal();

            var startDate = Convert.ToDateTime("01.09.2017");
            var finishDate = DateTime.Now;
            var dates_filter_type = 1;
            var case_id = 9;

            var milestoneReportDataList = milestoneReportDal.GetMilestoneReportData(startDate, finishDate, dates_filter_type, case_id, "11169"); // Получение данных только для одного договора
            // var milestoneReportDataList = milestoneReportDal.GetMilestoneReportData(startDate, finishDate, dates_filter_type, case_id, string.Empty); // Получение данных для всех договоров

            
            // Получение полного списка категорий
            var fullMilestoneCategoriesInfoList = milestoneReportDal.GetMilestoneFullCategoriesInfoList(); // Получение полного списка категорий контрольных точек
            var filteredMilestoneCategoriesInfoList = fullMilestoneCategoriesInfoList.Where(c => c.CatId == 4); // Фильтрация категорий контрольных точек (только те категории, которые надо вывести на экран)
            
            // Вывод на экран только выбранных категорий контрольных точек
            this.CreateGridStructure(milestoneReportDataList, filteredMilestoneCategoriesInfoList);
            this.MSGridView.DataSource = this.PrepareForDataBinding(milestoneReportDataList, filteredMilestoneCategoriesInfoList); 
            
            // Вывод на экран только тех контрольных точек, по которым есть данные
            //this.CreateGridStructure(milestoneReportDataList);
            //this.MSGridView.DataSource = this.PrepareForDataBinding(milestoneReportDataList); // Создание данных для биндинга
            
            this.MSGridView.DataBind();
        }

        // Создание структуры колонок
        protected void CreateGridStructure(IReadOnlyList<MilestoneReportData> milestoneReportDataList, IEnumerable<MilestoneCategoriesInfo> viewCategoriesInfoList = null)
        {
            if (milestoneReportDataList == null)
            {
                return;
            }

            var categoriesList = viewCategoriesInfoList?.ToList() ?? this.GetCategoriesFromReportData(milestoneReportDataList); //Если категория задана то используем ее, иначе берем категории из данных для отчета

            this.CreateFixedColumn(); //Создаем набор фиксированных коллонок

            foreach (var categories in categoriesList) // Проход по всем категориям 
            {
                var bandColumn = new GridViewBandColumn { Caption = categories.CatName, Name = $"CatName{categories.CatId}" }; //Создали колонку для катенгории
                foreach (var point in categories.MilestonePointInfoList) // Проход по всем контрольным точкам данной категории
                {
                    // Создаем колонку для контрольной точки и три колонки для информации по датам контрольной точке
                    var pointColumn = new GridViewBandColumn { Caption = point.MilestoneName, Name = $"Point{point.MilestoneId}" };
                    var pointColumn1 = new GridViewDataTextColumn { Caption = @"Тип<br>контрольной<br>точки", FieldName = $"Data1.V{point.MilestoneId}", Width = Unit.Pixel(90) };
                    var pointColumn2 = new GridViewDataTextColumn { Caption = @"Дата<br>начала", FieldName = $"Data2.V{point.MilestoneId}", Width = Unit.Pixel(80) };
                    var pointColumn3 = new GridViewDataTextColumn { Caption = @"Дата<br>оконч.", FieldName = $"Data3.V{point.MilestoneId}", Width = Unit.Pixel(80) };
                    pointColumn1.PropertiesTextEdit.EncodeHtml = false; //Это перенести в стиль
                    pointColumn2.PropertiesTextEdit.EncodeHtml = false;
                    pointColumn3.PropertiesTextEdit.EncodeHtml = false;

                    pointColumn.Columns.Add(pointColumn1);
                    pointColumn.Columns.Add(pointColumn2);
                    pointColumn.Columns.Add(pointColumn3);
                    bandColumn.Columns.Add(pointColumn);
                }

                this.MSGridView.Columns.Add(bandColumn);
            }

        }

        // Подготовка данных для биндинга 
        protected DataTable PrepareForDataBinding(IReadOnlyList<MilestoneReportData> milestoneReportDataList, IEnumerable<MilestoneCategoriesInfo> viewCategoriesInfoList = null)
        {
            if (milestoneReportDataList == null)
            {
                return null;
            }

            var categoriesList = viewCategoriesInfoList?.ToList() ?? this.GetCategoriesFromReportData(milestoneReportDataList); //Если категория задана то используем ее, иначе берем категории из данных для отчета

            var dt = new DataTable();
            // Создаем структуры таблицы данных
            dt.Columns.Add(new DataColumn("Prd")); // Добавляем фиксированные колонки фиксированные колонки
            dt.Columns.Add(new DataColumn("Dog"));
            dt.Columns.Add(new DataColumn("Ct"));
            dt.Columns.Add(new DataColumn("Npr"));
            dt.Columns.Add(new DataColumn("MsProjectInfo.IsExists"));

            categoriesList.ForEach(
                categiries =>
                {
                    categiries.MilestonePointInfoList.ForEach(
                        point =>
                        {
                            dt.Columns.Add(new DataColumn($"Data1.V{point.MilestoneId}")); // Создаем колонки для информации по дате контрольной точке
                            dt.Columns.Add(new DataColumn($"Data2.V{point.MilestoneId}"));
                            dt.Columns.Add(new DataColumn($"Data3.V{point.MilestoneId}"));
                        });
                });

            // Заполняем данными таблицу данных
            foreach (var dogovor in milestoneReportDataList)
            { 
                var dr = dt.NewRow();
                dr["Prd"] = dogovor.Prd; // Данные по фиксированным колонкам
                dr["Dog"] = dogovor.Dog;
                dr["Ct"] = dogovor.Ct;
                dr["Npr"] = dogovor.Npr;
                dr["MsProjectInfo.IsExists"] = dogovor.MsProjectInfo.IsExists;
                //Проход по всем категориям которые указаны при выборе отображаемых категорий
                dogovor.MilestoneCategiriesInfoList.Where(c => categoriesList.Select(s => s.CatId).Contains(c.CatId)).ToList().ForEach(
                categiries =>
                    {
                        //Для каждой контрольной точки категории заполняем столбцы с датами по контрольной точке
                        categiries.MilestonePointInfoList.ForEach(
                        point =>
                        {
                            dr[$"Data1.V{point.MilestoneId}"] = this.WrapToHtmlTable(point.MilestoneDateInfoList.Select(c => c.ControlPointType)); //Сразу обворачиваем это в HTML-таблицу
                            dr[$"Data2.V{point.MilestoneId}"] = this.WrapToHtmlTable(point.MilestoneDateInfoList.Select(c => c.TaskStartDate));
                            dr[$"Data3.V{point.MilestoneId}"] = this.WrapToHtmlTable(point.MilestoneDateInfoList.Select(c => c.TaskFinishDate));
                        });
                    });

                dt.Rows.Add(dr);
            }

            return dt;
        }
        
        //Получение категорий из из списка договоров
        protected List<MilestoneCategoriesInfo> GetCategoriesFromReportData(IReadOnlyList<MilestoneReportData> milestoneReportDataList)
        {
            var categoriesList = new List<MilestoneCategoriesInfo>();
            // Проход по всем категориям списка договором
            foreach (var categories in milestoneReportDataList.SelectMany(dogovor => dogovor.MilestoneCategiriesInfoList))
            {
                if (categoriesList.All(c => c.CatId != categories.CatId)) // Если в выходном списке такая категория отсутствует
                {
                    categoriesList.Add(new MilestoneCategoriesInfo // То создаем ее и сразу добавляем в список
                    {
                        CatId = categories.CatId,
                        CatName = categories.CatName,
                        CatOrder = categories.CatOrder,
                        MilestonePointInfoList = new List<MilestonePointInfo>()
                    });
                }

                var targetCategories = categoriesList.First(c => c.CatId == categories.CatId); // Выбираем из выходного списка категорий
                // Проход по списку контрольных точек, которые есть в категории договора, но нет в выходном списке контрольных точек
                foreach (var point in categories.MilestonePointInfoList.Where(point => targetCategories.MilestonePointInfoList.All(c => c.MilestoneId != point.MilestoneId)))
                {
                    targetCategories.MilestonePointInfoList.Add(new MilestonePointInfo // Cоздаем ее и добавляем в список контрольных точек
                                                                    { MilestoneId = point.MilestoneId, MilestoneName = point.MilestoneName, MilestoneOrder = point.MilestoneOrder });
                }
            }

            return categoriesList;
        }

        //Представление списка строк как HTML-таблицы
        protected string WrapToHtmlTable(IEnumerable<string> instr)
        {
            var enumerable = instr as string[] ?? instr.ToArray();
            if ((instr == null) || !enumerable.Any())
            {
                return string.Empty;
            }

            var strList = enumerable.ToList();

            var x = strList.Take(strList.Count - 1).Aggregate("<table height = \"100%\" width = \"100%\">", 
                                                               (current, str) => current + $"<tr style=\"border-bottom: 1px solid lightgray\"><td>{str}</td></tr>");
            x += $"<tr><td>{strList.Skip(strList.Count - 1).First()}</td></tr>" + "</table >";
            return x;
        }

        // Создание фиксированного набора колонок
        protected void CreateFixedColumn()
        {
            MilestoneReportData tmp;

            var column = new GridViewDataTextColumn { Caption = @" ", Name = nameof(tmp.Prd), FieldName = nameof(tmp.Prd), Width = Unit.Pixel(40) };
            this.MSGridView.Columns.Add(column);

            column = new GridViewDataTextColumn { Caption = @"№", Name = nameof(tmp.Dog), FieldName = nameof(tmp.Dog), Width = Unit.Pixel(50) };
            this.MSGridView.Columns.Add(column);

            column = new GridViewDataTextColumn { Caption = @"Ст.", Name = nameof(tmp.Ct), FieldName = nameof(tmp.Ct), Width = Unit.Pixel(50) };
            this.MSGridView.Columns.Add(column);

            column = new GridViewDataTextColumn { Caption = @"Наменование", Name = nameof(tmp.Npr), FieldName = nameof(tmp.Npr), Width = Unit.Pixel(300) };
            this.MSGridView.Columns.Add(column);

            column = new GridViewDataTextColumn { Caption = @"График в<br>MSProject", Name = $"{nameof(tmp.MsProjectInfo)}.{nameof(tmp.MsProjectInfo.IsExists)}", FieldName = $"{nameof(tmp.MsProjectInfo)}.{nameof(tmp.MsProjectInfo.IsExists)}", Width = Unit.Pixel(80) };
            this.MSGridView.Columns.Add(column);
        }
    }
}