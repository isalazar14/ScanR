using System.Collections.Generic;
using RestSharp;

namespace ReceiptScanner.Models
{
  public class Category
  {
    public string code {set;get;}
    public string label { set; get; }
    public int abs_relevance { set; get; }
    public int relevance { set; get; }
  }

  public class Status
  {
    public int code { set; get; }
    public string msg { set; get; }
    public int credits { set; get; }
    public int remaining_credits { set; get; }
  }

  class outPut
  {
    public Status status { set; get; }
    public List<Category> category_list { set; get; }
  }

  public class ItemToCategory
  {
    public string ItemName;
    public string CategoryName;
  }

  class Program
  {
    string key = "5e4192a2552b72b4a12bb83be19bff1e";

    public static IRestResponse Categorize(List<string> input)
    {
      RestClient MeaningClient = new RestClient("https://api.meaningcloud.com/class-1.1");

      // var accessKey = "5e4192a2552b72b4a12bb83be19bff1e";
      string inputstring = string.Join("%20",input);
      string test = "key=5e4192a2552b72b4a12bb83be19bff1e&of=json&txt="+inputstring+".&model=IAB_en";
      System.Console.WriteLine(test);

      var request = new RestRequest(Method.POST);
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      request.AddParameter("application/x-www-form-urlencoded",test, ParameterType.RequestBody);
      IRestResponse response = MeaningClient.Execute(request);
      return response;
    }
  }
}