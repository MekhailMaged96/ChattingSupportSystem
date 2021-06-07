using ApplicationCore.DTO;
using ApplicationCore.Services.MessageService;
using ApplicationCore.Services.UserService;
using AutoMapper;
using Events.Messages.Common;
using Events.Messages.Events;
using Infrastructure.UnitOfWork;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Microsoft.AspNet.Identity;

namespace WebApp.Controllers
{

    public class MessagesController : Controller
    {


        private MessageService messageService;
        private UserService userService;
        private UnitOfWork unitOfWork;

        public MessagesController()
        {
            messageService = new MessageService();
            userService = new UserService();
            unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            return View();
        }

    
        [Route("~/messages/details/{username?}")]
        public ActionResult Details(string username)
        {
            var user = unitOfWork.UserRepo.Get(e => e.UserName == username).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }


            return View(user);

            
        }

        [HttpPost]
        [Authorize]
        [Route("~/messages/createMessage")]
        public ActionResult CreateMessage(MessagesParamDTO createMessage)
        {

            createMessage.SenderId = User.Identity.GetUserId();
            createMessage.RecipientId =createMessage.RecipientId;


            var sender =  userService.GetUserById(createMessage.SenderId);

            var recipient =  userService.GetUserById(createMessage.RecipientId);

            if (recipient == null)
            {
                return Json("not found");
            }
            var message = new Infrastructure.Aggregets.MessageAgg.Message()
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessage.Content
            };

            unitOfWork.MessageRepo.Insert(message);

            unitOfWork.Save();

            
          
            
            var messageEvent = new MessageEvent()
            {
                Id = message.Id,
                Content = message.Content,
                RecipientId = message.RecipientId,
                SenderId = message.SenderId,
                MessageSent = message.MessageSent,
                SenderUsername = message.SenderUsername,
                RecipientUsername = message.RecipientUsername
            };

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
             /*
                channel.QueueDeclare(queue: EventBusConstants.MessageQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);


                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageEvent));

                channel.BasicPublish(exchange: "",
                                     routingKey: EventBusConstants.MessageQueue,
                                     basicProperties: null,
                                     body: body);

                */

                channel.ExchangeDeclare(EventBusConstants.MessageQueue, ExchangeType.Direct);
                channel.QueueDeclare(sender.UserName, true, false, false, null);
                channel.QueueBind(sender.UserName, EventBusConstants.MessageQueue, sender.UserName, null);
                var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageEvent));

                channel.BasicPublish(EventBusConstants.MessageQueue, sender.UserName, null, msg);

            }
            



            var messageDto = Mapper.Map<MessageDTO>(message);
            


            return Json(messageDto, JsonRequestBehavior.AllowGet);
            
        }

        [Authorize]
        [HttpPost,Route("~/messages/GetMessageThread")]
        public ActionResult GetMessageThread(MessagesParamDTO messageDTO)
        {

            messageDTO.SenderId = User.Identity.GetUserId();

            var messages = messageService.GetMessageThread(messageDTO.SenderId, messageDTO.RecipientId);

            var messagesDto = Mapper.Map<IEnumerable<MessageDTO>>(messages);
   

            return Json(messagesDto);
        }


    }
}