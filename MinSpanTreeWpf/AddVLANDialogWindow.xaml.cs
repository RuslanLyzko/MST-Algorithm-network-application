using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MinSpanTreeWpf
{
    /// <summary>
    /// Interaction logic for DistanceDialog.xaml
    /// </summary>
    public partial class AddVLANDialogWindow: Window
    {
        public string VLAN_IdsStr { get; private set; }

        public AddVLANDialogWindow()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxVLANIds.Text))
            {
                VLAN_IdsStr = tbxVLANIds.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please give a valid node identifiers", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbxVLANIds.Focus();
        }
    }
}
