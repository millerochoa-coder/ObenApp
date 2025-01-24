using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObenApp.Interfaces
{
    public interface IUploadGoogleSheet
    {
        string SpreadsheetId { get; set; }
        string SheetName { get; set; }
        SheetsService sheetsService { get; set; }

        void InitializeUploadGoogleSheet()
        {
            string[] Scopes = { SheetsService.Scope.Spreadsheets };
            string ApplicationName = "";

            // Ruta al archivo de credenciales JSON
            var credentialPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "credenciales.json");

            GoogleCredential credential;
            using(var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        async Task WriteDataAsync(string[,] data)
        {
            string range = $"{SheetName}!A1"; // Especifica el rango donde se quiere escribir
            var valueRange = new ValueRange();

            // Convierte los datos a un formato adecuado
            var values = new List<IList<object>>();
            for(int i = 0; i < data.GetLength(0); i++)
            {
                var row = new List<object>();
                for(int j = 0; j < data.GetLength(1); j++)
                {
                    row.Add(data[i, j]);
                }
                values.Add(row);
            }

            valueRange.Values = values;

            // Llama a la API de Google Sheets
            var request = sheetsService.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

            await request.ExecuteAsync();
        }
    }
}
