using LinkedIn_XML.Helpers;
using LinkedIn_XML.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;

namespace LinkedIn_XML.Controllers;

[ApiController]
[Route("[controller]")]
public class BlobsController : ControllerBase
{
    private readonly IBlobDataFileService _blobDataFileService;
    private readonly IDataFileService _dataFileService;

    public BlobsController(IBlobDataFileService blobDataService, IDataFileService dataFileService)
    {
        _blobDataFileService = blobDataService;
        _dataFileService = dataFileService;
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup(CancellationToken cancellationToken = default)
    {
        var doc = XMLHelper.GetXMLDocument(_dataFileService.FilePath);
        var bookId = 102;
        
        var idFoundByLinq = TraverseXmlByLinq(_dataFileService.FilePath, bookId);
        Console.WriteLine($"Book ({bookId}) " + (idFoundByLinq ? "found." : "not found."));

        var idFoundByXPath = TraverseXmlByXPath(_dataFileService.FilePath, bookId);
        Console.WriteLine($"Book ({bookId}) " + (idFoundByXPath ? "found." : "not found."));

        return Ok();
    }

    private bool TraverseXmlByLinq(string xmldataFilePath, int idToFind)
    {
        var xDoc = XDocument.Load(xmldataFilePath);

        return xDoc.Root?.Elements("book")?
            .Any(a => a.Attribute("id")?.Value == idToFind.ToString())
            ?? false;
    }

    private bool TraverseXmlByXPath(string xmldataFilePath, int idToFind)
    {
        string xpath = $"/catalog/book[@id='{idToFind}']";
        var xDoc = XDocument.Load(xmldataFilePath);

        return xDoc.XPathSelectElement(xpath) is not null;
    }
}
