
using Google.Protobuf.Collections;
using Grpc.Core;
using JetBrains.Annotations;
using NPOI.SS.Formula.Functions;
using NPOI.XSSF.UserModel.Helpers;
using ReportManagement.Application.Contracts.ReportManagement.Dtos;
using ReportManagement.Application.Contracts.ReportManagement.IServices;
using ReportManagement.Application.Grpc.ReportService;
using ReportManagement.Domain.Shared.ReportManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using static ReportManagement.Application.Grpc.ReportService.ApiContent.Types;

namespace ReportManagement.Application.ReportManagement.Grpc
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


 

        public override async Task<DashboardModelList> GetAllDashboard(DashboardRequestModel request, ServerCallContext context)
        {
            var dashboardsDto = await _dashboardService.GetList();
            var dashboarList = new DashboardModelList();
            dashboardsDto.ForEach(x =>
            {
                var DashboardModel = new DashboardModel
                {

                    Id = x.Id,
                    Priority = x.Priority,
                    Title = x.Title,
                };
                dashboarList.DashboardModel.Add(DashboardModel);
            });
            return dashboarList;
        }
        public override async Task<WidgetModelList> GetWidgetByDashboardId(WidgetRequestModel request, ServerCallContext context)
        {
            var widgetModelList = new WidgetModelList();

            var widgetDtos = await _widgetService.GetList(request.DashboardId);
            widgetDtos.ForEach(x =>
            {
                var widgetModel = new WidgetModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Condition ={x.Condition.Select(y =>new ConditionModel()
                {
                     Priority= y.Priority,
                     Title= y.Title,
                     Default=y.Default,
                     Name= y.Name,
                     MultiSelect = y.MultiSelect,
                     Type=(Application.Grpc.ReportService.WidgetTypeEnum)y.Type,
                     DrowpDownItems ={ y.DrowpDownItems!=null ? y.DrowpDownItems.Select(z => new Application.Grpc.ReportService.DrowpDownItem()
                      {
                         Value = z.Value,
                         Title = z.Title
                       }).ToList():null},
                     ApiContent = y.ApiContent!=null ? new Application.Grpc.ReportService.ApiContent()
                      {
                         Body=y.ApiContent.Body,
                         Url=y.ApiContent.Url,
                         Type=(ApiType)y.ApiContent.Type,
                      } :null

                }) }
                };
                widgetModelList.WidgetModel.Add(widgetModel);
            });
            return widgetModelList;
        }
        public override async Task<ChartModel> GetChart(ChartRequestModel request, ServerCallContext context)
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
                Type=(Application.Grpc.ReportService.WidgetTypeEnum)chartDto.Type,
                Categories = { chartDto.Categories.Select(x => new CategoryData
                {
                    Title = x.Title,
                    Color=x.Color,
                })
                },
                Series = { chartDto.Series.Select(x => new ChartSeriesData
                {
                    Data = {x.Data },
                     Color=x.Color,
                      Name=x.Name,

                }),
                }
            };
            return chartModel;
        }
        public override async Task<GridModel> GetGrid(GridRequestModel request, ServerCallContext context)
        {
            var conditionValues = request.ConditionValue.Select(x => new ConditionValue()
            {
                MultiSelect = x.MultiSelect,
                Name = x.Name,
                Value = x.Value,
                Type = (Domain.Shared.ReportManagement.Enums.ConditionTypeEnum)x.Type,
            }).ToList();

            var gridDto = await _widgetService.GetGrid(request.WidgetId, conditionValues);
            var MyDictionary = gridDto.Data.SelectMany(dict =>
          dict.Select(pair => new KeyValue
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

        public override async Task<Test> TestNullable(TestInput request, ServerCallContext context)
        {
            var timestamp = new Google.Protobuf.WellKnownTypes.Timestamp
            {
                Seconds = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Nanos = DateTimeOffset.Now.Millisecond * 1000000
            };
            var test = new Test()
            {
                //Result1 = "salam",
                //Result2 = 2,
                //MyTimestamp = timestamp
            };
            return test;
           
        }
    }

}

