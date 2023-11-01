using IFG.Core.Utility.Results;
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
using Volo.Abp.Domain.Entities;
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

      

        public async Task<List<WidgetDto>> GetList(int dashboardId)
        {
            var widgets = (await _dashboardWidgetRepository.GetQueryableAsync())
                .AsNoTracking()
                .Include(i => i.Widget)
                .Where(x => x.DashboardId == dashboardId)
                .Select(x => x.Widget)
                .ToList();
            var widgetDto = ObjectMapper.Map<List<Widget>, List<WidgetDto>>(widgets);
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

        public async Task<ChartDto> GetChart(int widgetId, List<ConditionValue> conditionValue)
        {
            var categories = new List<CategoryData>();
            var widget = await Validation(widgetId, null);
            var _getdata = await GetData(widget.Command, await CreateCondition(conditionValue));


            var seriDataList = new List<ChartSeriesData>();
            if (widget.OutPutType == OutPutTypeEnum.Column)
            {
                var data = _getdata
                    .Cast<IDictionary<string, object>>()
                    .Select(x => x.ToDictionary(x => x.Key, x => x.Value)).FirstOrDefault();
                if (data != null)
                {
                    categories = data.Keys.Select(x => new CategoryData()
                    {
                        Title = x
                    }).ToList();
                    seriDataList.Add(new ChartSeriesData()
                    {
                        Data = data.Select(x => long.Parse(x.Value.ToString()!)).ToList()
                    });
                }

            }
            if (widget.OutPutType == OutPutTypeEnum.Row)
            {
                var data = _getdata
                    .Cast<IDictionary<string, object>>()
                    .Select(x => x.ToDictionary(x => x.Key, x => x.Value)).ToList();
                if (data.Count > 0)
                {
                    var keys = data.FirstOrDefault()!.Keys.Select(x => x)
                        .ToList();
                    string _category = keys[0];
                    string _value = keys[1];
                    categories = data.Select(x => new CategoryData()
                    {
                        Title = x[_category].ToString()!
                    }).ToList();

                    seriDataList.Add(new ChartSeriesData()
                    {
                        Data = data.Select(x => long.Parse(x[_value].ToString()!)).ToList()
                    });
                }
            }
            var chartDto = new ChartDto()
            {
                Id = widget.Id,
                Title = widget.Title,
                Type = widget.Type,
                Categories = categories,
                Series = seriDataList

            };
            return chartDto;

        }
        public async Task<GridDto> GetGrid(int widgetId, List<ConditionValue> conditionValue)
        {
            var widget = await Validation(widgetId, null);
            var categories = new List<CategoryData>();
            var _getdata = await GetData(widget.Command ,await CreateCondition(conditionValue));

            var data = _getdata
                   .Cast<IDictionary<string, object>>()
                   .Select(x => x.ToDictionary(x => x.Key, x => x.Value)).ToList();

            if (data.Count > 0)
            {
                var keys = data.FirstOrDefault()!.Keys.Select(x => x)
                    .ToList();
                string _category = keys[0];
                categories = data.Select(x => new CategoryData()
                {
                    Title = x[_category].ToString()!
                }).ToList();
            }
            var gridDto = new GridDto()
            {
                Id = widget.Id,
                Title = widget.Title,
                Categories = categories,
                Data = data

            };
            return gridDto;
        }


        private async Task<string> CreateCondition(List<ConditionValue> conditionValue)
        {
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
                        if (!string.IsNullOrWhiteSpace(condition.Value))
                            value = "'" + condition.Value + "'";
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
                if (!string.IsNullOrWhiteSpace(value))
                    if (condition.Type == ConditionTypeEnum.DropDown || condition.Type == ConditionTypeEnum.CodingApi)
                        value = !condition.MultiSelect ? condition.Value : "'" + condition.Value + "'";
                if (string.IsNullOrEmpty(value))
                    value = "NULL";
                declare += " as " + type + " = " + value;
                conditionDec.Add(declare);

            }
            string Result = "";
            if (conditionDec.Count > 0)
                Result = " declare " + string.Join(",", conditionDec);
            return Result;
        }

        private async Task<IEnumerable<dynamic>> GetData(string command, string condition)
        {
            string _command = condition+ " " +command;
            var _getdata = _repository.GetReportData(_command);
            return _getdata;
        }
    }
}
