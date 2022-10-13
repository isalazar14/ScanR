using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ReceiptScanner.Models
{
  public class RecResultToView
  {
    public string ImgPath {get; set;}
    public List<ItemToView> ItemsList { get; set; }
    public string StoreName { get; set; }
    public DateTime Date { get; set; }
  }
}