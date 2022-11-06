﻿using ABE.LTRIC.Infrastructure.Interfaces;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ABE.LTRIC.WpfGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ICompanyRepository _companyRepository;
        public MainWindow(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
            InitializeComponent();
        }

        private async void btnAddTest_Click(object sender, RoutedEventArgs e)
        {
            var all = (await _companyRepository.GetAll()).ToList();
        }

    }
}
