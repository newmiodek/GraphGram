namespace GraphGram;

public partial class AboutPage : ContentPage {
    public AboutPage() {
		InitializeComponent();
	}

	private async void LicenseButtonClick(object sender, EventArgs e) {
		Button button = (Button)sender;
		if(button == CommunityToolkitMvvmButton)
			await Launcher.OpenAsync("https://github.com/CommunityToolkit/dotnet/blob/main/License.md");
		else if(button == InkscapeButton)
            await Launcher.OpenAsync("https://inkscape.org/about/license/");
		else if(button == MauiButton)
			await Launcher.OpenAsync("https://github.com/dotnet/maui/blob/main/LICENSE.txt");
    }
}
