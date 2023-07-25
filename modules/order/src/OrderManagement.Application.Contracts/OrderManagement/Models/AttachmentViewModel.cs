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
        public AttachmentEntityTypeEnum Type { get; set; }
        public string TypeTitle { get; set; }
        public string Description { get; set; }
        public List<string> Content { get; set; }
        public int Priority { get; set; }
    }
}
