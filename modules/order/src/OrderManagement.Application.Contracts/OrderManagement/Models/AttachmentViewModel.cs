using AutoMapper;
using Esale.Core.Utility.Tools;
using Newtonsoft.Json;
using OrderManagement.Domain.Shared;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OrderManagement.Application.Contracts
{
    public class AttachmentViewModel
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public string EntityTypeTitle { get; set; }
    }
}
