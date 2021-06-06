using ApplicationCore.Services.UserService;
using Events.Messages.Common;
using Infrastructure.UnitOfWork;
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

namespace DesktopApp
{
    public partial class LoginForm : Form
    {
        private UserService userService = new UserService();
        private MessageForm messageForm = new MessageForm();
        public LoginForm()
        {
            InitializeComponent();
            /*
            while (true)
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(queue: EventBusConstants.MessageQueue,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);



                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        MessageBox.Show(message);
                    };

                    channel.BasicConsume(queue: EventBusConstants.MessageQueue,
                                            autoAck: true,
                                            consumer: consumer);
                }
            }
            */

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text))
            {
                var user = userService.GetUserByName(txtUserName.Text);

                if(user == null)
                {
                    MessageBox.Show("User not found");
                }
                else
                {
                    this.Hide();
                    messageForm.UserName = user.UserName;
                    messageForm.Load_users();
                    messageForm.Show();
                }

            }
        }
    }
}
