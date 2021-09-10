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
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnSendMessage_Click(Object sender, RoutedEventArgs e)
        {
            TextBox tbxChat = ((Grid)this.Content).Children.OfType<TextBox>().Single(Child => Child.Uid == "tbxChat");
            TextBox tbxMessage = ((Grid)this.Content).Children.OfType<TextBox>().Single(Child => Child.Uid == "tbxMessage");
            string text = tbxMessage.Text;
            Message message = new Message(configReader.UId, configReader.UserName, text);
            dal.SendMessageAsync(message);
            dal.GetMessagesAsync(configReader.UId);
            tbxChat.Text = tbxChat.Text + tbxMessage.Text;
            tbxMessage.Text = "";
            ((Button)sender).IsEnabled = false;
        }

        /// <summary>
        /// Изменился текст внутри поля набора нового сообшения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTbxMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Проверка на наличие текста для управления доступностью кнопки "Отправить"
            Button btnSendMessage = ((Grid)this.Content).Children.OfType<Button>().Single(Child => Child.Uid == "btnSendMessage");

            if (((TextBox)sender).Text.Length > 0)
            {
                btnSendMessage.IsEnabled = true;
            }
            else
            {
                btnSendMessage.IsEnabled = false;
            }
        }

        /// <summary>
        /// Изменение значения конечной даты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDpEndDate_SelectDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker dpStartDate = ((Grid)this.Content).Children.OfType<DatePicker>().Single(Child => Child.Uid == "dpStartDate");
            DatePicker dpEndDate = (DatePicker)(sender);
            filterMessages(dpStartDate, dpEndDate);
        }

        /// <summary>
        /// Изменение значения начальной даты
        /// </summary>
        private async void OnDpStartDate_SelectDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker dpStartDate = (DatePicker)(sender);
            DatePicker dpEndDate = ((Grid)this.Content).Children.OfType<DatePicker>().Single(Child => Child.Uid == "dpEndDate");
            filterMessages(dpStartDate, dpEndDate);
        }

        /// <summary>
        /// Фильтрация сообщений, если отмечен чекбокс фильтрации и заполнены обе даты
        /// </summary>
        private async void filterMessages(DatePicker dpStartDate, DatePicker dpEndDate)
        {
            TextBox tbxChat = ((Grid)this.Content).Children.OfType<TextBox>().Single(Child => Child.Uid == "tbxChat");
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;
            CheckBox chbIsFilterApplied = ((Grid)this.Content).Children.OfType<CheckBox>().Single(Child => Child.Uid == "chbIsFilterApplied");

            if (chbIsFilterApplied.IsChecked == true && dpStartDate.SelectedDate != null && dpEndDate.SelectedDate != null)
            {
                List<Message> messageList = await dal.GetMessagesAsync(configReader.UId);
                messageList = messageList.Where(x => x.Datetime >= startDate && x.Datetime <= endDate).ToList();
                tbxChat.Text = string.Join(Environment.NewLine, messageList);
            }
        }

        /// <summary>
        /// Чекбокс фильтрации сообщений и поля выбора дат доступны только при наличии сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTbxChat_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckBox chbIsFilterApplied = ((Grid)this.Content).Children.OfType<CheckBox>().Single(Child => Child.Uid == "chbIsFilterApplied");
            DatePicker dpStartDate = ((Grid)this.Content).Children.OfType<DatePicker>().Single(Child => Child.Uid == "dpStartDate");
            DatePicker dpEndDate = ((Grid)this.Content).Children.OfType<DatePicker>().Single(Child => Child.Uid == "dpEndDate");

            if (((TextBox)sender).Text.Length > 0)
            {  
                chbIsFilterApplied.IsEnabled = true;
                dpStartDate.IsEnabled = true;
                dpEndDate.IsEnabled = true;
            }
            else
            {
                chbIsFilterApplied.IsEnabled = false;
                dpStartDate.IsEnabled = false;
                dpEndDate.IsEnabled = false;
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

                TextBox tbxChat = ((Grid)this.Content).Children.OfType<TextBox>().Single(Child => Child.Uid == "tbxChat");
                tbxChat.Text = textString;

            }
            catch (Exception ex)
            {
                TextBox tbxMessage = ((Grid)this.Content).Children.OfType<TextBox>().Single(Child => Child.Uid == "tbxMessage");
                tbxMessage.Text = ex.Message;
            }
        }
    }
}