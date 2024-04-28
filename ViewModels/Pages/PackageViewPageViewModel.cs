namespace MorcuTool.ViewModels.Pages;

public class PackageViewPageViewModel : ViewModelBase
{
    public ObservableCollection<TreeViewItem> TreeItems { get; } = new();

    public PackageViewPageViewModel()
    {
        BindingOperations.EnableCollectionSynchronization(TreeItems, treeItemsLock);
    }

    readonly object treeItemsLock = new();

    public void MakeFileTree()
    {
        lock (treeItemsLock)
        {
            TreeItems.Clear();

            // var treeNodesAndSubfiles = new Dictionary<TreeViewItem, Subfile>();
            var foldersProcessed = new Dictionary<string, TreeViewItem>();

            List<Subfile> subfiles = AppState.activePackage.subfiles;

            foreach (Subfile file in subfiles)
            {
                //now add entries to the file tree

                var dirs = new List<string>();

                string tempFilePath = Path.GetFileName(AppState.activePackage.filename).Replace("/", "");

                if (file.filename[0] != '/')
                    tempFilePath += "/";

                tempFilePath += file.filename;

                if (tempFilePath[^1] == '/')
                    tempFilePath = tempFilePath.Remove(tempFilePath.Length - 1);

                if (tempFilePath[0] == '/')
                    tempFilePath = tempFilePath.Substring(1, tempFilePath.Length - 1);

                int number_of_dir_levels = tempFilePath.Split('/').Length;

                for (int d = 0; d < number_of_dir_levels - 1; d++) //store a string for each level of the directory, so that we can check each folder individually (by this I mean checking whether or not it already exists in the tree)
                {
                    string dirName = Path.GetDirectoryName(tempFilePath)!;
                    dirs.Add(dirName);
                    tempFilePath = dirName;

                    if (tempFilePath[^1] == '/')
                        tempFilePath = tempFilePath.Remove(tempFilePath.Length - 1);
                }

                bool isRoot = true;
                var newestNode = new TreeViewItem();

                TreeViewItem selectedNode;

                for (int f = dirs.Count - 1; f >= 0; f--)
                {
                    if (!foldersProcessed.ContainsKey(dirs[f].ToLower())) //if the folder isn't in the tree yet
                    {
                        if (!isRoot)
                        {
                            //add to the chain of nodes
                            selectedNode = newestNode;
                            newestNode = new TreeViewItem
                            {
                                Header = Path.GetFileName(dirs[f]),
                                // ImageIndex = 0,
                                // SelectedImageIndex = 0,
                            };
                            selectedNode.Items.Add(newestNode);
                        }
                        else
                        {
                            //create a root node first
                            newestNode = new TreeViewItem
                            {
                                Header = Path.GetFileName(dirs[f]),
                                // ImageIndex = 0,
                                // SelectedImageIndex = 0,
                            };
                            TreeItems.Add(newestNode);
                            isRoot = false;
                        }

                        foldersProcessed.Add(dirs[f].ToLower(), newestNode); //add it to the list of folders we've put in the tree
                    }
                    else
                    {
                        newestNode = foldersProcessed[dirs[f].ToLower()]; //set the parent node of the next folder to the existing node
                        // newestNode.ImageIndex = 0;
                        // newestNode.SelectedImageIndex = 0;
                        isRoot = false;
                    }
                }

                selectedNode = newestNode;
                newestNode = new TreeViewItem
                {
                    Header = Path.GetFileName(file.filename),
                    // ImageIndex = 1,
                    // SelectedImageIndex = 1,
                };
                selectedNode.Items.Add(newestNode);
                file.treeNode = newestNode;
                newestNode.SetSubfile(file);
            }
        }
    }
}

public static class SubfileTreeView
{
    public static readonly DependencyProperty SubfileProperty = DependencyProperty.RegisterAttached(
        "Subfile",
        typeof(Subfile),
        typeof(SubfileTreeView),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
    );

    public static void SetSubfile(this TreeViewItem element, Subfile value)
    {
        element.SetValue(SubfileProperty, value);
    }

    public static Subfile GetSubfile(this TreeViewItem element)
    {
        return (Subfile)element.GetValue(SubfileProperty);
    }
}