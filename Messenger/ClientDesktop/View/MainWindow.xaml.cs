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
            Message message = new Message(Config.UId, Config.UserName, text);
            DAL.SendMessageAsync(message);
            DAL.GetMessagesAsync(Config.UId);
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
        /// Если чекбокс фмльтрации сообщений по дате доступен, то и поля выыбора дат доступны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChbIsFilterEnable_Checked(object sender, RoutedEventArgs e)
        {
            DatePicker dpStartDate = ((Grid)this.Content).Children.OfType<DatePicker>().Single(Child => Child.Uid == "dpStartDate");
            DatePicker dpEndDate = ((Grid)this.Content).Children.OfType<DatePicker>().Single(Child => Child.Uid == "dpEndDate");
            dpStartDate.IsEnabled = true;
            dpEndDate.IsEnabled = true;
        }

        /// <summary>
        /// Если чекбокс фильтрации сообщений по дате недоступен, то и поля выбора дат недоступны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnChbIsFilterEnable_Unchecked(object sender, RoutedEventArgs e)
        {
            DatePicker dpStartDate = ((Grid)this.Content).Children.OfType<DatePicker>().Single(Child => Child.Uid == "dpStartDate");
            DatePicker dpEndDate = ((Grid)this.Content).Children.OfType<DatePicker>().Single(Child => Child.Uid == "dpEndDate");
            dpStartDate.IsEnabled = false;
            dpEndDate.IsEnabled = false;
            TextBox tbxChat = ((Grid)this.Content).Children.OfType<TextBox>().Single(Child => Child.Uid == "tbxChat");
            List<Message> messageList = await DAL.GetMessagesAsync(Config.UId);            
            tbxChat.Text = string.Join(Environment.NewLine, messageList);    
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDpStartDate_SelectDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
            DatePicker dpStartDate = (DatePicker)(sender);
            DatePicker dpEndDate = ((Grid)this.Content).Children.OfType<DatePicker>().Single(Child => Child.Uid == "dpEndDate");
            filterMessages(dpStartDate, dpEndDate);
        }

        private async void filterMessages(DatePicker dpStartDate, DatePicker dpEndDate)
        {
            TextBox tbxChat = ((Grid)this.Content).Children.OfType<TextBox>().Single(Child => Child.Uid == "tbxChat");
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;

            if (dpStartDate.SelectedDate != null && dpEndDate.SelectedDate != null)
            {
                List<Message> messageList = await DAL.GetMessagesAsync(Config.UId);
                messageList = messageList.Where(x => x.Datetime >= startDate && x.Datetime <= endDate).ToList();
                tbxChat.Text = string.Join(Environment.NewLine, messageList);
            }
        }

        /// <summary>
        /// Чекбокс фильтрации сообщений доступен только при их наличии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTbxChat_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckBox chbIsFilterEnable = ((Grid)this.Content).Children.OfType<CheckBox>().Single(Child => Child.Uid == "chbIsFilterEnable");

            if (((TextBox)sender).Text.Length > 0)
            {  
                chbIsFilterEnable.IsEnabled = true;
            }
            else
            {
                chbIsFilterEnable.IsEnabled = false;
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
                List<Message> messageList = DAL.GetMessagesAsync(Config.UId).Result;
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