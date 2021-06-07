# Chatting Support System

### Technology : 
<ul>
   <li>	asp.net mvc  was used to create web app  and win forms was used to create desktop app  and they make communcation using rabbitmq server </li>
   <li>	User sends  a message to another user  and which is saved in the database and makes a queue in rabbitmq with sender`s name   </li>
   <li>	The other user  will recevice the message from the queue  </li>
   <li>Message contains the recipient username and id and the sender username and sender id and time and conent </li>
   <li>	Clean architecture was used to create the system. and “i repository” pattren and unit of work </li>
   <li>	The solution in visual studio  devided into 5 projects 
      <ol>
         <li> Desktopapp contains win forms  </li>
          <li>Webapp  contains  asp.net mvc applcation controllers and views  </li>
        <li>	 ApplicationCore contains DTO and Services for crud opperaions  </li>
        <li>Infrastructrue contains domain classes and connection to database and migrations  </li>
         <li> Events.messages contains common and constans for rabbitmq configurations  </li>
      </ol>
   </li>
   <li>	Make sure connection string for database in Desktopapp and webapp and Infrastructrue are the same </li>
   <li>	And rabbitmq is established </li>
</ul>
 
    
  
 
### Tools :  
<ul>
  <li>RabbitMQ</li>
   <li>Sql server</li>
   <li>Asp.net mvc </li>
   <li>Asp.net win forms </li>
   <li>Entity frame work code first and fluent api </li>
   <li>Jquery </li>
   <li>Ajex </li>
   <li>Bootstrap 4  </li>
   <li>Jquery plugins (time -ago)  </li>
</ul>
    
