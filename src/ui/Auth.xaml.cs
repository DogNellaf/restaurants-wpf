﻿using System;
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
using ui.Helper;

namespace ui
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            var username = usernameTextBox.Text;
            var password = passwordTextBox.Text;
            var client = RequestClient.Auth(username, password);
            if (client is not null)
            {
                new ClientWorkspace(client).Show();

            }
            else
            {
                MessageBox.Show("Введены неверные данные");
            }
        }
    }
}