using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Security.Cryptography;  
using System.Text;  
using System.IO;
namespace ReceiptScanner.Models{

    public class Photos{
        [Key]
        public int PhotoId{get;set;}
        [Required]
        public string PhotoPath{get;set;}
        [MaxLength(255)]
        public string Desc{get;set;}
        // [Required]
        // public int CreatorId{get;set;}
        // public Users Creator{get;set;}
        

    }
}