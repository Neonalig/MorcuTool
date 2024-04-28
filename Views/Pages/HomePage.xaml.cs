namespace MorcuTool.Views.Pages;

public partial class HomePage
{
    public HomePage()
    {
        InitializeComponent();
    }

    void OpenPackageButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog1 = new OpenFileDialog
        {
            Title = "Select MySims package",
            DefaultExt = "package",
            Filter = "MySims package (*.package; *.wii)|*.package;*.wii",
            CheckFileExists = true,
            CheckPathExists = true,
        };

        if (openFileDialog1.ShowDialog() == true)
        {
            AppState.activePackage = new Package
            {
                filebytes = File.ReadAllBytes(openFileDialog1.FileName),
                filename = openFileDialog1.FileName,
                viewModel = AppState.activePackageViewPageViewModel,
            };

            AppState.activePackage.LoadPackage();
        }
    }
}