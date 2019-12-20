using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReceiptScanner.Models;

namespace ReceiptScanner
{

    public class HomeController : Controller
    {
        // private ReceiptsRecognitionContext dbContext;
        // public HomeController(ReceiptsRecognitionContext context)
        // {
        //     dbContext = context;
        // }

        [HttpGet("")]
        public ViewResult Index()
        {
            return View("Index");
        }
        
        [HttpPost("recognize")]
        public IActionResult Recognize(string filePath)
        {
            Recognizer r = new Recognizer();
            List<ItemToView> ResultList = r.recognizeIt(filePath);
            // Console.WriteLine("***** List of items ******");
            // foreach(ItemToView i in ResultList)
            // {
            //     Console.WriteLine($"Item name: {i.ItemName}, item price: {i.ItemPrice}");
            // }
            return View("Result", ResultList);
        }
        // [HttpGet("result")]
        // public IActionResult Result(List<ItemToView> ResultList)
        // {  
        //     return View("Result");
        // }
    }
}