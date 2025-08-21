using Json.More;
using LinkedIn_XML.Services;
using Microsoft.AspNetCore.Mvc;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace LinkedIn_XML.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamingsController : ControllerBase
{
    private readonly IBlobDataFileService _blobDataFileService;

    public StreamingsController(IBlobDataFileService blobDataFileService)
    {
        _blobDataFileService = blobDataFileService;
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup(CancellationToken cancellationToken = default)
    {
        var filePath = _blobDataFileService.FilePath;

        var bookName = "Et consequuntur quas esse.";
        var idFound = TraverseXmlByReader(_blobDataFileService.FilePath, bookName);
        Console.WriteLine($"Book ({bookName}) " + (idFound ? "found." : "not found."));

        return Ok();
    }

    private bool TraverseXmlByReader(string xmldataFilePath, string nameToFind)
    {
        using var reader = XmlReader.Create(xmldataFilePath);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "book")
            {
                reader.ReadToDescendant("title");
                var title = reader.ReadElementContentAsString();
                if (!string.IsNullOrWhiteSpace(title) && 
                    title.Equals(nameToFind, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
        }
        return false;
    }

}
