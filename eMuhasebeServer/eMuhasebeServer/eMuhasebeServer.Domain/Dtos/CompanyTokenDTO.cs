using System.Text.Json.Serialization;

namespace eMuhasebeServer.Domain.Dtos
{
    public sealed class CompanyTokenDTO
    {
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("id")]
        public Guid CompanyId { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; } = string.Empty;

        [JsonPropertyName("taxDepartment")]
        public string TaxDepartment { get; set; } = string.Empty;

        [JsonPropertyName("taxNumber")]
        public string TaxNumber { get; set; } = string.Empty;

        [JsonPropertyName("fullAddress")]
        public string FullAddress { get; set; } = string.Empty;

    }
}