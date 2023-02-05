namespace Clever.Firmware.Contracts.FirmwarePackages;

public class FileContent
{
    public string FileName { get; set; }

    public Stream Content { get; set; }

    public string ContentType { get; set; }

    public FileContent(string fileName, Stream content, string contentType)
    {
        FileName = fileName;
        Content = content;
        ContentType = contentType;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Reliability",
        "CA2000:Dispose objects before losing scope",
        Justification = "Disposed by parent")]
    public HttpContent ToHttpContent()
        => new MultipartFormDataContent
            {
                {
                    new StreamContent(this.Content),
                    "file",
                    this.FileName
                },
            };
}
