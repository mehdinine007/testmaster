using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using AdminPanelManagement.Application.Grpc.ReportGrpcClient;
using Google.Protobuf.Collections;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CategoryData = AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report.CategoryData;
using ChartSeriesData = AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report.ChartSeriesData;

namespace AdminPanelManagement.Application.AdminPanelManagement.Grpc
{
    public class ReportGrpcClientService : ApplicationService, IReportGrpcClientService
    {
        private IConfiguration _configuration { get; set; }
        public ReportGrpcClientService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<DashboardDto>> GetAllDashboard()

        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler();

            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:ReportUrl"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new ReportServiceGrpc.ReportServiceGrpcClient(channel);
            var result = client.GetAllDashboard(new DashboardRequestModel() { });
            var dashboardDto = result.DashboardModel.Select(x => new DashboardDto
            {
                Id = x.Id,
                Priority = x.Priority,
                Title = x.Title,
            }).ToList();

            return dashboardDto;

        }
        public async Task<List<WidgetDto>> GetWidgetByDashboardId(int dashboardId)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler();

            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:ReportUrl"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new ReportServiceGrpc.ReportServiceGrpcClient(channel);
            var result = client.GetWidgetByDashboardId(new WidgetRequestModel() { DashboardId = dashboardId });
            var widgetDto = result.WidgetModel.Select(x => new WidgetDto
            {
                Id = x.Id,
                Title = x.Title,
                Condition = x.Condition.Select(y => new ConditionDto
                {
                    Priority = y.Priority,
                    Title = y.Title,
                    Default = y.Default,
                    Name = y.Name,
                    MultiSelect = y.MultiSelect,
                    Type = (Domain.Shared.AdminPanelManagement.Enum.ConditionTypeEnum)y.Type,
                    DrowpDownItems = y.DrowpDownItems != null ? y.DrowpDownItems.Select(z => new Contracts.AdminPanelManagement.Dtos.report.DrowpDownItem
                    {
                        Value = z.Value,
                        Title = z.Title
                    }).ToList() : null,
                    ApiContent = y.ApiContent != null ? new Contracts.AdminPanelManagement.Dtos.report.ApiContent()
                    {
                        Body = y.ApiContent.Body,
                        Url = y.ApiContent.Url,
                        Type = (Contracts.AdminPanelManagement.Dtos.report.ApiCallType)y.ApiContent.Type,
                    } : null
                }).ToList(),
            }).ToList();
            return widgetDto;
        }
        public async Task<ChartDto> GetChart(int widgetId, List<ConditionValue> conditionValue)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var httpHandler = new HttpClientHandler();

            var condition = conditionValue.Select(x => new ConditionValueModel
            {
                MultiSelect = x.MultiSelect,
                Name = x.Name,
                Value = x.Value,
                Type = (ConditionTypeEnum)x.Type
            });
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:ReportUrl"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new ReportServiceGrpc.ReportServiceGrpcClient(channel);
            var result = client.GetChart(new ChartRequestModel() { WidgetId = widgetId, ConditionValue = { condition } });

            var chartDto = new ChartDto
            {
                Id = result.Id,
                Title = result.Title,
                Type = (Domain.Shared.AdminPanelManagement.Enum.WidgetTypeEnum)result.Type,
                Categories = result.Categories.Select(x => new CategoryData
                {
                    Color = x.Color,
                    Title = x.Title,

                }).ToList(),
                Series = result.Series.Select(x => new ChartSeriesData
                {
                    Color = x.Color,
                    Data = x.Data.ToList(),
                    Name = x.Name

                }).ToList(),
            };
            return chartDto;
        }
        public async Task<GridDto> GetGrid(int widgetId, List<ConditionValue> conditionValue)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var httpHandler = new HttpClientHandler();

            var condition = conditionValue.Select(x => new ConditionValueModel
            {
                MultiSelect = x.MultiSelect,
                Name = x.Name,
                Value = x.Value,
                Type = (ConditionTypeEnum)x.Type
            });
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:ReportUrl"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new ReportServiceGrpc.ReportServiceGrpcClient(channel);
            var result = client.GetGrid(new GridRequestModel() { WidgetId = widgetId, ConditionValue = { condition } });

            var gridDto = new GridDto
            {
                Id = result.Id,
                Title = result.Title,
                Categories = result.Categories.Select(x => new CategoryData
                {
                    Color = x.Color,
                    Title = x.Title,

                }).ToList(),
                Data = result.Data
                   .Cast<IDictionary<string, object>>()
                   .Select(x => x.ToDictionary(x => x.Key, x => x.Value)).ToList()
            };
            return gridDto;
        }


        public async Task<string> Test1()
        {

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:ReportUrl"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new ReportServiceGrpc.ReportServiceGrpcClient(channel);
            var result = client.Test1(new TestInput() { });

            return result.Result;

        }

    }
}

