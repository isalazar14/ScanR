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

    // private MyContext db;

    public HomeController(IHostingEnvironment env
                          /* MyContext context */)
    {
      _env = env;
      _imgUploadDir = "./wwwroot/uploads";
      _imgs_bytes = new List<byte[]>();
      _imgs_base64 = new List<string>();
      // db = context;
    }
    public IActionResult Index()
    {
      return View();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Process(IFormFile file)
    {
      if (file.Length > 0 && file.ContentType.Contains("image"))
      {
        // full path to file in temp location
        string datetime = DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss");
        string filetype = "." + file.ContentType.Split("/")[1];
        string filename = datetime + filetype;
        string filePath = $"{_imgUploadDir}/{filename}";
        
        
        // save uploads to local files
        using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
          await file.CopyToAsync(stream);
        }
          
      // process uploaded files
        // CryptoEngine Encryptor = new CryptoEngine();
        // foreach (string path in filePaths){
        //   string encryptedPath = Encryptor.Encrypt(path);
        //   Photos img = new Photos {
        //     PhotoPath = encryptedPath
        //   };
        //   db.Add(img);
        // }
        // db.SaveChanges();

      // run vision service
        Recognizer r = new Recognizer();
        List<ItemToView> results = r.recognizeIt($"./wwwroot/uploads/{filename}");
        ResultsViewModel resultsModel = new ResultsViewModel
        {
          ImgPath = filename,
          Results = results
        };
        return View("Result", resultsModel);
        // return RedirectToAction("Result", new { resultList = ResultList} );
        // return RedirectToAction("Index", new {obj = imgs_bytes});
        // return View("Index", imgs_bytes);
        }
        else 
        {
          return RedirectToAction("Index");
        }
      }

    
    [HttpGet("result")]
    public IActionResult Result(ResultsViewModel ResultsModel)
    {
      return View(ResultsModel);
    }

    public IActionResult UploadedImages(List<byte[]> imgs_bytes)
    {
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
