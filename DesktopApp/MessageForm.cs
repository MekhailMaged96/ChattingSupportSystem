using ApplicationCore.Services.MessageService;
using ApplicationCore.Services.UserService;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class MessageForm : Form
    {
        public string UserName { get; set; }
        public string Recipient { get; set; }

        private UnitOfWork unitOfWork = new UnitOfWork();
        private MessageService messagesService = new MessageService();
        private UserService userService = new UserService();



        public MessageForm()
        {
            InitializeComponent();
        
        }

        public void Load_users()
        {
            var users = unitOfWork.UserRepo.Get(e=>e.UserName != UserName).ToList();

            foreach(var user in users)
            {
                listUsers.Items.Add(user.UserName);
            }
          
        }
        public void Load_Messages(string RecipientUserName)
        {
            var messages = messagesService.GetMessageThreadForDesktop(UserName, RecipientUserName);
            
            foreach(var message in messages)
            {
                txtInfo.Text += message.SenderUsername + ">> " + message.Content + Environment.NewLine;
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

                txtMessage.Text = string.Empty;
            }
        }

        private void listUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Recipient = listUsers.SelectedItem.ToString();
            txtInfo.Text = string.Empty;
            Load_Messages(Recipient);
        }

        private void txtInfo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
