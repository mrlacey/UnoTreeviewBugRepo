using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace UnoTreeviewBugRepo
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<TreeViewItem> treeViewContents = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        public MainPage()
        {
            this.InitializeComponent();
            UpdateTreeView();
        }

        public ObservableCollection<TreeViewItem> TreeViewContents
        {
            get => treeViewContents;
            set
            {
                treeViewContents = value;
                PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(TreeViewContents)));
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateTreeView();
        }

        private void UpdateTreeView()
        {
            TreeViewContents.Clear();

            var content = CreateTreeView(1);

            if (content is not null)
            {
                TreeViewContents.Add(content);
            }

            DisplayedTreeView.ItemsSource = TreeViewContents;
        }

        private TreeViewItem? CreateTreeView(int level)
        {
            var result = new TreeViewItem() { Name = $"Level {level}" };

            if (level < 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    result.Children.Add(CreateTreeView(level + 1));
                }
            }

            return result;
        }
    }

    public class TreeViewItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string? Name { get; set; }
        private ObservableCollection<TreeViewItem>? m_children;
        public ObservableCollection<TreeViewItem> Children
        {
            get
            {
                m_children ??= new ObservableCollection<TreeViewItem>();

                return m_children;
            }
            set
            {
                m_children = value;
                NotifyPropertyChanged(nameof(Children));
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
