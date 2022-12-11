﻿using RestaurantsClasses.WorkersSystem;
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
using ui.Helper;

namespace ui.WorkerWorkspacePages
{
    /// <summary>
    /// Логика взаимодействия для WorkerOrders.xaml
    /// </summary>
    public partial class WorkerOrders : Window
    {
        private Window _previous;
        private Worker _worker;

        public WorkerOrders(Window previous, Worker worker)
        {
            InitializeComponent();
            _previous = previous;
            _worker = worker;

            var orders = RequestClient.GetAllOfflineOrders().Where(x => x.ServerId == _worker.id);

            var buttonTemplate = new FrameworkElementFactory(typeof(Button));
            buttonTemplate.SetBinding(Button.NameProperty, new Binding("Id"));
            buttonTemplate.SetBinding(Button.ContentProperty, new Binding("ButtonText"));
            //buttonTemplate.Text = "Закрепить за собой";
            buttonTemplate.AddHandler(Button.ClickEvent, new RoutedEventHandler((o, e) => setCompleteButton(o, e)));

            ordersGrid.Columns.Add(
                new DataGridTextColumn()
                {
                    Header = "Когда был создан",
                    Binding = new Binding("Created"),
                    Width = 200
                }
            );

            ordersGrid.Columns.Add(
                new DataGridTextColumn()
                {
                    Header = "Статус",
                    Binding = new Binding("Status"),
                    Width = 110
                }
            );

            ordersGrid.Columns.Add(
                new DataGridTextColumn()
                {
                    Header = "Столик",
                    Binding = new Binding("TableNum"),
                    Width = 110
                }
            );

            ordersGrid.Columns.Add(
                new DataGridTemplateColumn()
                {
                    Header = "Закрепить за собой",
                    CellTemplate = new DataTemplate() { VisualTree = buttonTemplate },
                    Width = 200
                }
            );


            foreach (var order in orders)
            {
                ordersGrid.Items.Add(new Item()
                {
                    Created = order.Created,
                    Status = order.Status,
                    TableNum = order.TableId,
                    Id = order.id,
                    ButtonText = $"Закрепить за собой заказ {order.id}"
                });
            }
        }

        private void setCompleteButton(object sender, RoutedEventArgs e)
        {
            string rawOrderId = ((Button)sender).Content.ToString().Split(' ').Last();
            if (!int.TryParse(rawOrderId, out int orderId))
                return;

            RequestClient.SetOrderToWorker(orderId, _worker.id);
            exitButton_Click(sender, e);
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            _previous.Show();
            Close();
        }
    }
}