using TreeViewItem = Wpf.Ui.Controls.TreeViewItem;

namespace MorcuTool.ViewModels.Pages;

public class PackageViewPageViewModel : ViewModelBase
{
    public ObservableCollection<TreeViewItem> TreeItems { get; } = new();
}