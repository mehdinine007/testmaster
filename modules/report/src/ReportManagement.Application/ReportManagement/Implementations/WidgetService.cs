using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using ReportManagement.Application.Contracts.ReportManagement.IServices;
using ReportManagement.Application.Contracts.WorkFlowManagement.Constants;
using ReportManagement.Domain.ReportManagement;
using ReportManagement.Domain.Shared.ReportManagement.Dtos;
using ReportManagement.Domain.Shared.ReportManagement.Enums;
using ReportManagement.EntityFrameworkCore.ReportManagement.EntityFrameworkCore.Repositories;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace ReportManagement.Application.ReportManagement.Implementations
{
    public class WidgetService : ApplicationService, IWidgetService
    {
        private readonly IRepository<Widget, int> _widgetRepository;
        private readonly IRepository<DashboardWidget, int> _dashboardWidgetRepository;
        private readonly IWidgetRepository _repository;
       


        public WidgetService(IRepository<Widget, int> widgetRepository, IWidgetRepository repository
            , IRepository<DashboardWidget, int> dashboardWidgetRepository)
        {
            _widgetRepository = widgetRepository;
            _repository = repository;
            _dashboardWidgetRepository = dashboardWidgetRepository;
        }


        public async Task<WidgetDto> Add(WidgetCreateOrUpdateDto widgetCreateOrUpdateDto)
        {
            var widget = ObjectMapper.Map<WidgetCreateOrUpdateDto, Widget>(widgetCreateOrUpdateDto);
            var entity = await _widgetRepository.InsertAsync(widget, autoSave: true);
            return ObjectMapper.Map<Widget, WidgetDto>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var widget = await Validation(id, null);
            await _widgetRepository.DeleteAsync(id);
            return true;
        }

        public async Task<WidgetDto> GetById(int id)
        {
            var widget = await Validation(id, null);
            var widgetDto = ObjectMapper.Map<Widget, WidgetDto>(widget);
            return widgetDto;
        }

        public async Task<ChartDto> GetChart(int widgetId, List<ConditionValue> conditionValue)
        {
            var widget = await Validation(widgetId, null);
            var categoryDatas = JsonConvert.DeserializeObject<List<CategoryData>>(widget.Fields);

            var conditionDec = new List<string>();
            foreach (var condition in conditionValue)
            {
                string declare = "@" + condition.Name;
                string type = "";
                string value = condition.Value;
                switch (condition.Type)
                {
                    case ConditionTypeEnum.String:
                        type = "nvarchar(max)";
                        value = "'"+condition.Value + "'";
                        break;
                    case ConditionTypeEnum.Numerical:
                        type = "int";
                        break;
                    case ConditionTypeEnum.DropDown:
                        type = !condition.MultiSelect ? "int" : "nvarchar(max)";
                        break;
                    case ConditionTypeEnum.CodingApi:
                        type = !condition.MultiSelect ? "int" : "nvarchar(max)";
                        break;
                    case ConditionTypeEnum.Date:
                        type = "datetime";
                        break;
                    default:
                        type = "nvarchar(max)";
                        break;
                }
                if (condition.Type == ConditionTypeEnum.DropDown || condition.Type == ConditionTypeEnum.CodingApi)
                    value = !condition.MultiSelect ? condition.Value : "'" + condition.Value + "'";
                if (string.IsNullOrEmpty(value))
                    value = "NULL";
                declare += " as " + type + " = " + value;
                conditionDec.Add(declare);
            }
            string _command = widget.Command;
            if (conditionDec.Count > 0)
                _command = " declare " + string.Join(",", conditionDec) + " " + widget.Command;
            var result = _repository.GetReportData(_command);
            List<ChartSeriesData> SeriDataList = new List<ChartSeriesData>();
            ChartSeriesData seriesData = new ChartSeriesData();
            seriesData.Data = result.SelectMany(x => x.Values).Cast<int>().ToList();

            SeriDataList.Add(seriesData);
            var chartDto = new ChartDto()
            {
                Id = widget.Id,
                Title = widget.Title,
                Type = widget.Type,
                Categories = JsonConvert.DeserializeObject<List<CategoryData>>(widget.Fields),
                Series = SeriDataList

            };
            return chartDto;

        }

        public async Task<List<DashboardWidgetDto>> GetList(int dashboardId)
        {
            var widgetQuery = (await _dashboardWidgetRepository.GetQueryableAsync());
            var widgets= widgetQuery.Include(x => x.Widget).Where(x=>x.DashboardId == dashboardId).ToList();    
            var widgetDto = ObjectMapper.Map<List<DashboardWidget>, List<DashboardWidgetDto>>(widgets);
            return widgetDto;
        }

        public async Task<WidgetDto> Update(WidgetCreateOrUpdateDto widgetCreateOrUpdateDto)
        {
            var widget = await Validation(widgetCreateOrUpdateDto.Id, widgetCreateOrUpdateDto);
            widget.Title = widgetCreateOrUpdateDto.Title;
            widget.Command = widgetCreateOrUpdateDto.Command;
            widget.Fields = JsonConvert.SerializeObject(widgetCreateOrUpdateDto.Fields);
            widget.Condition = JsonConvert.SerializeObject(widgetCreateOrUpdateDto.Condition);
            widget.Type = widgetCreateOrUpdateDto.Type;
            var entity = await _widgetRepository.UpdateAsync(widget);
            return ObjectMapper.Map<Widget, WidgetDto>(entity);
        }

        private async Task<Widget> Validation(int? id, WidgetCreateOrUpdateDto widgetCreateOrUpdateDto)
        {
            var widgetQuery = (await _widgetRepository.GetQueryableAsync());
            Widget widget = new Widget();
            if (id is not null)
            {
                widget = widgetQuery.FirstOrDefault(x => x.Id == id);
                if (widget is null)
                {
                    throw new UserFriendlyException(ReportConstant.WidgetNotFound, ReportConstant.WidgetNotFoundId);
                }
            }

            return widget;
        }



    }
}
