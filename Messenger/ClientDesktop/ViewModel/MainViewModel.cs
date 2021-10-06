using CommonClasses;
using CommonClasses.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientDesktop.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        DAL DAL = new DAL();
        private ConfigReader configReader = new ConfigReader();

        private ICommand _sendMesageCommand;
        public ICommand SendMessageCommand => _sendMesageCommand ?? new RelayCommand(SendMessage);

        private void SendMessage()
        {
            Message message = new Message(1, "USER", CurrentMessage);
            Messages.Add(message);
            DAL.SendMessageAsync(message);
            OnPropertyChanged(nameof(CurrentMessage));
        }

        ///// <summary>
        ///// Коллекция сообщений
        ///// </summary>
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        private string _currentMessage;
        public string CurrentMessage
        {
            get => _currentMessage;
            set
            {
                _currentMessage = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            string filePath = configReader.FilePathMessages + configReader.UId.ToString() + "_messages.json";
            // загрузка кеша сообщений из json
            List<Message> messages = JsonHelper.LoadFromJSON(configReader.UId, filePath); //loadFromJSON(configReader.UId);
            Messages = new ObservableCollection<Message>(messages); 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}