using Newtonsoft.Json;

namespace Web.Models.ProductInstall;

public class ProductInstallAttachmentRemoveParam
{
    [JsonProperty("attachmentId")]
    public long AttachmentId { get; set; }
}