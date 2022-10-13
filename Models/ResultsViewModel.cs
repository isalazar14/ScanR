using System.Collections.Generic;

namespace ReceiptScanner.Models
{
  public class ResultsViewModel
  {
    public string ImgPath {get; set;}

    public List<ItemToView> Results {get; set;}
  }
}