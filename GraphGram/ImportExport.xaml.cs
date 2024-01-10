using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace GraphGram;

public partial class ImportExport : ContentPage {
	public ImportExport() {
		InitializeComponent();
	}

	private async void StartFilePicker(object sender, EventArgs e) {
		PickOptions options = new PickOptions();

        var tokenSource = new CancellationTokenSource();
        CancellationToken ct = tokenSource.Token;

        var fileSaverResult = await PickAndShow(options);
        if(fileSaverResult != null) {
            await Toast.Make("Data successfully imported to the table").Show(ct);
        }
        else {
            await Toast.Make("Failed to import data").Show(ct);
        }
    }

	private async Task<FileResult> PickAndShow(PickOptions options) {
		try {
			var result = await FilePicker.Default.PickAsync(options);
			if(result != null) {
				using var stream = await result.OpenReadAsync();
				using var stream_reader = new StreamReader(stream);
				string text = stream_reader.ReadToEnd();
				float?[,] decoded = DecodeCVS(text);
                WeakReferenceMessenger.Default.Send(new ImportDataMessage(decoded));
            }
			return result;
		}
		catch { }
		return null;
	}

	private float?[,] DecodeCVS(string cvs) {
		int row = 0;
		int column = 0;
		string current_number = "";
		float?[,] output = new float?[Constants.DEFAULT_ROW_COUNT, 4];
		for(int i = 0; i < cvs.Length; i++) {
			if(cvs[i] == '\t') {
				float parsed_number;
				if(!float.TryParse(current_number, out parsed_number)) {
					break;
				}
				output[row, column] = parsed_number;
				column++;
				if(column > 3) {
					break;
				}
				current_number = "";
			}
			else if(cvs[i] == '\r') {
                float parsed_number;
				if(!float.TryParse(current_number, out parsed_number)) {
					break;
				}
				output[row, column] = parsed_number;
				if(column != 3) {
					break;
				}
				column = 0;
				row++;
				current_number = "";
				if(i + 1 < cvs.Length && cvs[i + 1] == '\n') {
                    i++;
				}
			}
			else if(cvs[i] == '\n') {
                float parsed_number;
				if(!float.TryParse(current_number, out parsed_number)) {
					break;
				}
				output[row, column] = parsed_number;
				if(column != 3) {
					break;
				}
				column = 0;
				row++;
				current_number = "";
			}
			else if(cvs[i] == '0' || cvs[i] == '1' || cvs[i] == '2' || cvs[i] == '3' || cvs[i] == '4' ||
					cvs[i] == '5' || cvs[i] == '6' || cvs[i] == '7' || cvs[i] == '8' || cvs[i] == '9' ||
					cvs[i] == '.' || cvs[i] == '-' || cvs[i] == '+' || cvs[i] == 'e' || cvs[i] == 'E') {
				current_number += cvs[i];
			}
		}
		return output;
	}

	private async void StartFileSaver(object sender, EventArgs e) {
		RequestMessage<string> tableRequest = new RequestMessage<string>();
		WeakReferenceMessenger.Default.Send(tableRequest);

        using var stream = new MemoryStream(Encoding.Default.GetBytes(tableRequest.Response));

		var tokenSource = new CancellationTokenSource();
		CancellationToken ct = tokenSource.Token;

        var fileSaverResult = await FileSaver.Default.SaveAsync("test.cvs", stream, ct);
        if(fileSaverResult.IsSuccessful) {
            await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(ct);
        }
        else {
            await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(ct);
        }
    }
}
