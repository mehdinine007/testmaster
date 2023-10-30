
using Google.Protobuf.Collections;
using Grpc.Core;
using NPOI.XSSF.UserModel.Helpers;
using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using ReportManagement.Application.Contracts.ReportManagement.IServices;
using ReportManagement.Application.Grpc.ReportServiceGrpc;
using ReportManagement.Domain.Shared.ReportManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ReportManagement.Application.ReportManagement.Implementations
{
    public class GrpcReportService : ReportServiceGrpc.ReportServiceGrpcBase
    {

        private readonly IWidgetService _widgetService;
        private readonly IDashboardService _dashboardService;

        public GrpcReportService(IWidgetService widgetService, IDashboardService dashboardService)
        {
            _widgetService = widgetService;
            _dashboardService = dashboardService;
        }


        public async Task<List<DashboardModel>> GetAllDashboard()
        {
            var dashboardsDto = await _dashboardService.GetList();
            var dashboards = dashboardsDto.Select(x => new DashboardModel()
            {
                Id = x.Id,
                Priority = x.Priority,
                Title = x.Title,

            }).ToList();
            return dashboards;
        }

        public async Task<List<WidgetModel>> GetWidgetByDashboardId(WidgetRequestModel request)
        {
            var widgetDtos = await _widgetService.GetList(request.DashboardId);
            var widgets = widgetDtos.Select(x => new WidgetModel()
            {
                Id = x.Id,
                Title = x.Title,
                Condition = { x.Condition.Select(y =>new ConditionModel()
                {
                     Priority= y.Priority,
                     Title= y.Title,
                     Default=y.Default,
                     Name= y.Name,
                     MultiSelect = y.MultiSelect,
                     Type=(Grpc.ReportServiceGrpc.WidgetTypeEnum)y.Type,
                     DrowpDownItems ={ y.DrowpDownItems!=null ? y.DrowpDownItems.Select(z => new Grpc.ReportServiceGrpc.DrowpDownItem()
                      {
                         Value = z.Value,
                         Title = z.Title
                       }).ToList():null},
                     ApiContent = y.ApiContent!=null ? new Grpc.ReportServiceGrpc.ApiContent()
                      {
                         Body=y.ApiContent.Body,
                         Url=y.ApiContent.Url,
                         Type=(Grpc.ReportServiceGrpc.ApiContent.Types.ApiType)y.ApiContent.Type,
                      } :null

                })}
            }).ToList();
            return widgets;
        }

        public async Task<ChartModel> GetChart(ChartRequestModel request)
        {
            var conditionValues = request.ConditionValue.Select(x => new ConditionValue()
            {
                MultiSelect = x.MultiSelect,
                Name = x.Name,
                Value = x.Value,
                Type = (Domain.Shared.ReportManagement.Enums.ConditionTypeEnum)x.Type,
            }).ToList();

            var chartDto = await _widgetService.GetChart(request.WidgetId, conditionValues);
            var chartModel = new ChartModel()
            {
                Id = chartDto.Id,
                Title = chartDto.Title,
                Categories = { chartDto.Categories.Select(x => new CategoryData
                {
                    Title = x.Title,
                    Color = x.Color,
                })
                },
                Series = { chartDto.Series.Select(x => new ChartSeriesData
                {
                    Color = x.Color,
                    Name = x.Name,
                    Data = {x.Data },

                }),
                }
            };
            return chartModel;
        }

        public async Task<GridModel> GetGrid(ChartRequestModel request)
        {
            var conditionValues = request.ConditionValue.Select(x => new ConditionValue()
            {
                MultiSelect = x.MultiSelect,
                Name = x.Name,
                Value = x.Value,
                Type = (Domain.Shared.ReportManagement.Enums.ConditionTypeEnum)x.Type,
            }).ToList();

            var gridDto = await _widgetService.GetGrid(request.WidgetId, conditionValues);
            var MyDictionary = gridDto.Data.SelectMany<Dictionary<string, object>, KeyValue>(dict =>
          dict.Select<KeyValuePair<string, object>, KeyValue>(pair => new KeyValue
          {
              Key = pair.Key,
              Value = pair.Value as Google.Protobuf.WellKnownTypes.Any
          })).ToList();
            var chartModel = new GridModel()
            {
                Id = gridDto.Id,
                Title = gridDto.Title,
                Categories = { gridDto.Categories.Select(x => new CategoryData
                {
                    Title = x.Title,
                    Color = x.Color,
                })
                },
                Data = { MyDictionary.Select(x=>new KeyValue
                {
                    Key=x.Key,
                    Value=x.Value,
                })
                }
            };
            return chartModel;
        }


    }

}

