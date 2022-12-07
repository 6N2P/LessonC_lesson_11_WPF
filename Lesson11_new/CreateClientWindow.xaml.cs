using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Lesson11_new.ViewModels;

namespace Lesson11_new
{
    /// <summary>
    /// Логика взаимодействия для CreateClientWindow.xaml
    /// </summary>
    public partial class CreateClientWindow : Window
    {
        public CreateClientWindow()
        {
            InitializeComponent();
            DataContext = new CreateClientWindowViewModel(this);
        }
    }
}
