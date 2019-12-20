using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReceiptScanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ReceiptScanner;

namespace ReceiptScanner.Controllers
{
  public class HomeController : Controller
  {
    private IHostingEnvironment _env;
    private string _imgUploadDir;
    private List<byte[]> _imgs_bytes;
    private List<string> _imgs_base64;

    private MyContext db;

    public HomeController(IHostingEnvironment env,
                          MyContext context)
    {
      _env = env;
      // _imgUploadDir = Path.Combine(env.WebRootPath, "uploads");
      _imgUploadDir = "./wwwroot/uploads";
      _imgs_bytes = new List<byte[]>();
      _imgs_base64 = new List<string>();
      db = context;
    }
    public IActionResult Index()
    {
      return View();
    }

    // public async Task<IActionResult> Process(List<IFormFile> files)
    [HttpPost("upload")]
    public async Task<IActionResult> Process(IEnumerable<IFormFile> files)
    {
      long size = files.Sum(f => f.Length);

      List<string> filePaths = new List<string>();
      List<byte[]> imgs_bytes = new List<byte[]>();
      foreach (var file in files)
      {
        if (file.Length > 0 && file.ContentType.Contains("image"))
        {
          // full path to file in temp location
          string datetime = DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss");
          // string imgGuid = new Guid().ToString();
          string filetype = "." + file.ContentType.Split("/")[1];
          // string filename = datetime + imgGuid + filetype;
          string filename = datetime + filetype;
          // var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
          string filePath = $"{_imgUploadDir}/{filename}";
          filePaths.Add(filePath);
          
          string type = file.GetType().ToString();
          
          // save uploads to local files
          using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
          {
            await file.CopyToAsync(stream);
          }
          
          // // save uploads to byte arrays
          // using (MemoryStream stream = new MemoryStream())
          // {
          //   await file.OpenReadStream().CopyToAsync(stream);
          //   imgs_bytes.Add(stream.ToArray());
          // }
        }
      }

      // process uploaded files
      CryptoEngine Encryptor = new CryptoEngine();
      foreach (string path in filePaths){
        string encryptedPath = Encryptor.Encrypt(path);
        Photos img = new Photos {
          PhotoPath = encryptedPath
        };
        db.Add(img);
      }
      db.SaveChanges();
      // Don't rely on or trust the FileName property without validation.

      // return Ok(new { count = files.Count, size, filePaths });

      string[] firstfilepath = filePaths[0].Split("\\");
      int lastsegment = firstfilepath.Length-1;
      string firstfilename = firstfilepath[lastsegment];
      Recognizer r = new Recognizer();
      List<ItemToView> ResultList = r.recognizeIt($"./wwwroot/uploads/{firstfilename}");
      return View("Result", ResultList);
      // return RedirectToAction("Result", new { resultList = ResultList} );
      // return RedirectToAction("Index", new {obj = imgs_bytes});
      // return View("Index", imgs_bytes);
    }

    
    [HttpGet("result")]
    public IActionResult Result(List<ItemToView> resultList)
    {
      return View(resultList);
    }

    public IActionResult UploadedImages(List<byte[]> imgs_bytes)
    {
      // if (imgs_bytes.Count == 0)
      // {
      //   return View();
      // }
      List<string> base64imgs = new List<string>();
      imgs_bytes = (List<byte[]>)TempData["imgs"];
      foreach (byte[] imgbytes in imgs_bytes)
      {
        base64imgs.Add(Convert.ToBase64String(imgbytes));
      }
      return View(base64imgs);
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
