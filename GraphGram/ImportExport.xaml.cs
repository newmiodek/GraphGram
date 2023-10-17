using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace GraphGram;

public partial class ImportExport : ContentPage {
	public ImportExport() {
		InitializeComponent();
	}

	private async void StartFilePicker(object sender, EventArgs e) {
		PickOptions options = new PickOptions();
		await PickAndShow(options);
	}

	private async Task<FileResult> PickAndShow(PickOptions options) {
		try {
			var result = await FilePicker.Default.PickAsync(options);
			if(result != null) {
				using var stream = await result.OpenReadAsync();
				using var stream_reader = new StreamReader(stream);
				string text = stream_reader.ReadToEnd();
				float?[,] decoded = DecodeCVS(text);
				/*string inserted_text = "";
				bool found_null = false;
				for(int i = 0; i < decoded.GetLength(0); i++) {
					for(int j = 0; j < 4; j++) {
						if(decoded[i, j] == null) {
                            found_null = true;
                            break;
                        }
						inserted_text += decoded[i, j].ToString() + " ";
                    }
					if(found_null) {
						break;
					}
					inserted_text += "\r\n";
				}
				Debug.WriteLine(inserted_text);
				textfield.Text = inserted_text;*/
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
		bool decode_failed = false;
		for(int i = 0; i < cvs.Length; i++) {
			if(cvs[i] == ' ') {
				Debug.WriteLine("Option 1");
				float parsed_number;
				if(!float.TryParse(current_number, out parsed_number)) {
					Debug.WriteLine("Decode Failed\nLast char = \"" + cvs[i] + "\"");
                    decode_failed = true;
					break;
				}
				output[row, column] = parsed_number;
				column++;
				if(column > 3) {
                    Debug.WriteLine("Decode Failed\nLast char = \"" + cvs[i] + "\"");
                    decode_failed = true;
					break;
				}
				current_number = "";
			}
			else if(cvs[i] == '\r') {
                Debug.WriteLine("Option 2");
                float parsed_number;
				if(!float.TryParse(current_number, out parsed_number)) {
                    Debug.WriteLine("Decode Failed\nLast char = \"" + cvs[i] + "\"");
                    decode_failed = true;
					break;
				}
				output[row, column] = parsed_number;
				if(column != 3) {
                    Debug.WriteLine("Decode Failed\nLast char = \"" + cvs[i] + "\"");
                    decode_failed = true;
					break;
				}
				column = 0;
				row++;
				current_number = "";
				if(i + 1 < cvs.Length && cvs[i + 1] == '\n') {
                    Debug.WriteLine("Option 2.5");
                    i++;
				}
			}
			else if(cvs[i] == '\n') {
                Debug.WriteLine("Option 3");
                float parsed_number;
				if(!float.TryParse(current_number, out parsed_number)) {
                    Debug.WriteLine("Decode Failed\nLast char = \"" + cvs[i] + "\"");
                    decode_failed = true;
					break;
				}
				output[row, column] = parsed_number;
				if(column != 3) {
                    Debug.WriteLine("Decode Failed\nLast char = \"" + cvs[i] + "\"");
                    decode_failed = true;
					break;
				}
				column = 0;
				row++;
				current_number = "";
			}
			else if(cvs[i] == '0' || cvs[i] == '1' || cvs[i] == '2' || cvs[i] == '3' || cvs[i] == '4' ||
					cvs[i] == '5' || cvs[i] == '6' || cvs[i] == '7' || cvs[i] == '8' || cvs[i] == '9' ||
					cvs[i] == '.' || cvs[i] == '-' || cvs[i] == '+' || cvs[i] == 'e' || cvs[i] == 'E') {
                Debug.WriteLine("Option 4");
                Debug.WriteLine("Got char '" + cvs[i] + "'");
				current_number += cvs[i];
			}

		}
		return output;
	}

	private async void StartFileSaver(object sender, EventArgs e) {
        using var stream = new MemoryStream(Encoding.Default.GetBytes("Hello from the Community Toolkit!"));

		var tokenSource = new CancellationTokenSource();
		CancellationToken ct = tokenSource.Token;

        var fileSaverResult = await FileSaver.Default.SaveAsync("test.txt", stream, ct);
        if(fileSaverResult.IsSuccessful) {
            await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(ct);
        }
        else {
            await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(ct);
        }
    }
}
