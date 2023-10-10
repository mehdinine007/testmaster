using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using ReportManagement.Application.Contracts.ReportManagement.IServices;
using ReportManagement.Application.Contracts.WorkFlowManagement.Constants;
using ReportManagement.Domain.ReportManagement;
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
        public WidgetService(IRepository<Widget, int> widgetRepository)
        {
            _widgetRepository = widgetRepository;
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

        public async Task<ChartDto> GetChart(int widgetId)
        {
        var widget=  await Validation(widgetId,null);


            var chartDto = new ChartDto()
            {
                 Id= widget.Id,
                 Title= widget.Title,
                 Type= widget.Type,  
                 Categories = JsonConvert.DeserializeObject<List<CategoryData>>(widget.Fields),
                 Series = JsonConvert.DeserializeObject<List<ChartSeriesData>>(widget.Fields)

            };
            return chartDto;
            
        }

        public async Task<List<WidgetDto>> GetList(int dashboardId)
        {
            var widget = (await _widgetRepository.GetQueryableAsync()).ToList();
            var widgetDto = ObjectMapper.Map<List<Widget>, List<WidgetDto>>(widget);
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
            var widgetQuery = await _widgetRepository.GetQueryableAsync();
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
