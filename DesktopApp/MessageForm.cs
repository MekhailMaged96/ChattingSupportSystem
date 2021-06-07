using ApplicationCore.Services.MessageService;
using ApplicationCore.Services.UserService;
using Events.Messages.Common;
using Events.Messages.Events;
using Infrastructure.UnitOfWork;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace DesktopApp
{
    public partial class MessageForm : Form
    {
        public string UserName { get; set; }
        public string Recipient { get; set; }
        public string message;
        private UnitOfWork unitOfWork = new UnitOfWork();
        private MessageService messagesService = new MessageService();
        private UserService userService = new UserService();
        private BackgroundWorker backgroundWorker1;


        public MessageForm()
        {
            InitializeComponent();
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(BackgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker1_RunWorkerCompleted);


        }
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                if (!string.IsNullOrEmpty(Recipient))
                {

                    channel.QueueDeclare(queue: Recipient, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    BasicGetResult result = channel.BasicGet(queue: Recipient, autoAck: true);
                    if (result != null)
                    {
                        var conent = Encoding.UTF8.GetString(result.Body.ToArray());
                        var message = JsonConvert.DeserializeObject<MessageEvent>(conent);

                        e.Result = message;
                    }
                }
            }

        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           var obj =  e.Result as MessageEvent;
           ReceiveMessage(obj);

            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        
        }
        private void MessageForm_Load(object sender, EventArgs e)
        {
         
        }
       
        public void Load_Messages(string RecipientUserName)
        {
            var messages = messagesService.GetMessageThreadForDesktop(UserName, RecipientUserName);
            
            foreach(var message in messages)
            {
                txtInfo.Text += message.SenderUsername + ">>" + message.Content + Environment.NewLine;
               // txtInfo.Text += message.MessageSent.ToString() + Environment.NewLine; ;
            }


        }
        public void Load_users()
        {
            var users = unitOfWork.UserRepo.Get(e => e.UserName != UserName).ToList();

            foreach (var user in users)
            {
                listUsers.Items.Add(user.UserName);
            }

        }
        private void btnSend_Click(object sender, EventArgs e)
        {
         

            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                var send = userService.GetUserByName(UserName);

                var recep = userService.GetUserByName(Recipient);

                var message = new Infrastructure.Aggregets.MessageAgg.Message
                {
                   Content = txtMessage.Text,
                   RecipientId = recep.Id,
                   SenderId = send.Id,
                   RecipientUsername =recep.UserName,
                   SenderUsername = send.UserName
                };

                unitOfWork.MessageRepo.Insert(message);
                unitOfWork.Save();

                txtInfo.Text += message.SenderUsername + ">>" + message.Content + Environment.NewLine;
              //  txtInfo.Text += message.MessageSent.ToString()+Environment.NewLine; ;
                txtMessage.Text = string.Empty;
            }
        }

        private void listUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Recipient = listUsers.SelectedItem.ToString();
            txtInfo.Text = string.Empty;
            Load_Messages(Recipient);
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
          
        }

        private void txtInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void ReceiveMessage(MessageEvent message)
        {
            if(message != null)
            {
                txtInfo.Text += message.SenderUsername + ">>" + message.Content + Environment.NewLine;

                Load_Messages(message.RecipientUsername);
            }
          
            //   Load_Messages(Recipient);
        }
    }
}
