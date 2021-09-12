using CommonClasses.Model;
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

namespace ClientDesktop.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConfigReader configReader = new ConfigReader();
        private DAL dal = new DAL();

        public MainWindow()
        {
            InitializeComponent();

            setBindingDatePickers();
            setBindingChbxIsFilterApplied();
        }

        /// <summary>
        /// Биндинг: поля выбора дат доступны только при отмеченном чекбоксе фильтрации //chbxIsFilterApplied.IsChecked
        /// </summary>
        private void setBindingDatePickers()
        {
            Binding binding = new Binding();
            binding.ElementName = "chbxIsFilterApplied"; // элемент-источник
            binding.Path = new PropertyPath("IsChecked"); // свойство элемента-источника
            binding.Mode = BindingMode.OneWay; // установка режима связи (не обязательно)
            dpStartDate.SetBinding(DatePicker.IsEnabledProperty, binding); // установка привязки для элемента-приемника 1
            dpEndDate.SetBinding(DatePicker.IsEnabledProperty, binding); // установка привязки для элемента-приемника 2
        }

        /// <summary>
        /// Биндинг: чекбокс фильтрации доступен только при наличии сообщений в поле чата
        /// </summary>
        private void setBindingChbxIsFilterApplied()
        {
            Binding binding = new Binding();
            binding.ElementName = "tbxChat"; // элемент-источник
            binding.Path = new PropertyPath("TextProperty"); // свойство элемента-источника
            binding.Mode = BindingMode.OneWay; // установка режима связи (не обязательно)

            //binding.Converter = new TextToBoolConverter();
            chbxIsFilterApplied.SetBinding(CheckBox.IsEnabledProperty, binding); // установка привязки для элемента-приемника
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnSendMessage_Click(Object sender, RoutedEventArgs e)
        {
            string text = tbxMessage.Text;
            Message message = new Message(configReader.UId, configReader.UserName, text);
            dal.SendMessageAsync(message);
            dal.GetMessagesAsync(configReader.UId);
            tbxChat.Text = tbxChat.Text + tbxMessage.Text;
            tbxMessage.Text = "";
            ((Button)sender).IsEnabled = false;
        }

        /// <summary>
        /// Кнопка "Отправить" доступна только при наличии текста сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTbxMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSendMessage.IsEnabled = ((TextBox)sender).Text.Length > 0;
        }

        /// <summary>
        /// Изменение значения конечной даты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDpEndDate_SelectDateChanged(object sender, SelectionChangedEventArgs e)
        {
            filterMessages(dpStartDate, dpEndDate);
        }

        /// <summary>
        /// Изменение значения начальной даты
        /// </summary>
        private async void OnDpStartDate_SelectDateChanged(object sender, SelectionChangedEventArgs e)
        {
            filterMessages(dpStartDate, dpEndDate);
        }

        /// <summary>
        /// Фильтрация сообщений, если отмечен чекбокс фильтрации и заполнены обе даты
        /// </summary>
        private async void filterMessages(DatePicker dpStartDate, DatePicker dpEndDate)
        {
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;

            if (chbxIsFilterApplied.IsChecked == true && dpStartDate.SelectedDate != null && dpEndDate.SelectedDate != null)
            {
                List<Message> messageList = await dal.GetMessagesAsync(configReader.UId);
                messageList = messageList.Where(x => x.Datetime.Date >= startDate && x.Datetime.Date <= endDate).ToList();
                tbxChat.Text = string.Join(Environment.NewLine, messageList);
            }
        }
        
        /// <summary>
        /// Окно с перепиской загружено
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTbxChat_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Message> messageList = dal.GetMessagesAsync(configReader.UId).Result;
                String textString = string.Join(Environment.NewLine, messageList);
                tbxChat.Text = textString;
            }
            catch (Exception ex)
            {
                tbxMessage.Text = ex.Message;
            }
        }
    }
}