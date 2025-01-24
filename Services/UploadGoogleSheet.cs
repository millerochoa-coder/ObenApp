using Google.Apis.Sheets.v4;
using ObenApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObenApp.Services
{
    public class UploadGoogleSheet : IUploadGoogleSheet
    {
        public string SpreadsheetId { get; set; }
        public string SheetName { get; set; }
        public SheetsService sheetsService { get; set; }

        public void Initialize()
        {
            IUploadGoogleSheet ToGoogleSheet = new UploadGoogleSheet();
            ToGoogleSheet.SpreadsheetId = SpreadsheetId;
            ToGoogleSheet.SheetName = SheetName;
            ToGoogleSheet.sheetsService = sheetsService;

            ToGoogleSheet.InitializeUploadGoogleSheet();
        }
    }
}
